using System;
using System.IO;
using System.Text;
using AssetBundles;
using UnityEditor;
using UnityEngine;

public enum HotFixOption
{
    Intranet,   // 内网
    Cdn,        // 外网
    None,       // 不开启热更
    Customize,  // 自定义热更路径
}

public enum VersionOption
{
    PromotePatch,           // 提升最后补丁位
    PromoteMinorVersion,    // 提升一位小版本，同时把补丁位清0
    PromoteLargeVersion,    // 提升一位大版本， 同时把小版本和补丁位清0
    CustomizeVersion        // 使用自定义版本信息
}

public enum BuildMode
{
    Package,    // 出包
    Patch,      // 补丁
    All,        // 出包+补丁
}

public class GameBuilder 
{
    /// <summary>
    /// 游戏版本创建参数
    /// </summary>
    public class GameBuilderParameter
    {
        public BuildMode BuildMode { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public BuildTarget BuildTarget { get; set; }

        /// <summary>
        /// 是否启用热更
        /// </summary>
        public bool EnableHotFix { get; set; }

        public string HotFixUrl { get; set; }

        /// <summary>
        /// copy fmod
        /// </summary>

        public bool CopyFmod { get; set; }

        /// <summary>
        /// 增量式构建
        /// </summary>
        public bool IncreativeBuildAssetBundles { get; set; }

        /// <summary>
        /// 开启bundle压缩
        /// </summary>
        public bool BundleCompress { get; set; }

        /// <summary>
        /// 强制应用所有rule
        /// </summary>
        public bool ApplyAllRule { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>

        public AppVersion BuildVersion { get; set; }

        public BuildOptions BuildOptions { get; set; }

        public string OutputPath { get; set; }

        public bool UseMono2X { get; set; }

        public bool Multithreaded { get; set; }
        
        public string MacroDefines { get; set; }

        public bool ExportProject { get; set; }

        public bool UseGradle { get; set; }

        public string BundleIdentifier { get; set; }

        public long BuildNumber { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("BuildMode:{0} \n", BuildMode));
            sb.Append(String.Format("BuildTarget:{0} \n" , BuildTarget));
            sb.Append(String.Format("EnableHotFix:{0} \n", EnableHotFix));
            sb.Append(String.Format("HotFixUrl:{0} \n", HotFixUrl));
            sb.Append(String.Format("CopyFmod:{0} \n", CopyFmod));
            sb.Append(String.Format("IncreativeBuildAssetBundles:{0} \n", IncreativeBuildAssetBundles));
            sb.Append(String.Format("BundleCompress:{0} \n", BundleCompress));
            sb.Append(String.Format("ApplyAllRule:{0} \n", ApplyAllRule));
            sb.Append(String.Format("BuildVersion:{0} \n", BuildVersion));
            sb.Append(String.Format("BuildOptions:{0} \n", BuildOptions.ToString()));
            sb.Append(String.Format("OutputPath:{0} \n", OutputPath));
            sb.Append(String.Format("UseMono2X:{0} \n", UseMono2X));
            sb.Append(String.Format("Multithreaded:{0} \n", Multithreaded));
            sb.Append(String.Format("MacroDefines:{0} \n", MacroDefines));
            sb.Append(String.Format("ExportProject:{0} \n", ExportProject));
            sb.Append(String.Format("UseGradle:{0} \n", UseGradle));
            sb.Append(String.Format("BundleIdentifier:{0} \n", BundleIdentifier));

            return sb.ToString();
        }

    }

    public static bool BuildGame(GameBuilderParameter para)
    {
        Debug.Log("Build Game Start and Para is:" + para);

        if (!para.BuildVersion.CheckConstraint())
        {
            Debug.LogError("BuildVersion not pass constraint! version:" + para.BuildVersion.GetVersionString());
            return false;
        }

        try
        {
            if (para.BuildTarget == BuildTarget.iOS)
            {
                para.UseMono2X = false;
                Debug.Log("ios platform always use IL2ccpp. is not will compiler error");
            }

            // 处理热更url
            if (!HandleHotFix(para))
            {
                return false;
            }
            Debug.Log("Clear " + Application.streamingAssetsPath);
            FileUtils.ClearDirectoryReadOnly(Application.streamingAssetsPath);
            FileUtils.ClearDirectory(Application.streamingAssetsPath);
            SetupSetting(para);

            // build lua bundle
            Debug.Log("Start MarkBundlesNames ");
            ToLuaMenu.MarkBundlesNames();
            Debug.Log("End MarkBundlesNames ");

            if (para.ApplyAllRule)
            {
                Debug.Log("Start ApplyAllRule " );
                AssetBundleRuleUtils.ApplyAll(null, true);
                Debug.Log("ApplyAllRule finish ");
            }

            string sourcePath = Path.Combine(Utility.AssetBundlesOutputPath, Utility.GetPlatformName());
            if (!para.IncreativeBuildAssetBundles)
            {
                Debug.Log("Clear Assetbundle dir:" + sourcePath);
                FileUtils.ClearDirectoryReadOnly(sourcePath);
                FileUtils.ClearDirectory(sourcePath);
                Debug.Log("finish Clear Assetbundle dir:" );
            }
            
            Debug.Log("BuildAssetBundles to " + sourcePath);
            BuildScript.BuildAssetBundles(null, para.BundleCompress ? ToLuaMenu.GetBuildAssetBundleOptions(): BuildAssetBundleOptions.None);
            Debug.Log("BuildAssetBundles finish ");

            Debug.Log("Start CopyDirectory to Streaming");
            FileUtils.CopyDirectory(sourcePath, VersionConst.StreamingVersionPath);
            Debug.Log("Finish CopyDirectory to Streaming");

            // 这个放在下面，不然会被前面的删除掉
            if (para.CopyFmod)
            {
                // -音效引擎相关代码，放在打包资源之后因为会清理打包资源-//
                Debug.Log("CopyToStreamingAssets---------------------------------------");
                //EventManager.DoCopyToStreamingAssets();
                Debug.Log("MasterBankName---------------------------------------");
            }
            Debug.Log("GenHotFixFileListAndCheckConflict");
            AssetBundleRuleUtils.GenHotFixFileListAndCheckConflict();
            bool bOk = true;
            if (para.BuildMode == BuildMode.Patch || para.BuildMode == BuildMode.All)
            {
                Debug.Log("开始生成补丁");
                VersionPatchBuilder patchBuilder = new VersionPatchBuilder(para.BuildTarget, para.BuildVersion, para.BuildNumber);
                if (!patchBuilder.Build())
                {
                    bOk = false;
                }
                
                Debug.Log("结束生成补丁");
            }

            // 如果要求生成补丁，但补丁失败，则包也
            if (bOk && (para.BuildMode == BuildMode.Package || para.BuildMode == BuildMode.All))
            {
                Debug.Log("开始写入发布时版本信息:" + VersionConst.ReleasedVersionPath);
                // 写入发布版本信息
                SaveReleasedVersionFile(para.BuildVersion, VersionConst.ReleasedVersionPath);
                Debug.Log("结束写入发布时版本信息");

                Debug.Log("开始生成安装包");
                string[] levels = BuilderEditorUtils.GetBuildScenes();
                var error = BuildPipeline.BuildPlayer(levels
                    , para.OutputPath
                    , para.BuildTarget
                    , para.BuildOptions);
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError("Build failed: " + error);
                    bOk = false;
                }
                Debug.Log("结束生成安装包");


            }

            return bOk;
        }
        catch (Exception e)
        {
            Debug.LogError("error BuildGame stderror exception: " + e.Message);
            return false;
        }
    }

    private static void SaveReleasedVersionFile(AppVersion appVersion, string savedPath)
    {
        DoSaveVersionFile(appVersion, savedPath);
        AssetDatabase.Refresh();
        VersionConst.RefleshVerionInfo();
    }

    public static void DoSaveVersionFile(AppVersion appVersion, string savedPath)
    {
        FileUtils.DelFile(savedPath);
        Debug.Log("GameBuilder BuildVersion:" + appVersion);
        FileUtils.WriteFile(savedPath, appVersion.ToString());
    }

    /// <summary>
    /// 构建设置
    /// </summary>
    /// <param name="para"></param>
    private static void SetupSetting(GameBuilderParameter para)
    {
        Debug.Log("SetupSetting");
        BuildTarget buildTarget = para.BuildTarget;
        BuildTargetGroup buildTargetGroup = PlatformEditorUtils.GetSelectedBuildTargetGroup(buildTarget);

        //EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        EditorUserBuildSettings.SwitchActiveBuildTarget( buildTarget);

        // 仅当有出包时才处理
        if (para.BuildMode != BuildMode.Patch)
        {
            // 版本号
            PlayerSettings.bundleVersion = para.BuildVersion.GetVersionString();
            //PlayerSettings.applicationIdentifier = para.BundleIdentifier;

            if (buildTarget == BuildTarget.Android)
            {
                PlayerSettings.Android.bundleVersionCode = para.BuildVersion.GetVersionCode();

                if (para.ExportProject)
                {
                    EditorUserBuildSettings.androidBuildSystem =
                        para.UseGradle ? AndroidBuildSystem.Gradle : AndroidBuildSystem.ADT;
                }
                else
                {
                    EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Internal;
                }
            }
            else if (buildTarget == BuildTarget.iOS)
            {
                PlayerSettings.iOS.buildNumber = para.BuildVersion.GetVersionCode().ToString();
            }

            //-如果是多线程渲染版本开启多线程渲染-//
            //PlayerSettings.SetMobileMTRendering(buildTargetGroup, para.Multithreaded);
            PlayerSettings.MTRendering = para.Multithreaded;

            //-打包之前先设置一下 预定义标签, 这时设置的宏，后面的代码不能马上生效,但BuildPlayer可以生效
            string macroDefines = para.MacroDefines;
            if (!String.IsNullOrEmpty(macroDefines))
            {
                macroDefines = para.UseMono2X ?
                    BuilderEditorUtils.AddMacro(macroDefines, "USE_MONO") :
                    BuilderEditorUtils.RemoveMacro(macroDefines, "USE_MONO");

                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, macroDefines);
            }

            DoMonoSetting(para.UseMono2X, buildTargetGroup);
        }
    }

    /// <summary>
    /// save hotfixUrl and hotfix macro
    /// </summary>
    /// <param name="para"></param>
    /// <returns></returns>
    private static bool HandleHotFix(GameBuilderParameter para)
    {
        if (para.EnableHotFix)
        {
            var url = para.HotFixUrl;

            if (String.IsNullOrEmpty(url))
            {
                Debug.LogError("BuildGame HotFixUrl is empty");
                return false;
            }

            FileUtils.WriteFile(GameConst.GameResourceRootDir + "/" + VersionConst.CustomSeverUrlFileName, url);

            url = FileUtils.RemoveLastSeparator(url);
            FileUtils.DelFile(GameConst.GameResourceRootDir + "/" + VersionConst.CustomSeverUrlFileName);
            FileUtils.WriteFile(GameConst.GameResourceRootDir + "/" + VersionConst.CustomSeverUrlFileName, url);

            para.MacroDefines = BuilderEditorUtils.RemoveMacro(para.MacroDefines, BuilderEditorUtils.NoHotFixMacro);
            para.MacroDefines =
                BuilderEditorUtils.AddMacro(para.MacroDefines, BuilderEditorUtils.CustomServerHotFixMacro);
        }
        else
        {
            para.MacroDefines =
                BuilderEditorUtils.RemoveMacro(para.MacroDefines, BuilderEditorUtils.CustomServerHotFixMacro);
            para.MacroDefines = BuilderEditorUtils.AddMacro(para.MacroDefines, BuilderEditorUtils.NoHotFixMacro);
        }
        return true;
    }

    private static void RemoveWetestFiels(
		string u3DAutomationTargetPath,
		string libcrashmonitorTargetPath,
		string u3DautomationTargetPath)
	{
		FileUtil.DeleteFileOrDirectory(u3DAutomationTargetPath);
		FileUtil.DeleteFileOrDirectory(u3DAutomationTargetPath + ".meta");
		FileUtil.DeleteFileOrDirectory(libcrashmonitorTargetPath);
		FileUtil.DeleteFileOrDirectory(libcrashmonitorTargetPath + ".meta");
		FileUtil.DeleteFileOrDirectory(u3DautomationTargetPath);
		FileUtil.DeleteFileOrDirectory(u3DautomationTargetPath + ".meta");
	}

	private static void DoMonoSetting(bool useMono2X, BuildTargetGroup buildTargetGroup)
    {
        // 设置mono2x可以用来测试
        PlayerSettings.SetScriptingBackend(
            buildTargetGroup,
            useMono2X ? ScriptingImplementation.Mono2x : ScriptingImplementation.IL2CPP);
        ScriptingImplementation backend = PlayerSettings.GetScriptingBackend(buildTargetGroup);

        if (useMono2X && backend != ScriptingImplementation.Mono2x)
        {
            Debug.LogError("Warning: If the scripting backend is not Mono2x there may be problems");
        }
        else if (!useMono2X && backend != ScriptingImplementation.IL2CPP)
        {
            Debug.LogError("Warning: If the scripting backend is not IL2CPP there may be problems");
        }

        Debug.Log("useMono2x:" + useMono2X);
        Debug.Log("is Mono2x:" + (backend == ScriptingImplementation.Mono2x));
    }


    public static string GetHotFixUrl(HotFixOption option, string defaultUrl)
    {
        string url = defaultUrl;
        switch (option)
        {
            case HotFixOption.Intranet:
                url = VersionConst.IntranatHotFixCdn;
                break;
            case HotFixOption.Cdn:
                url = VersionConst.HotFixUrl;
                break;
            case HotFixOption.None:
                url = String.Empty;
                break;
            case HotFixOption.Customize:
                break;
            default:
                throw new ArgumentOutOfRangeException("option", option, null);
        }

        return url;
    }

    /// <summary>
    /// 获得构建版本
    /// </summary>
    /// <param name="versionOption"></param>
    /// <param name="customizeVersion"></param>
    /// <param name="buildTarget"></param>
    /// <returns></returns>
    public static AppVersion GetBuildVersion(VersionOption versionOption, AppVersion customizeVersion, BuildTarget buildTarget)
    {
        AppVersion lastBuildVersion = GetLastBuildVersion(buildTarget);
        AppVersion newVersion = lastBuildVersion;
        switch (versionOption)
        {
            case VersionOption.PromotePatch:
            {
                if(lastBuildVersion == AppVersion.InitVersion)
                {
                    Debug.Log("try check is first version build");
                    // 是否是第一个版本，如果是就不增加补丁位。为了处理第一次打包
                    string dir = PlatformEditorUtils.GetBuildToolVersionPatchDir(buildTarget, AppVersion.InitVersion);
                    if (!Directory.Exists(dir) || Directory.GetDirectories(dir).Length == 0)
                    {
                        Debug.Log("it's first version build");
                        break;
                    }
                }
                newVersion.Revision += 1;
            }
            break;
            case VersionOption.PromoteMinorVersion:
            {
                newVersion.MinorVersion += 1;
                newVersion.Revision = 0;
            }
            break;
            case VersionOption.PromoteLargeVersion:
            {
                newVersion.MajorVersion += 1;
                newVersion.MinorVersion = 0;
                newVersion.Revision = 0;
            }
            break;
            case VersionOption.CustomizeVersion:
            {
                newVersion = customizeVersion;
            }
            break;
            default:
                throw new ArgumentOutOfRangeException("versionOption", versionOption, null);
        }

        Debug.LogFormat("GetBuildVersion lastBuildVersion:{0}  newVersion:{1}", lastBuildVersion, newVersion);
        return newVersion;
    }

    /// <summary>
    /// 获得补丁版本
    /// </summary>
    public static AppVersion GetLastBuildVersion(BuildTarget buildTarget)
    {
        string path = PlatformEditorUtils.GetBuildToolVersionDir(buildTarget) + "/" + VersionConst.VersionFileName;
        if (!File.Exists(path))
        {
            return new AppVersion();
        }

        string versionStr = File.ReadAllText(path);
        return  new AppVersion(versionStr);
    }
}

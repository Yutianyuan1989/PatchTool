using System;
using System.IO;
using UnityEngine;

/// <summary>
/// 补丁目录的结构：(这里以Android举例)
///     Android
///         PatchInfo.json    //记录着各个版本补丁信息
///         0.0.0
///             各个补丁.zip
///         0.1.0
///             各个补丁.zip
///         (其它版本号)
///     iOS
///     Win
/// </summary>
public class VersionConst
{

    /// <summary>
    /// 是否需要版本更新
    /// </summary>
    /// <returns></returns>
    public static bool NeedVersionUpdate()
    {
        return VersionUpdateConfig.Mode != VersionUpdateMode.Disable;
    }

    public static bool UseDevelopCdn { get; set; }

    /// <summary>
    /// 配置的更新热更地址
    /// </summary>
    private static string ConfigHotFixUrl = string.Empty;

    /// <summary>
    /// 自定义的服务url存放文件名
    /// </summary>
    public const string CustomSeverUrlFileName = "AssetBundleServerURL.bytes";

    /// <summary>
    /// 外网的更新地址
    /// </summary>
    public const string HotFixCdn = "http://wgamecdn.lilithcdn.com/HotFix";

    /// <summary>
    /// 内网的更新地址
    /// </summary>
    public const string IntranatHotFixCdn = "http://192.168.1.198:8086";
    /// <summary>
    /// 补丁的根目录
    /// </summary>
    public static string HotFixUrl
    {
        get
        {
            if (VersionUpdateConfig.Mode == VersionUpdateMode.Disable)
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(ConfigHotFixUrl))
            {
                return ConfigHotFixUrl;
            }

            if (VersionUpdateConfig.Mode == VersionUpdateMode.CustomServer)
            {
                string path = PatchResourceRoot + "/" + CustomSeverUrlFileName;
                if (FileUtils.IsFileExist(path))
                {
                    ConfigHotFixUrl = File.ReadAllText(path);
                }

                if (string.IsNullOrEmpty(ConfigHotFixUrl))
                {
                    TextAsset txt = Resources.Load<TextAsset>(FileUtils.EraseExtension(CustomSeverUrlFileName));
                    if (txt != null)
                    {
                        ConfigHotFixUrl = txt.text;
                        if (!string.IsNullOrEmpty(ConfigHotFixUrl) && ConfigHotFixUrl[ConfigHotFixUrl.Length - 1] == '/')
                        {
                            ConfigHotFixUrl = ConfigHotFixUrl.Substring(0, ConfigHotFixUrl.Length - 1);
                        }
                    }
                }
            }
            else if(VersionUpdateConfig.Mode == VersionUpdateMode.LocalhostServer)
            {
                ConfigHotFixUrl = "http://localhost:7888/";
            }

            return ConfigHotFixUrl;
        }
    }

    /// <summary>
    /// 记录版本的文件名
    /// </summary>
    public const string VersionFileName = "LastBuildVersion.txt";

    /// <summary>
    /// 发布时版本文件路径
    /// </summary>
    public static string ReleasedVersionPath
    {
        get { return string.Format("{0}/{1}", GameConst.GameResourceRootDir, VersionFileName); }
    }

    /// <summary>
    /// 是否加载版本文件
    /// </summary>
    private static bool _isLoadVersionInfo;

    /// <summary>
    /// 发布时的版本信息
    /// </summary>
    private static AppVersion _releaseAppVersion;

    /// <summary>
    /// 发布版本信息
    /// </summary>
    public static AppVersion ReleaseAppVersion
    {
        get
        {
            if (!_isLoadVersionInfo)
            {
                RefleshVerionInfo();

                _isLoadVersionInfo = true;
            }

            return _releaseAppVersion;
        }
    }

    /// <summary>
    /// 强制刷新版本信息
    /// </summary>
    public static void RefleshVerionInfo()
    {
        TextAsset ta = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(VersionFileName));
        if (ta != null)
        {
            try
            {
                _releaseAppVersion = new AppVersion(ta.text);
                Debug.Log("_releaseAppVersion:" + _releaseAppVersion);
            }
            catch (Exception)
            {
                Debug.LogError("load relesed version info error!!");
            }
        }
    }


    /// <summary>
    /// 补丁信息的文件名
    /// </summary>
    public const string PatchInfoFileName = "PatchInfo.json";


    /// <summary>
    /// 该发布版本的补丁地址根目录
    /// </summary>

    public static string PatchUrl
    {
        get { return HotFixUrl + "/" + GameConst.OsDir + "/" + ReleaseAppVersion.GetVersionString(); }
    }


    /// <summary>
    /// 补丁信息url
    /// </summary>
    public static string VersionPatchInfoUrl
    {
        get { return HotFixUrl + "/" + GameConst.OsDir + "/" + PatchInfoFileName; }
    }

    /// <summary>
    /// 补丁根目录
    /// </summary>
    public static string PatchCacheRoot
    {
        get
        {
            return Application.persistentDataPath + "/" + GameConst.OsDir + "/" + ReleaseAppVersion.GetVersionString() ;
        }
    }

    /// <summary>
    /// 应用补丁的资源目录
    /// </summary>
    /// <returns></returns>
    public static string PatchResourceRoot
    {
        get
        {
            return PatchCacheRoot + "/Resources";
        }
    }

    /// <summary>
    /// 补丁临时目录，用于存放zip包的
    /// </summary>
    public static string PatchTmpRoot
    {
        get
        {
            return Application.persistentDataPath + "/" + GameConst.OsDir + "/" + ReleaseAppVersion.GetVersionString() + "/Tmp";
        }
    }

    /// <summary>
    /// 记录打过补丁的版本信息
    /// </summary>
    /// <returns></returns>
    public static string PatchedVersionPath
    {
        get
        {
            return PatchCacheRoot + "/" + VersionFileName;
        }
    }

    /// <summary>
    /// streaming 路径
    /// </summary>
    public static string StreamingPath
    {
        get
        {
#if !UNITY_EDITOR && UNITY_ANDROID
        return Application.dataPath + "!assets";
#else
            return Application.streamingAssetsPath;
#endif            
        }
    }

    /// <summary>
    /// streaming下面的补丁相关目录
    /// </summary>
    public static string StreamingVersionPath
    {
        get
        {
            return StreamingPath + "/" + GameConst.OsDir;
        }
    }

    /*
     * 为何要用两个路径，主要想同步加载，所以要放一份在Resource下面。
     * 但又不想把它打成assetbundle.所以不好放在streaming目录。因为这个不好同步加载
     * 所以放两份。
     * todo 如果测试稳定了可以把它打成assetbundle
     */
    
    /// <summary>
    /// 未打包的文件相关信息
    /// </summary>
    public static string RawFileRecordName = "rawFileRecords.txt";

    /// <summary>
    /// 记录原始文件发布时Resource路径,方便加载
    /// </summary>
    public static string ReleasedRawFileRecordResourcePath
    {
        get { return GameConst.GameResourceRootDir + "/" + RawFileRecordName; }
    }

    /// <summary>
    /// 记录原始文件发布时Streaming路径. 这个主要为后面补丁对比用
    /// </summary>
    public static string ReleasedRawFileRecordStreamingPath
    {
        get { return StreamingVersionPath + "/" + RawFileRecordName; }
    }
}
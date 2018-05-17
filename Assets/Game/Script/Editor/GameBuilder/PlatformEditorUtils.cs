#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

#endif

public class PlatformEditorUtils
{
#if UNITY_EDITOR
    /// <summary>
    /// 获得不同平台的后缀
    /// </summary>
    /// <param name="buildTarget"></param>
    /// <returns></returns>
    public static string GetAppExt(BuildTarget buildTarget)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            return ".ipa";
        }
        else if (buildTarget == BuildTarget.Android)
        {
            return ".apk";
        }
        else if (buildTarget == BuildTarget.StandaloneWindows ||
                 buildTarget == BuildTarget.StandaloneWindows64)
        {
            return ".exe";
        }

        Debug.LogError("Unknown target！");

        return ".apk";
    }

    /// <summary>
    /// 设置平台，设置成字符串
    /// </summary>
    /// <param name="buildTarget"></param>
    public static string GetPlatformDesc(BuildTarget buildTarget)
    {
        string target = "Windows";
        if (buildTarget == BuildTarget.iOS)
        {
            target = "iOS";
        }

        if (buildTarget == BuildTarget.Android)
        {
            target = "Android";
        }

        if (buildTarget == BuildTarget.StandaloneWindows)
        {
            target = "Windows";
        }
        return target;
    }


	/// <summary>
	/// 获得不同平台的app名字
	/// </summary>
	/// <param name="buildTarget"></param>
	/// <returns></returns>
	public static string GetAppName(BuildTarget buildTarget, string appName = "LatestVersion")
    {
        return GetPlatformDesc(buildTarget) + "/" + appName + GetAppExt(buildTarget);
    }

    public static BuildTarget GetSelectedBuildTarget(BuildTargetGroup buildTargetGroup)
    {
        switch (buildTargetGroup)
        {
            case BuildTargetGroup.Standalone:
                return EditorUserBuildSettings.selectedStandaloneTarget;

            case BuildTargetGroup.iOS:
                return BuildTarget.iOS;

            case BuildTargetGroup.Android:
                return BuildTarget.Android;

            case BuildTargetGroup.tvOS:
                return BuildTarget.tvOS;

            case BuildTargetGroup.Tizen:
                return BuildTarget.Tizen;

            case BuildTargetGroup.XboxOne:
                return BuildTarget.XboxOne;

            case BuildTargetGroup.PSP2:
                return BuildTarget.PSP2;

            case BuildTargetGroup.PS4:
                return BuildTarget.PS4;

            case BuildTargetGroup.WSA:
                return BuildTarget.WSAPlayer;

            case BuildTargetGroup.WebGL:
                return BuildTarget.WebGL;

            case BuildTargetGroup.SamsungTV:
                return BuildTarget.SamsungTV;

            default:
                return EditorUserBuildSettings.activeBuildTarget;
        }
    }

    public static BuildTargetGroup GetSelectedBuildTargetGroup(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {
            case BuildTarget.iOS:
                return BuildTargetGroup.iOS;

            case BuildTarget.Android:
                return BuildTargetGroup.Android;

            case BuildTarget.tvOS:
                return BuildTargetGroup.tvOS;

            case BuildTarget.Tizen:
                return BuildTargetGroup.Tizen;

            case BuildTarget.XboxOne:
                return BuildTargetGroup.XboxOne;

            case BuildTarget.PSP2:
                return BuildTargetGroup.PSP2;

            case BuildTarget.PS4:
                return BuildTargetGroup.PS4;

            case BuildTarget.WSAPlayer:
                return BuildTargetGroup.WSA;

            case BuildTarget.WebGL:
                return BuildTargetGroup.WebGL;

            case BuildTarget.SamsungTV:
                return BuildTargetGroup.SamsungTV;

            default:
                return BuildTargetGroup.Standalone;
        }
    }

    /// <summary>
    /// 获得build tool 路径
    /// </summary>
    /// <returns></returns>
    public static string GetBuildToolPath()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
        if (directoryInfo.Parent != null && directoryInfo.Parent.Parent != null)
        {
            string buildToolDir = directoryInfo.Parent.Parent + "/tools/BuildTool";
            var replace = buildToolDir.Replace('/', '\\');
            return buildToolDir;
        }

        return string.Empty;
    }

    /// <summary>
    /// 获得输出的版本路径
    /// </summary>
    /// <param name="buildTarget"></param>
    /// <returns></returns>
    public static string GetBuildToolVersionDir(BuildTarget buildTarget)
    {
        return GetBuildToolPath() + "/Output/" + GetPlatformDesc(buildTarget) + "/Version";
    }

    /// <summary>
    /// 获得补丁路径
    /// </summary>
    /// <param name="buildTarget"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public static string GetBuildToolVersionPatchDir(BuildTarget buildTarget, AppVersion version)
    {
        string relativePath = string.Format("{0}.{1}/{2}", version.MajorVersion, version.MinorVersion, version.Revision);
        return GetBuildToolVersionDir(buildTarget) + "/" + relativePath;
    }

    /// <summary>
    /// 获得大版本路径
    /// </summary>
    /// <param name="buildTarget"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public static string GetBuildToolVersionMajorMinorDir(BuildTarget buildTarget, AppVersion version)
    {
        string relativePath = string.Format("{0}.{1}", version.MajorVersion, version.MinorVersion);
        return GetBuildToolVersionDir(buildTarget) + "/" + relativePath;
    }

    public static string GetBuildToolPackageDir(BuildTarget buildTarget)
    {
        return GetBuildToolPath() + "/Output/" + GetPlatformDesc(buildTarget) + "/Package";
    }

    /// <summary>
    /// 主要存放iOS导出的工程
    /// </summary>
    /// <param name="buildTarget"></param>
    /// <returns></returns>
    public static string GetBuildToolProjectDir(BuildTarget buildTarget)
    {
        return GetBuildToolPath() + "/Output/" + GetPlatformDesc(buildTarget) + "/Project";
    }
#endif
}

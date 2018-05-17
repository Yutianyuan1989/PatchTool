#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


public enum VersionUpdateMode
{
    /// <summary>
    /// 不进行热更
    /// </summary>
    Disable,

    /// <summary>
    /// 使用AssetBundleServerURL.bytes里的地址进行更新(外网cdn也通过这个方式来配置)
    /// </summary>
    CustomServer,

    /// <summary>
    /// 使用 http://localhost:7888/ 地址来更新. 这个打包时不要用，只是为了测试方便
    /// </summary>
    LocalhostServer,
}

public class VersionUpdateConfig
{
    /// <summary>
    /// 使用哪种更新模式
    /// </summary>
    const string kVersionUpdateModeOption = "kVersionUpdateModeOption";
    private static VersionUpdateMode mVersionUpdateMode = VersionUpdateMode.Disable;

    private static bool mIsInit = false;

    /// <summary>
    /// 使用哪种更新模式
    /// </summary>
    public static VersionUpdateMode Mode
    {
        get
        {
            if (!mIsInit)
            {
                mVersionUpdateMode = GetDefaultUpdateMode();
                mIsInit = true;
            }
            return mVersionUpdateMode;
        }
        set
        {
            if (value != mVersionUpdateMode)
            {
                mVersionUpdateMode = value;
#if UNITY_EDITOR
                EditorPrefs.SetInt(kVersionUpdateModeOption, (int)value);
#endif
            }
        }
    }

    private static VersionUpdateMode GetDefaultUpdateMode()
    {
#if UNITY_EDITOR
        return (VersionUpdateMode)EditorPrefs.GetInt(kVersionUpdateModeOption, (int)VersionUpdateMode.Disable);
#elif NoHotFix
            return VersionUpdateMode.Disable;
#elif CustomServerHotFix
            return VersionUpdateMode.CustomServer;
#else
            return VersionUpdateMode.Disable;
#endif
    }
}

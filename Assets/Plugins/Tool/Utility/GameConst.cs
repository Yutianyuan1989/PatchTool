using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GameConst
{
    /// <summary>
    /// 对应的平台目录
    /// </summary>
    public static string OsDir
    {
        get
        {
#if UNITY_STANDALONE || UNITY_STANDALONE_WIN
            string osDir = "Windows";
#elif UNITY_ANDROID
            string osDir = "Android";
#elif UNITY_IPHONE
            string osDir = "iOS";        
#else
            string osDir = "";        
#endif

            return osDir;
        }
    }

    /// <summary>
    /// 游戏的Resources目录
    /// </summary>
    public static string GameResourceRootDir
    {
        get
        {
            return Application.dataPath + "/WGame/Resources";
        }
    }

    /// <summary>
    /// 热更资源的相对路径
    /// </summary>
    public const string GameHotfixRelativeRootDir = "Assets/WGame/HotFixResources/";

    /// <summary>
    /// 场景相对路径
    /// </summary>
    public const string GameScenesRelativeRootDir = "Assets/WGame/Scenes/";

    /// <summary>
    /// Resources资源的相对路径
    /// </summary>
    public const string GameResourceRelativeRootDir = "Assets/WGame/Resources/";

}

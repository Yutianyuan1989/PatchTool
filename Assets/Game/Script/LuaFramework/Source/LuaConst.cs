using UnityEngine;

public static class LuaConst
{
    public static string LuaFrameworkRootDir = Application.dataPath + "/Scripts/LuaFramework";
    public static string GameResourceRootDir = Application.dataPath + "/Resources";
    public static string GameResourceLuaDir = GameResourceRootDir + "/Lua";         //Resourc下的lua目录

    public static string LuaExportRelativeTempDir = "temp/Lua";
    public static string LuaExportTempDir = Application.dataPath + "/" + LuaExportRelativeTempDir;

    /// <summary>
    /// 如果要做assetbundle的md5对比的话，把这个设置成false, 就不删除临时目录了，
    /// 不然每次生成的assetbundle的md5都不一样
    /// </summary>
    public static bool RemoveLuaExportTempDir = false;

    public static string luaDir = LuaFrameworkRootDir + "Game/Lua";                //lua逻辑代码目录
    public static string toluaDir = LuaFrameworkRootDir + "Game/ToLua/Lua";        //tolua lua文件目录
    public static string generateDir = Application.dataPath + "/Game/Script/LuaFramework/Source/Generate/";
    public static string toluaBaseTypeDir = LuaFrameworkRootDir + "/ToLua/BaseType/";

    public static string osDir = GameConst.OsDir;
    public static string LuaPrefixPath = "assets/temp/lua";
    public static string LuaPersistentDir = string.Format("{0}/{1}/{2}", Application.persistentDataPath,osDir, LuaPrefixPath);      //手机运行时lua文件下载目录    
    public static string LuaStreamingDir = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, osDir, LuaPrefixPath);

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN    
    public static string zbsDir = "D:/ZeroBraneStudio/lualibs/mobdebug";        //ZeroBraneStudio目录       
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
	public static string zbsDir = "/Applications/ZeroBraneStudio.app/Contents/ZeroBraneStudio/lualibs/mobdebug";
#else
    public static string zbsDir = LuaPersistentDir + "/mobdebug/";
#endif    

    public static bool openLuaSocket = true;            //是否打开Lua Socket库
    public static bool openLuaDebugger = false;         //是否连接lua调试器
}
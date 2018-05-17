using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuilderEditorUtils
{
/// <summary>
    /// 在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 总之返回一个字符串数组。
    /// </summary>
    /// <returns></returns>
    public static string[] GetBuildScenes(Func<EditorBuildSettingsScene, bool> filterFunc=null)
	{
		List<string> names = new List<string>();
        
		foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
		{
            if (e == null)
            {
                continue;
            }

            if( filterFunc == null && e.enabled 
                || filterFunc!=null && filterFunc(e))
            {
                names.Add(e.path);
            }
		}

		return names.ToArray();
	}

    private const char MacroSeparator = ';';

    /// <summary>
    /// 热更的宏定义名字
    /// </summary>
    public const string NoHotFixMacro = "NoHotFix";
    public const string CustomServerHotFixMacro = "CustomServerHotFix";


    /// <summary>
    /// 添加宏
    /// </summary>
    /// <param name="defines"></param>
    /// <param name="macro"></param>
    /// <returns></returns>
    public static string AddMacro(string defines, string macro)
    {
        if (string.IsNullOrEmpty(defines))
        {
            return macro;
        }

        if (!ExistMacro(defines, macro))
        {
            var splitDefines = defines.Split(MacroSeparator).ToList();
            splitDefines.Add(macro);
            return string.Join(MacroSeparator.ToString(), splitDefines.ToArray());
        }

        return defines;
    }

    public static string RemoveMacro(string defines, string macro)
    {
        if (ExistMacro(defines, macro))
        {
            var splitDefines = defines.Split(MacroSeparator).ToList();
            
            splitDefines.RemoveAll(a => string.Compare(a, macro, StringComparison.OrdinalIgnoreCase) == 0);
            return string.Join(MacroSeparator.ToString(), splitDefines.ToArray());
        }

        return defines;
    }


    /// <summary>
    /// 是不是存在宏
    /// </summary>
    /// <param name="defines">全部的宏</param>
    /// <param name="macro">要查找的宏</param>
    /// <returns></returns>
    public static bool ExistMacro(string defines, string macro)
    {
        if (string.IsNullOrEmpty(defines) || string.IsNullOrEmpty(macro))
        {
            return false;
        }

        return !string.IsNullOrEmpty(defines.Split(MacroSeparator)
                .FirstOrDefault(a => string.Compare(a, macro, StringComparison.OrdinalIgnoreCase) == 0));
    }
}

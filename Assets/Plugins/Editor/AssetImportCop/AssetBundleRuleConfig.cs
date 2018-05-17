using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
[CreateAssetMenu(menuName = "Create AssetBundle Config")]
public class AssetBundleRuleConfig : ScriptableObject
{
    const string SettingsAssetName = "AssetBundleRuleGlobalConfig";
    /// <summary>
    /// 包含的目录
    /// </summary>
    public List<string> ContainDirs = new List<string>();

    // 这里不用_assetbundle, 因为AssetBundlesBrower会把.assetbundle当做Variant
    // 会出现以下的错误
    /*
     * AssetBundleBrowser: Bundle 'assets/test/test2' has a name conflict with a bundle-folder.
     * Display of bundle data and building of bundles will not work.
     * Details: If you name a bundle 'x/y', then the result of your build will be a bundle named 'y' in a folder named 'x'.  
     * You thus cannot also have a bundle named 'x' at the same level as the folder named 'x'.
     */
    /// <summary>
    /// assetbundle 后缀
    /// todo 看是不是要变成可配置的
    /// </summary>
    public static string AssetbundleExtention = "_assetbundle";

    private static AssetBundleRuleConfig instance = null;
    public static AssetBundleRuleConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load(SettingsAssetName) as AssetBundleRuleConfig;

                //if (instance == null)
                //{
                //    instance = CreateInstance<AssetBundleRuleConfig>();
                //    AssetDatabase.CreateAsset(instance, "Assets/WGame/Resources/" + SettingsAssetName + ".asset");
                //    AssetDatabase.SaveAssets();
                //    AssetDatabase.Refresh();

                //    Debug.Log("AssetBundle Rule Config: cannot find Rule Config, creating default Rule Config");
                //}

            }
            return instance;
        }
    }

    /// <summary>
    /// 不在可应用的目录
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool CanApply(string path)
    {
        foreach (var dir in ContainDirs)
        {
            if (path.StartsWith(dir, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

}
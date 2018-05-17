using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetBundleRuleUtils
{

    public class AssetBundleRuleFindResult
    {
        public AssetBundleRuleFindResult(AssetBundleRule rule, string assetPath )
        {
            Rule = rule;
            RulePath = AssetDatabase.GetAssetPath(rule);
            AssetPath = assetPath;

            // 同一级目录的话，就应用文件规则。 否则应用子目录规则
            IsApplyFileRule = Path.GetDirectoryName(RulePath) == Path.GetDirectoryName(assetPath);
        }

        /// <summary>
        /// rule
        /// </summary>
        public AssetBundleRule Rule { get; private set; }

        /// <summary>
        /// 使用文件规则
        /// </summary>
        public bool IsApplyFileRule { get; private set; }

        /// <summary>
        /// 找到的rule设置路径
        /// </summary>
        public string RulePath { get; private set; }

        /// <summary>
        /// 要查找的资源路径
        /// </summary>
        public string AssetPath { get; private set; }
    }


    // todo 弄成可配置的
    /// <summary>
    /// 设置的根目录
    /// </summary>
    public static string BundlesRoot = "Assets/WGame/Resources/";


    // 这里不用_assetbundle, 因为AssetBundlesBrower会把.assetbundle当做Variant
    // 会出现以下的错误
    /*
     * AssetBundleBrowser: Bundle 'assets/test/test2' has a name conflict with a bundle-folder.
     * Display of bundle data and building of bundles will not work.
     * Details: If you name a bundle 'x/y', then the result of your build will be a bundle named 'y' in a folder named 'x'.  
     * You thus cannot also have a bundle named 'x' at the same level as the folder named 'x'.
     */
    public const string AssetbundleExtention = "_assetbundle";

    /// <summary>
    /// 延迟设置bundleName的列表
    /// </summary>
    private static List<AssetBundleRuleFindResult> sDelayUpdateBundleNameList = new List<AssetBundleRuleFindResult>();

    /// <summary>
    /// 延迟清理bundleName的列表
    /// </summary>
    private static List<string> sDelayClearBundleNameList = new List<string>();

    /// <summary>
    /// 是否正在应用规则
    /// </summary>
    public static  bool IsApplyingRule;

    /// <summary>
    /// 应用该规则文件
    /// </summary>
    /// <param name="assetBundleRule"></param>
    public static void ApplyRule(AssetBundleRule assetBundleRule)
    {
        if (assetBundleRule == null)
        {
            Debug.LogError("ApplyRule assetBundleRule == null");
            return;
        }

        // get the directories that we do not want to apply changes to 

        var assetrulepath = AssetDatabase.GetAssetPath(assetBundleRule).Replace(assetBundleRule.name + ".asset", "")
            .TrimEnd('/');
        string projPath = Application.dataPath;
        projPath = projPath.Remove(projPath.Length - 6);

        string fullPath = projPath + AssetDatabase.GetAssetPath(assetBundleRule);
        string dirPath = Path.GetDirectoryName(fullPath);
        if (string.IsNullOrEmpty(dirPath))
        {
            Debug.LogError("ApplyRule dirPath == null fullPath:" + fullPath);
            return;
        }

        // 找到不能应用该规则的子目录(就是有自己规则的目录)
        List<string> dontapply = new List<string>();
        var assetGuids = AssetDatabase.FindAssets("t:AssetBundleRule", new[] { assetrulepath });

        foreach (var guid in assetGuids)
        {
            //  排除当前rule目录
            string assetDir = Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(guid));
            if (!string.IsNullOrEmpty(assetDir) && String.Compare(assetDir, assetrulepath,StringComparison.OrdinalIgnoreCase) != 0)
            {
                var d = assetDir.Replace(Application.dataPath, "Assets");
                d = d.TrimEnd('/');
                d = d.Replace('\\', '/');

                // 增加自己目录
                if (!dontapply.Contains(d))
                {
                    dontapply.Add(d);
                }

                // 有assetBundle rule文件的话， 子目录都应该排除掉
                string[] directories = Directory.GetDirectories(d, "*", SearchOption.AllDirectories);
                foreach (var oneDir in directories)
                {
                    var dir = oneDir.TrimEnd('/');
                    dir = dir.Replace('\\', '/');
                    if (!dontapply.Contains(dir))
                    {
                        dontapply.Add(dir);
                    }
                }
            }
        }

        // 遍历所有文件， 找到合适的文件
        List<string> finalAssetList = new List<string>();
        var allAssets = AssetDatabase.FindAssets("", new[] {assetrulepath});
        foreach (var findAsset in allAssets)
        {
            var asset = AssetDatabase.GUIDToAssetPath(findAsset);

            if(!IsValidBundlePath(asset)) continue;
            if(IsAssetBundleRuleFile(asset)) continue;
            if (dontapply.Contains(Path.GetDirectoryName(asset))) continue;
            if (finalAssetList.Contains(asset)) continue;
            if (asset == AssetDatabase.GetAssetPath(assetBundleRule)) continue;
            finalAssetList.Add(asset);
        }

        // 设置每个文件的规则
        foreach (var asset in finalAssetList)
        {
            ApplyRuleToOneFile(new AssetBundleRuleFindResult(assetBundleRule, asset));
        }
    }


    /// <summary>
    /// 把该rule应用到该文件上
    /// </summary>
    /// <param name="bundleRule"></param>
    private static void ApplyRuleToOneFile(AssetBundleRuleFindResult bundleRule)
    {
        // 如果直接在这里设置importer.assetBundleName的话， 会导致CacheServer Asset validation failed。
        // 应该是unity的bug。 只能绕过去了
        if (sDelayClearBundleNameList.Count == 0 && sDelayUpdateBundleNameList.Count == 0)
        {
            EditorApplication.update += Update;
        }
        sDelayUpdateBundleNameList.Add(bundleRule);
    }


    /// <summary>
    /// 查找该文件对应的规则文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static AssetBundleRuleFindResult FindRuleForAsset(string path)
    {
        AssetBundleRule rule = SearchRecursive(path);
        if (rule != null)
        {
            return new AssetBundleRuleFindResult(rule, path);
        }

        return null;
    }

    /// <summary>
    /// 递归向上查找规则文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static AssetBundleRule SearchRecursive(string path)
    {
        foreach (var findAsset in AssetDatabase.FindAssets("t:AssetBundleRule", new[] { Path.GetDirectoryName(path) }))
        {
            var p = Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(findAsset));
            if (p == Path.GetDirectoryName(path))
            {
                Debug.LogFormat("Found AssetBundleRule for Asset Rule({0}) for ({1})" , AssetDatabase.GUIDToAssetPath(findAsset), path);
                {
                    return AssetDatabase.LoadAssetAtPath<AssetBundleRule>(AssetDatabase.GUIDToAssetPath(findAsset));
                }
            }
        }
        //no match so go up a level
        path = Directory.GetParent(path).FullName;
        path = path.Replace('\\', '/');
        path = path.Remove(0, Application.dataPath.Length);
        path = path.Insert(0, "Assets");
        //Debug.Log("Searching: " + path);
        if (path != "Assets")
            return SearchRecursive(path);

        //no matches
        return null;
    }

    public static void ApplyRuleToOneFile(string assetPath)
    {
        AssetBundleRuleFindResult bundleRule = FindRuleForAsset(assetPath);

        if (bundleRule == null || bundleRule.Rule == null)
        {
            Debug.Log("No asset bundle rules found for asset");
            return;
        }

        ApplyRuleToOneFile(bundleRule);
    }


    public static void ClearAssetBundleName(string assetPath)
    {
        // 如果直接在这里设置importer.assetBundleName的话， 会导致CacheServer Asset validation failed。
        // 应该是unity的bug。 只能绕过去了
        if (sDelayClearBundleNameList.Count == 0 && sDelayUpdateBundleNameList.Count==0)
        {
            EditorApplication.update += Update;
        }
        sDelayClearBundleNameList.Add(assetPath);
    }

    /// <summary>
    /// 是否有效的bundle 路径
    /// </summary>
    /// <param name="path"></param>
    /// <param name="checkFileExist">delete会触发rule,但这时文件不存在了</param>
    /// <returns></returns>
    public static bool IsValidBundlePath(string path, bool checkFileExist = true)
    {
        // 排除文件夹
        if (Directory.Exists(path))
        {
            return false;
        }

        // 忽略的文件
        if (IsIgnoreFile(path))
        {
            return false;
        }

        // 文件不存在
        if (checkFileExist && !File.Exists(path))
        {
            return false;
        }

        // 必须在Application.dataPath目录
        if (Path.IsPathRooted(path) && !path.Contains(Application.dataPath))
        {
            return false;
        }

        //  是否满足我们的文件路径要求
        if (!AssetBundleRuleConfig.Instance.CanApply(path))
        {
            return false;
        }
        //if (!path.StartsWith(BundlesRoot, StringComparison.OrdinalIgnoreCase))
        //{
        //    return false;
        //}

        // todo 增加后缀处理
        // todo 增加黑名单处理

        return true;
    }

    public static bool IsIgnoreFile(string path)
    {
        // 忽略打包时lua的目录
        return string.Compare(Path.GetExtension(path), ".cs", StringComparison.OrdinalIgnoreCase) == 0
            || path.StartsWith("Assets/temp/Lua");
    }

    /// <summary>
    /// 非热更文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool IsNoHotFixFile(string path)
    {
        // 忽略打包时lua的目录
        path = path.Replace('\\', '/');
        return string.Compare(Path.GetExtension(path), ".psd", StringComparison.OrdinalIgnoreCase) == 0
            || path.EndsWith("/Editor",StringComparison.OrdinalIgnoreCase)
            || path.IndexOf("/Editor/", StringComparison.OrdinalIgnoreCase)>=0;
    }

    /// <summary>
    /// 是否是rule文件
    /// </summary>
    /// <returns></returns>
    public static bool IsAssetBundleRuleFile(string path)
    {
        //var allRuleFiles = AssetDatabase.FindAssets("t:AssetBundleRule", new[] { Path.GetDirectoryName(path) }).Select(AssetDatabase.GUIDToAssetPath).ToList();
        //return allRuleFiles.Contains(path);

        if (Path.GetExtension(path) != ".asset")
        {
            return false;
        }

        return AssetDatabase.FindAssets("t:AssetBundleRule", new[] {Path.GetDirectoryName(path)})
                   .FirstOrDefault(a => AssetDatabase.GUIDToAssetPath(a) == path) != null;

    }


    /// <summary>
    /// 是否该目录存在类型文件
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool ExistAssetInDir(string dir, string type)
    {
        return AssetDatabase.FindAssets(string.Format("t:{0}", type), new[] { dir })
                   .FirstOrDefault(a => Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(a))==dir) != null;
    }



    public static AssetBundleRule GetAssetBundleRuleFile(string assetPath)
    {
        return AssetDatabase.LoadAssetAtPath<AssetBundleRule>(AssetDatabase.GUIDToAssetPath(assetPath));
    }


    /// <summary>
    /// bundlename的实际调协
    /// </summary>
    private static void Update()
    {
        IsApplyingRule = true;
        int count = sDelayUpdateBundleNameList.Count + sDelayClearBundleNameList.Count;
        int index = 0;
        foreach (var result in sDelayUpdateBundleNameList)
        {
            index++;
            EditorUtility.DisplayProgressBar(string.Format("AssetBundle Rule Apply ({0}/{1})", index, count), string.Format("{0}",  result.AssetPath), index / (float)count);
            result.Rule.ApplySettings(AssetImporter.GetAtPath(result.AssetPath), result.IsApplyFileRule, result.RulePath);
        }

        foreach (var path in sDelayClearBundleNameList)
        {
            index++;
            EditorUtility.DisplayProgressBar(string.Format("AssetBundle Rule Apply ({0}/{1})", index, count), string.Format("{0}", path), index / (float)count);
            var importer = AssetImporter.GetAtPath(path);
            if (importer != null)
            {
                importer.SetAssetBundleNameAndVariant(string.Empty, string.Empty);
            }
        }

        EditorUtility.ClearProgressBar();
        IsApplyingRule = false;
        sDelayUpdateBundleNameList.Clear();
        sDelayClearBundleNameList.Clear();

        AssetDatabase.RemoveUnusedAssetBundleNames();

        if (EditorApplication.update != null)
        {
            EditorApplication.update -= Update;
        }
    }

    /// <summary>
    /// 获得所有asset bundle rule
    /// </summary>
    /// <returns></returns>
    public static List<AssetBundleRule> GetAllAssetBundleRules()
    {
        var ruleObjects = new List<AssetBundleRule>();

        ruleObjects.Clear();
        var settings = AssetBundleRuleConfig.Instance;
        if (settings != null && settings.ContainDirs.Count > 0)
        {
            var assetPaths = AssetDatabase.FindAssets("t:AssetBundleRule", settings.ContainDirs.ToArray()).Select(AssetDatabase.GUIDToAssetPath);

            foreach (var assetPath in assetPaths)
            {
                var rule = AssetDatabase.LoadAssetAtPath<AssetBundleRule>(assetPath);
                if (rule == null)
                {
                    Debug.LogError("AssetBundleRule cannt get at:" + assetPath);
                    continue;
                }

                ruleObjects.Add(AssetDatabase.LoadAssetAtPath<AssetBundleRule>(assetPath));
            }
        }
        else
        {
            Debug.Log("Could not found AssetBundleRules.  settings is null :" + (settings == null));
        }

        return ruleObjects;
    }

    public static void ApplyAll(List<AssetBundleRule> ruleObjects, bool immedate)
    {
        ruleObjects = ruleObjects ?? GetAllAssetBundleRules();
        foreach (var oneAssetBundleRule in ruleObjects)
        {
            if (oneAssetBundleRule != null)
            {
                ApplyRule(oneAssetBundleRule);
            }
        }

        if (immedate)
        {
            Update();
        }
    }

    /// <summary>
    /// 保存文件列表
    /// </summary>
    public static void GenHotFixFileListAndCheckConflict()
    {
        // 不带后缀的相对路径|后缀|是否更新资源|assetbundle名字
        List<string> fileList = new List<string>();
        string[] allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
        foreach (var oneName in allAssetBundleNames)
        {
            // ignore lua file
            if (oneName.StartsWith("assets/temp/lua"))
            {
                continue;
            }
            var assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(oneName);
            Dictionary<string, bool> pathWithoutExtDict = new Dictionary<string, bool>();
            foreach (var onePath in assetPaths)
            {
                string pathWithoutExt = Path.GetDirectoryName(onePath) + "/" + Path.GetFileNameWithoutExtension(onePath);
                string oneFileInfo = string.Format("{0}|{1}|{2}|{3}",
                    pathWithoutExt,
                    Path.GetExtension(onePath),
                    0, oneName);
                fileList.Add(oneFileInfo);

                if (pathWithoutExtDict.ContainsKey(pathWithoutExt))
                {
                    Debug.LogError(string.Format("assetPath: ({0}) found conflict. may file names same but extention diffent", onePath), AssetDatabase.LoadAssetAtPath<Object>(onePath));
                }
                else
                {
                    pathWithoutExtDict.Add(pathWithoutExt, true);
                }
            }
        }

        string rawFileRecordResourcePath = VersionConst.ReleasedRawFileRecordResourcePath;
        string rawFileRecordStreamingPath = VersionConst.ReleasedRawFileRecordStreamingPath;

        if (File.Exists(rawFileRecordResourcePath))
        {
            File.Delete(rawFileRecordResourcePath);
        }

        if (File.Exists(rawFileRecordStreamingPath))
        {
            File.Delete(rawFileRecordStreamingPath);
        }

        File.WriteAllText(rawFileRecordResourcePath, string.Join("\n", fileList.ToArray()));
        File.WriteAllText(rawFileRecordStreamingPath, string.Join("\n", fileList.ToArray()));
    }
}
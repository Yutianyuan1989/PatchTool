using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AssetBundleRule))]
public class AssetRuleInspector : Editor
{
    private AssetBundleRule orig;

    private AssetBundleRuleImportSettings currentSetting;

    [MenuItem("Assets/Create AssetBundle Rule")]
    public static void CreateAssetRule()
    {
        var newRule = CreateInstance<AssetBundleRule>();
        newRule.ApplyDefaults();

        string selectionpath = "Assets";
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            selectionpath = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(selectionpath))
            {
                selectionpath = Path.GetDirectoryName(selectionpath);
            }
            break;
        }

        if (AssetBundleRuleUtils.ExistAssetInDir(selectionpath, "AssetBundleRule"))
        {
            Debug.LogError("该文件已经存在AssetBundle Rule文件， 不能同时存在两份");
            return;
        }

        string newRuleFileName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(selectionpath, "New AssetBundle Rule.asset"));
        newRuleFileName = newRuleFileName.Replace("\\", "/");
        AssetDatabase.CreateAsset(newRule, newRuleFileName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newRule;
    }

    private string FileSettingHelp = @"
设置当前目录文件的assetbundle name:
    Ignore:  忽略该目录文件assetbundle name相关
    Empty:  清空assetbundle name
    FilePath:  用文件路径做为assetbundle name
    FilePathRemoveTagAndExt： 用移除后缀标签及扩展名的路径做assetbundle name
    CurrentFolderName:  当前文件夹名字做为assetbundle name
            ";

    private string SubDirSettingHelp = @"
设置子目录文件的assetbundle name:
    Ignore:  忽略子目录文件的assetbundle name相关设置
    Empty:  清空子目录文件的assetbundle name
    FilePath:  用文件路径做为assetbundle name
    FilePathRemoveTagAndExt： 用移除后缀标签及扩展名的路径做assetbundle name
    CurrentFolderName:  当前文件夹名字做为assetbundle name
    FolderNameInRuleDir:  使用自己对应的Rule文件里文件夹名字做为assetbundle name
    RuleDir:              使用放rule文件的文件夹名
            ";


    public override void OnInspectorGUI()
    {
        var t = (AssetBundleRule)target;

        //EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("AssetBundle Rule:");

        EditorGUILayout.Space();
        EditorStyles.helpBox.fontSize = 14;
        EditorGUILayout.HelpBox(FileSettingHelp, MessageType.Info);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("当前目录文件AssetBundle设置");

        currentSetting.FileSetting = (AssetBundleFileSetting) EditorGUILayout.EnumPopup(currentSetting.FileSetting);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(SubDirSettingHelp, MessageType.Info);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("子目录文件AssetBundle设置:");
        currentSetting.SubFolderSetting = (AssetBundleSubFolderSetting)EditorGUILayout.EnumPopup(currentSetting.SubFolderSetting);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        //if (EditorGUI.EndChangeCheck ()) 
        //{
        //    changed = true;
        //}

        string dir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(t));
        if (AssetBundleRuleConfig.Instance.CanApply(dir))
        {
            // todo 分开处理文件和子目录变更
            if (changed && GUILayout.Button("Apply"))
            {
                 Apply(t);
            }

            if (GUILayout.Button("Force Apply"))
            {
                if (EditorUtility.DisplayDialog("Force Apply", "是否强制刷新该规则", "Yes","No"))
                {
                    Apply(t);
                }
            }
        }
        else
        {
            EditorGUILayout.HelpBox("无法应用rule文件，请先在Resources/AssetBundleRuleGlobalConfig里添加可应用目录", MessageType.Error);
        }
    }

    private void Apply(AssetBundleRule assetBundleRule)
    {
        AssetBundleRuleUtils.ApplyRule(assetBundleRule);

        // save rule setting
        EditorUtility.SetDirty(assetBundleRule);

        //AssetDatabase.RemoveUnusedAssetBundleNames();
        //changed = false;
        //currentSetting = orig.settings;

        orig.settings = currentSetting;
    }



    private bool changed
    {
        get { return currentSetting.FileSetting != orig.settings.FileSetting 
              || currentSetting.SubFolderSetting != orig.settings.SubFolderSetting; }
    }
    
    void OnEnable()
    {
        //changed = false;
        orig = (AssetBundleRule) target;
        currentSetting = orig.settings;

       Undo.RecordObject(target,"assetbundleruleundo");
    }

    void OnDisable()
    {
        if (changed)
        {
            string dir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(orig));
            if (AssetBundleRuleConfig.Instance.CanApply(dir) && EditorUtility.DisplayDialog("Unsaved Settings", "Unsaved AssetBundleRule Changes", "Apply", "Revert"))
            {
                Apply((AssetBundleRule)target);
            }
            else
            {
                Undo.PerformUndo();
                currentSetting = orig.settings;
                //SerializedObject so = new SerializedObject(target);
                //so.SetIsDifferentCacheDirty();
                //so.Update();
            }
        }
        
        //changed = false;


    }
}
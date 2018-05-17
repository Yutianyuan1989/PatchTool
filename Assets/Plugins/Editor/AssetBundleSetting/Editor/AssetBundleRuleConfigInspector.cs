using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(AssetBundleRuleConfig))]
class AssetBundleRuleConfigInspector:Editor
{
    //[MenuItem("Assets/Create AssetBundle Config")]
    public static void CreateAssetRuleConfig()
    {
        var newRule = CreateInstance<AssetBundleRuleConfig>();

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

        // 是否已经存在
        bool bFound = AssetDatabase.FindAssets("t:AssetBundleRuleConfig").Length > 0;
        if (bFound)
        {
            Debug.LogError("只能放一份全局的AssetBundleRuleConfig配置文件");
            return;
        }

        if (selectionpath != null)
        {
            string newRuleFileName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(selectionpath, "AssetBundleRuleGlobalConfig.asset"));
            newRuleFileName = newRuleFileName.Replace("\\", "/");
            AssetDatabase.CreateAsset(newRule, newRuleFileName);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newRule;
    }


    private List<AssetBundleRule> ruleObjects;
    private AssetBundleRuleConfig settings;

    public override void OnInspectorGUI()
    {
        if (settings == null)
        {
            return;
        }

        GUIStyle style = new GUIStyle(GUI.skin.label);

        EditorGUILayout.LabelField("AssetBundle Rule可以应用的目录:", style);
        EditorGUI.indentLevel++;
        if (settings.ContainDirs.Count != 0)
        {
            for (int i = 0; i < settings.ContainDirs.Count; i++)
            {
                string oldPath = settings.ContainDirs[i];
                EditorGUILayout.BeginHorizontal();
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 100;

                EditorGUILayout.PrefixLabel("RulePath:", GUI.skin.textField, style);
                
                EditorGUIUtility.labelWidth = labelWidth;
                GUI.enabled = false;
                EditorGUILayout.LabelField(settings.ContainDirs[i]);
                GUI.enabled = true;

                if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
                {
                    GUI.FocusControl(null);

                    string path = EditorUtility.OpenFolderPanel("Locate AssetBundle Rule Root Dir", oldPath, null);
                    if (!string.IsNullOrEmpty(path))
                    {
                        if (ValidatePath(path))
                        {
                            path = MakePathRelativeToProject(path);
                            settings.ContainDirs[i] = path;
                            EditorUtility.SetDirty(settings);
                        }

                    }
                }
                if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false)))
                {
                    settings.ContainDirs.RemoveAt(i);
                    EditorUtility.SetDirty(settings);
                    RefleshRuleInfos();
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.LabelField("您还没有添加AssetBundle Rule根目录", style);
        }
        EditorGUI.indentLevel--;

        EditorGUILayout.Space();
        
        if (GUILayout.Button("增加可应用AssetBundle Rule根目录"))
        {
            GUI.FocusControl(null);

            string path = EditorUtility.OpenFolderPanel("Locate AssetBundle Rule Root Dir", Application.dataPath, null);
            if (!string.IsNullOrEmpty(path))
            {
                if (ValidatePath(path))
                {
                    path = MakePathRelativeToProject(path);
                    settings.ContainDirs.Add(path);
                    EditorUtility.SetDirty(settings);
                    RefleshRuleInfos();
                }
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("已配置可应用的所有规则:", style);
        EditorGUI.indentLevel++;
        if (ruleObjects != null && ruleObjects.Count>0)
        {
            foreach (var ruleObject in ruleObjects)
            {

                string dir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(ruleObject));
                EditorGUILayout.LabelField(dir + ":");

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField("AssetBundle Rule:", ruleObject, typeof(AssetBundleRule), false);
                EditorGUI.EndDisabledGroup();
            }
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.Space();

        if (GUILayout.Button("Apply All Rule"))
        {
            if (EditorUtility.DisplayDialog("Apply All Rule", "您确定要重新应用所有的rule吗", "Yes", "No"))
            {
                if (ruleObjects != null)
                {
                    AssetBundleRuleUtils.ApplyAll(ruleObjects, false);
                }

                Debug.Log("Apply All OK");
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("生成热更资源路径文件并且检测冲突"))
        {
            GenFileListAndCheckConflict();

            Debug.Log("生成热更资源路径文件并且检测冲突完成");
        }
    }

    /// <summary>
    /// 保存文件列表
    /// </summary>
    private static void GenFileListAndCheckConflict()
    {
        AssetBundleRuleUtils.GenHotFixFileListAndCheckConflict();
    }

    bool ValidatePath(string path)
    {
        if (!path.Contains(Application.dataPath))
        {
            EditorUtility.DisplayDialog("Add Rule Root Error", "请选择Assets下的目录", "OK");
            return false;
        }

        path = MakePathRelativeToProject(path);

        if (settings != null)
        {
            foreach (var path2 in settings.ContainDirs)
            {
                var p = path2.Replace("\\", "/");

                if (p.Contains(path) || path.Contains(p))
                {
                    EditorUtility.DisplayDialog("Add Rule Root Error", "该目录包含了其它目录或者被其它目录所包含，请确认", "OK");
                    return false;
                }
            }
        }

        return true;
    }

    void RefleshRuleInfos()
    {
        ruleObjects = AssetBundleRuleUtils.GetAllAssetBundleRules();
    }

    public static string MakePathRelativeToProject(string path)
    {
        string fullPath = Path.GetFullPath(path);
        string fullProjectPath = Path.GetFullPath(Environment.CurrentDirectory + Path.DirectorySeparatorChar);
        return fullPath.Replace(fullProjectPath, "").Replace("\\","/");
    }
    void OnEnable()
    {
        settings = target as AssetBundleRuleConfig;
        RefleshRuleInfos();
    }
}
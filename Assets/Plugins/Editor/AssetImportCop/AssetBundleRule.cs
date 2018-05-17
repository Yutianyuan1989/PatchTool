using UnityEngine;
using UnityEditor;
using System.IO;
using System;

/// <summary>
/// ��ǰĿ¼�µ��ļ�����
/// </summary>
[System.Serializable]
public enum AssetBundleFileSetting
{
    Ignore,                 // ����assetbundle name�������
    Empty,                  // ���assetbundle name
    FilePath,               // ���ļ�·����Ϊassetbundle name
    CurrentFolderName,      // ��ǰ�ļ���������Ϊassetbundle name
    FilePathRemoveTagAndExt,// �Ƴ���׺��ǩ����չ��
}


/// <summary>
/// ��Ŀ¼�µ��ļ�����
/// </summary>
[System.Serializable]
public enum AssetBundleSubFolderSetting
{
    Ignore,                 // ������Ŀ¼�ļ���assetbundle name�������
    Empty,                  // �����Ŀ¼�ļ���assetbundle name
    FilePath,               // ���ļ�·����Ϊassetbundle name
    CurrentFolderName,      // ��ǰ�ļ���������Ϊassetbundle name
    FolderNameInRuleDir,    // �Լ�����Rule���ò����ļ�������
    RuleDir,                // ��rule�ļ����ļ�����
    FilePathRemoveTagAndExt,      // �Ƴ���׺��ǩ����չ��
}

[System.Serializable]
public class AssetBundleRule : ScriptableObject
{
    public AssetBundleRuleImportSettings settings;

    public static AssetBundleRule CreateAssetRule()
    {
        var assetRule = AssetBundleRule.CreateInstance<AssetBundleRule>();

        assetRule.ApplyDefaults();

        return assetRule;
    }

    public void ApplyDefaults()
    {
        settings.ApplyDefaults();
    }

	public void ApplySettings(AssetImporter importer, bool useFileSetting, string rulePath)
	{
		settings.Apply(importer, useFileSetting, rulePath);
	}
}


[System.Serializable]
public struct AssetBundleRuleImportSettings
{
	public AssetBundleFileSetting FileSetting;
	public AssetBundleSubFolderSetting SubFolderSetting;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="importer"></param>
    /// <param name="useFileSetting"></param>
    /// <param name="rulePath"></param>
	public void Apply(AssetImporter importer, bool useFileSetting, string rulePath)
	{
	    if (useFileSetting)
	    {
	        ApplyFileSetting(importer);
        }
	    else
	    {
	        ApplySubFoldrSetting(importer, rulePath);
	    }
	    
	}


    private string EraseExtension(string fullName)
    {
        if (fullName == null)
        {
            return string.Empty;
        }
        int length = fullName.LastIndexOf('.');
        if (length > 0)
        {
            return fullName.Substring(0, length);
        }
        return fullName;
    }

    /// <summary>
    /// ���rootĿ¼��һ����pathĿ¼·��
    /// </summary>
    /// <returns></returns>
    private string GetTopPath(string path, string root)
    {
        string rootDirectory = Path.GetDirectoryName(root);
        while (!string.IsNullOrEmpty(path) && Path.GetDirectoryName(path) != rootDirectory)
        {
            path = Directory.GetParent(path).ToString();
        }

        return path;
    }

    public void ApplyFileSetting(AssetImporter importer)
    {
        if (AssetBundleRuleUtils.IsNoHotFixFile(importer.assetPath))
        {
            importer.assetBundleName = string.Empty;
            return;
        }

        switch (FileSetting)
        {
            case AssetBundleFileSetting.Ignore:
                break;
            case AssetBundleFileSetting.Empty:
                importer.assetBundleName = string.Empty;
                break;
            case AssetBundleFileSetting.FilePath:
                importer.assetBundleName = GetAssetBundleNameByFilePath(importer.assetPath);
                break;
            case AssetBundleFileSetting.CurrentFolderName:
                importer.assetBundleName = Path.GetDirectoryName(importer.assetPath) + AssetBundleRuleConfig.AssetbundleExtention;
                break;
            case AssetBundleFileSetting.FilePathRemoveTagAndExt:
                if (!string.IsNullOrEmpty(importer.assetPath))
                {
                    var assetBundleName = GetAssetbundleNameByRemoveTagAndExt(importer.assetPath);
                    importer.assetBundleName = assetBundleName;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// ���bundleNameͨ��RemoveTagAndExt
    /// ��Ҫ���������Ĺ��� xxxx_����[(_xx)*]
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    private string GetAssetbundleNameByRemoveTagAndExt(string assetPath)
    {
        string dir = Path.GetDirectoryName(assetPath);
        string fileName = Path.GetFileNameWithoutExtension(assetPath);
        if (string.IsNullOrEmpty(fileName))
        {
            return string.Empty;
        }

        char tagSeparator = '_';
        int lastTagIndex = fileName.Length;
        int currentTagIndex = fileName.LastIndexOf(tagSeparator);

        string handledFileName = string.Empty;
        while (currentTagIndex != -1)
        {
            int tagValue;
            if (string.IsNullOrEmpty(handledFileName))
            {
                // ���ҵ� (_����)ģʽ
                if (int.TryParse(fileName.Substring(currentTagIndex + 1, lastTagIndex - currentTagIndex - 1), out tagValue))
                {
                    handledFileName = fileName.Substring(0, currentTagIndex);
                }
            }
            else
            {
                // ���Ѿ��ҵ�(_����)ģʽ������£� ����ǰ������(_����)ģʽ �Ž���
                if (!int.TryParse(fileName.Substring(currentTagIndex + 1, lastTagIndex - currentTagIndex - 1), out tagValue))
                {
                    break;
                }
                else
                {
                    handledFileName = fileName.Substring(0, currentTagIndex);
                }
            }
            

            lastTagIndex = currentTagIndex;
            currentTagIndex = fileName.LastIndexOf(tagSeparator, lastTagIndex - 1);
        }

        string assetBundleName;
        if (string.IsNullOrEmpty(handledFileName))
        {
            assetBundleName = GetAssetBundleNameByFilePath(assetPath);
        }
        else
        {
            assetBundleName = dir +"/"+ handledFileName+ AssetBundleRuleConfig.AssetbundleExtention;
        }

        return assetBundleName;
    }

    public void ApplySubFoldrSetting(AssetImporter importer, string rulePath)
    {
        if (AssetBundleRuleUtils.IsNoHotFixFile(importer.assetPath))
        {
            importer.assetBundleName = string.Empty;
            return;
        }
        switch (SubFolderSetting)
        {
            case AssetBundleSubFolderSetting.Ignore:
                break;
            case AssetBundleSubFolderSetting.Empty:
                importer.assetBundleName = string.Empty;
                break;
            case AssetBundleSubFolderSetting.FilePath:
                importer.assetBundleName = GetAssetBundleNameByFilePath(importer.assetPath);
                break;
            case AssetBundleSubFolderSetting.CurrentFolderName:
                importer.assetBundleName = Path.GetDirectoryName(importer.assetPath) + AssetBundleRuleUtils.AssetbundleExtention;
                break;
            case AssetBundleSubFolderSetting.FolderNameInRuleDir:
                importer.assetBundleName = GetTopPath(importer.assetPath, rulePath) + AssetBundleRuleUtils.AssetbundleExtention;
                break;
            case AssetBundleSubFolderSetting.RuleDir:
                importer.assetBundleName = Path.GetDirectoryName(rulePath) + AssetBundleRuleUtils.AssetbundleExtention;
                break;
            case AssetBundleSubFolderSetting.FilePathRemoveTagAndExt:
                if (!string.IsNullOrEmpty(importer.assetPath))
                {
                    var assetBundleName = GetAssetbundleNameByRemoveTagAndExt(importer.assetPath);
                    importer.assetBundleName = assetBundleName;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// ����·�������assetbundle name
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    private string GetAssetBundleNameByFilePath(string assetPath)
    {
        string assetBundleName = string.Empty;
        if (!string.IsNullOrEmpty(assetPath))
        {
            string ext = Path.GetExtension(assetPath);
            if (string.IsNullOrEmpty(ext))
            {
                assetBundleName = EraseExtension(assetPath) + AssetBundleRuleConfig.AssetbundleExtention;
            }
            else
            {
                assetBundleName = EraseExtension(assetPath) + ext.Replace('.', '_') +
                                           AssetBundleRuleConfig.AssetbundleExtention;
            }
        }
        return assetBundleName;
    }

    public void ApplyDefaults()
    {
        FileSetting = AssetBundleFileSetting.Ignore;
        SubFolderSetting = AssetBundleSubFolderSetting.Ignore;
    }
}

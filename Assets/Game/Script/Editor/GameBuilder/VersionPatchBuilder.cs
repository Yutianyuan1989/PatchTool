using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WGame;

class VersionPatchBuilder
{
    /// <summary>
    /// 记录热更信息的文件
    /// </summary>
    public const string HotFixFileRecordName = "hotFileRecords.txt";

    /// <summary>
    /// 版本输出路径
    /// </summary>
    private readonly string VersionOutputDir;

    /// <summary>
    /// 该次构建的版本根目录,路径格式：版本号/时间戳
    /// </summary>
    private readonly string _currentBuildVersionRootPath;

    /// <summary>
    /// 版本信息
    /// </summary>
    private readonly AppVersion _appVersion;

    /// <summary>
    /// build号
    /// </summary>
    private readonly long _buildNum;

    private readonly BuildTarget _buildTarget;

    /// <summary>
    /// 这个版本的时间戳文件夹名
    /// </summary>
    private readonly string _timeVersionDirName;

    /// <summary>
    /// 生成的补丁名
    /// </summary>
    private string _patchName = string.Empty;

    /// <summary>
    /// 该版本的总体信息目录
    /// </summary>
    public string SnapshotDir
    {
        get { return _currentBuildVersionRootPath + "/Snapshot"; }
    }

    /// <summary>
    /// 该版本针对上一个版本的补丁目录
    /// </summary>
    public string PatchDir
    {
        get { return _currentBuildVersionRootPath + "/Patch"; }
    }

    public VersionPatchBuilder(BuildTarget buildTarget, AppVersion appVersion, long buildNum)
    {
        _appVersion = appVersion;
        _buildNum = buildNum;
        _buildTarget = buildTarget;
        VersionOutputDir = PlatformEditorUtils.GetBuildToolVersionDir(buildTarget);
        FileUtils.CreateDirectory(VersionOutputDir);

        string versionOutputRoot = PlatformEditorUtils.GetBuildToolVersionPatchDir(_buildTarget,_appVersion);
        FileUtils.CreateDirectory(versionOutputRoot);

        _timeVersionDirName = string.Format("{0:yyyy-MM-dd hh_mm_ss}({1})", DateTime.Now, buildNum);

        // 以当前的时间戳来命名文件夹
        _currentBuildVersionRootPath = versionOutputRoot + "/" + _timeVersionDirName;

        FileUtils.CreateDirectory(_currentBuildVersionRootPath);
        FileUtils.CreateDirectory(PatchDir);
        FileUtils.CreateDirectory(SnapshotDir);
    }

    public bool Build()
    {
        bool bSuccess = false;
        try
        {
            do
            {
                // check
                var versionUpdateInfo = Validate();
                if (versionUpdateInfo == null)
                {
                    break;
                }

                GenerateSnapshot();

                if (!MakePatch())
                {
                    break;
                }

                SaveFiles(versionUpdateInfo);

                bSuccess = true;
            } while (false);

        }
        catch (Exception)
        {
            bSuccess = false;
            throw;
        }
        finally
        {
            Clear(bSuccess);

        }
        return bSuccess;
    }


    public void SaveFiles(VersionUpdateInfo versionUpdateInfo)
    {
        GameBuilder.DoSaveVersionFile(_appVersion,
            PlatformEditorUtils.GetBuildToolVersionDir(_buildTarget) + "/" + VersionConst.VersionFileName);

        SaveUpdateInfo(versionUpdateInfo);

        CopyToDiffPath();
    }

    /// <summary>
    /// 主要为了Jenkins方便copy
    /// </summary>
    private void CopyToDiffPath()
    {
        string diffDir = VersionOutputDir + "/Diff";
        FileUtils.DelDir(diffDir);

        if (File.Exists(CurrentBuildUpdateInfoPath()))
        {
            FileUtils.CreateDirectory(diffDir);
            FileUtils.CopyFile(CurrentBuildUpdateInfoPath(), diffDir + "/" + VersionConst.PatchInfoFileName);
        }
        
        if (_appVersion.Revision != 0)
        {
            string patchDir = string.Format("{0}/{1}.{2}.0", diffDir, _appVersion.MajorVersion, _appVersion.MinorVersion);
            FileUtils.CreateDirectory(patchDir);

            string sourcePatchPath = PatchDir + "/" + _patchName + ".zip";
            FileUtils.CopyFile(sourcePatchPath, patchDir + "/" + _patchName + ".zip");
        }
    }

    /// <summary>
    /// 保存版本信息
    /// </summary>
    /// <param name="versionUpdateInfo"></param>
    private void SaveUpdateInfo(VersionUpdateInfo versionUpdateInfo)
    {
        if (_appVersion.Revision != 0)
        {
            AddPatchToVersionUpdateInfo(versionUpdateInfo);
        }

        var json = JsonUtility.ToJson(versionUpdateInfo, true);
        File.WriteAllText(CurrentBuildUpdateInfoPath(), json);
    }

    /// <summary>
    /// 当前版本的更新文件路径
    /// </summary>
    /// <returns></returns>
    private string CurrentBuildUpdateInfoPath()
    {
        return SnapshotDir + "/" + VersionConst.PatchInfoFileName;
    }

    /// <summary>
    /// 把生成的补丁放到版本更新里
    /// </summary>
    /// <param name="versionUpdateInfo"></param>
    private void AddPatchToVersionUpdateInfo(VersionUpdateInfo versionUpdateInfo)
    {
        foreach (var oneItem in versionUpdateInfo.VersionUpdateItems)
        {
            if (oneItem.Version == _appVersion)
            {
                List<PatchInfo> newPatchs = new List<PatchInfo>();
                if (oneItem.Patches != null)
                {
                    foreach (var onePatch in oneItem.Patches)
                    {
                        if (onePatch.RevisionNum >= _appVersion.Revision)
                        {
                            break;
                        }

                        newPatchs.Add(onePatch);
                    }
                }

                // 增加新增patch
                string patchPath = PatchDir + "/" + _patchName + ".zip";
                newPatchs.Add(new PatchInfo()
                {
                    PatchName = _patchName + ".zip",
                    FileSize = (ulong) FileUtils.GetFileLength(patchPath),
                    RevisionNum = _appVersion.Revision,
                    Md5 = GameUtil.md5file(patchPath)
                });

                oneItem.Patches = newPatchs;
                break;
            }
        }
    }

    /// <summary>
    /// 验证合理性
    /// </summary>
    /// <returns></returns>
    VersionUpdateInfo Validate()
    {
        VersionUpdateInfo updateInfo = GetPreviousVersionUpdateInfo(_appVersion);

        VersionUpdateItem currentVersionItem = null;

        foreach (var oneItem in updateInfo.VersionUpdateItems)
        {
            if (oneItem.Version.IsSameVersionSame(_appVersion))
            {
                currentVersionItem = oneItem;
                break;
            }
        }

        if (currentVersionItem != null)
        {
            // 如果版本都已经比我们>=了
            if (currentVersionItem.Version > _appVersion ||
                (currentVersionItem.Version == _appVersion && _appVersion.Revision != 0))
            {
                Debug.LogErrorFormat("cannt overwrite [{0}] , it >= my version [{1}]", currentVersionItem.Version,  _appVersion);
                return null;
            }
        }

        AppVersion previousVersion = currentVersionItem==null ? new AppVersion(): currentVersionItem.Version;
        if (_appVersion.Revision != 0 && previousVersion.Revision + 1 != _appVersion.Revision)
        {
            Debug.LogErrorFormat("previous version [{0}]'s revision add one != patched version [{1}]", previousVersion, _appVersion);
            return null;
        }

        // new update info
        if (currentVersionItem == null || !currentVersionItem.Version.IsSameVersionSame(_appVersion))
        {
            updateInfo.VersionUpdateItems.Add(new VersionUpdateItem()
            {
                Version = _appVersion
            });
        }
        else
        {
            currentVersionItem.Version = _appVersion;
        }

        return updateInfo;
    }

    /// <summary>
    /// 获得先前的版本更新信息
    /// </summary>
    /// <param name="appVersion"></param>
    /// <returns></returns>
    private VersionUpdateInfo GetPreviousVersionUpdateInfo(AppVersion appVersion)
    {
        // 缺省版本信息
        VersionUpdateInfo versionUpdateInfo = new VersionUpdateInfo()
        {
            VersionUpdateItems = new List<VersionUpdateItem>()
        };

        do
        {
            if (appVersion.Revision == 0)
            {
                break;
            }

            string previousVersionPath = GetPreviousLastestVersionPath(PreviousVersion());
            if (string.IsNullOrEmpty(previousVersionPath))
            {
                break;
            }

            string previousUpdateInfoPath = previousVersionPath + "/Snapshot/" + VersionConst.PatchInfoFileName;
            if (File.Exists(previousUpdateInfoPath))
            {
                var oldUpdateInfo = JsonUtility.FromJson<VersionUpdateInfo>(File.ReadAllText(previousUpdateInfoPath));
                if (oldUpdateInfo != null && oldUpdateInfo.VersionUpdateItems != null)
                {
                    versionUpdateInfo = oldUpdateInfo;
                }
            }

        } while (false);

        return versionUpdateInfo;
    }

    void OnError()
    {
        Debug.Log("补丁构建失败:" + _appVersion);
        FileUtils.DelDir(_currentBuildVersionRootPath);


        string path = PlatformEditorUtils.GetBuildToolVersionPatchDir(_buildTarget, _appVersion);
        if (Directory.Exists(path) && Directory.GetDirectories(path).Length == 0)
        {
            FileUtils.DelDir(path);
        }

        path = PlatformEditorUtils.GetBuildToolVersionMajorMinorDir(_buildTarget, _appVersion);
        if (Directory.Exists(path) && Directory.GetDirectories(path).Length == 0)
        {
            FileUtils.DelDir(path);
        }
    }
    void Clear(bool bSuccess)
    {
        if (!bSuccess)
        {
            OnError();
        }
        Debug.Log("VersionPatchBuilder Clear :" + bSuccess);
    }
    /// <summary>
    /// 生成快照
    /// </summary>
    private void GenerateSnapshot()
    {
        FileUtils.CopyFile(VersionConst.ReleasedRawFileRecordResourcePath, SnapshotDir + "/" + VersionConst.RawFileRecordName);
        RecordHotFixInfos(SnapshotDir);
    }

    /// <summary>
    /// 构建补丁
    /// </summary>
    /// <returns></returns>
    private bool MakePatch()
    {
        if (_appVersion.Revision == 0)
        {
            return true;
        }

        // get lastest version's hot file shapshot
        string previousHotFixRecordPath = GetPreviousLastestHotFixRecordPath(PreviousVersion());
        if (string.IsNullOrEmpty(previousHotFixRecordPath))
        {
            return false;
        }

        // 获得先前版本时间戳
        string previousVersionStr = GetTimeVersionStrByPath(previousHotFixRecordPath);
        if (string.IsNullOrEmpty(previousVersionStr))
        {
            return false;
        }
        // diff
        var newOrChangeList = DiffRecords(previousHotFixRecordPath, SnapshotDir + "/" + HotFixFileRecordName);

        // 生成补丁名
        _patchName = GenPactchName(previousVersionStr);

        string patchDir = PatchDir + "/" + _patchName;
        if (!ZipPatch(newOrChangeList, patchDir))
        {
            return false;
        }

       return true;
    }

    /// <summary>
    /// 把相关补丁打包
    /// </summary>
    /// <param name="newOrChangeList"></param>
    /// <param name="patchDir"></param>
    /// <returns></returns>
    private bool ZipPatch(List<string> newOrChangeList, string patchDir)
    {
        // 创建补丁目录
        string currentPatchDir = patchDir;
        FileUtils.CreateDirectory(currentPatchDir);

        // 把相关的补丁内容copy进来
        foreach (var item in newOrChangeList)
        {
            var strings = item.Split('|');
            string sourceRelativePath = strings[0];

            //Debug.Log("ZipPatch item:" + item);
            //Debug.Log("ZipPatch item222:" + Path.GetDirectoryName(currentPatchDir + "/" + sourceRelativePath));
            FileUtils.CreateDirectory(Path.GetDirectoryName(currentPatchDir + "/" + sourceRelativePath));

            
            string sourcePath = VersionConst.StreamingVersionPath + "/" + sourceRelativePath;
            string targetPath = currentPatchDir + "/" + sourceRelativePath;
            FileUtils.CopyFile(sourcePath, targetPath);
            //Debug.Log("sourcePath :" + sourcePath);
            //Debug.Log("targetPath :" + targetPath);

            // 把相关的manifest也拷进去
            if (File.Exists(sourcePath + ".manifest"))
            {
                FileUtils.CopyFile(sourcePath + ".manifest", targetPath + ".manifest");
            }
        }

        string patchZip = patchDir + ".zip";
        ZipHelper.ZipFileDirectory(currentPatchDir, patchZip);

        if (File.Exists(patchZip))
        {
            FileUtils.DelDir(currentPatchDir);
        }
        else
        {
            Debug.LogError("could not make patch zip:" + patchZip);
            return false;
        }

        return true;
    }

    /// <summary>
    /// 产生补丁名
    /// </summary>
    /// <param name="previousVersionStr"></param>
    /// <returns></returns>
    private string GenPactchName(string previousVersionStr)
    {
        int leftFlag = previousVersionStr.IndexOf("(", StringComparison.Ordinal);
        int rightFlag = previousVersionStr.IndexOf(")", StringComparison.Ordinal);
        int count = rightFlag - leftFlag - 1;
        string previoustBuildNum = previousVersionStr.Substring(leftFlag + 1, count);
        string patchName = string.Format("{0}_{1}_{2}", _appVersion.Revision, previoustBuildNum, _buildNum);
        return patchName;
    }

    /// <summary>
    /// 获得先前版本的最后一个hotFixRecord
    /// </summary>
    /// <param name="previousVersion"></param>
    /// <returns></returns>
    private string GetPreviousLastestHotFixRecordPath(AppVersion previousVersion)
    {
        string previousVersionPath = GetPreviousLastestVersionPath(previousVersion);
        Debug.Log("GetPreviousLastestVersionPath:" + previousVersionPath);
        if (string.IsNullOrEmpty(previousVersionPath))
        {
            return string.Empty;
        }

        string previousHotFixRecordPath = previousVersionPath + "/Snapshot/" + HotFixFileRecordName;
        if (!File.Exists(previousHotFixRecordPath))
        {
            Debug.LogError("could not found HotFixFileRecordName:" + previousHotFixRecordPath);
            return string.Empty;
        }

        return previousHotFixRecordPath;
    }

    /// <summary>
    /// 先前版本
    /// </summary>
    /// <returns></returns>
    private AppVersion PreviousVersion()
    {
        AppVersion previousVersion = _appVersion;
        previousVersion.Revision = (ushort)Math.Max(previousVersion.Revision-1,0);
        return previousVersion;
    }

    /// <summary>
    /// 比较生成获得两个版本的差异
    /// </summary>
    /// <param name="previousHotFixRecordPath"></param>
    /// <param name="currentHotFixRecordPath"></param>
    /// <returns></returns>
    private List<string> DiffRecords(string previousHotFixRecordPath, string currentHotFixRecordPath)
    {
        var previousAllLines = File.ReadAllLines(previousHotFixRecordPath);
        var currentAllLines = File.ReadAllLines(currentHotFixRecordPath);

        // 找到新增或者修改的
        List<string> newOrChangeList = new List<string>();
        foreach (var curOneLine in currentAllLines)
        {
            if (!previousAllLines.Contains(curOneLine))
            {
                newOrChangeList.Add(curOneLine);
            }
        }

        if (newOrChangeList.Count == 0)
        {
            Debug.LogWarningFormat("version {0} and {1} is same. not patch generate", previousHotFixRecordPath, currentHotFixRecordPath);
        }

        return newOrChangeList;
    }

    /// <summary>
    /// 根据路径，获得先前版本的时间戳
    /// </summary>
    /// <param name="previousHotFixRecordPath"></param>
    /// <returns></returns>
    private static string GetTimeVersionStrByPath(string previousHotFixRecordPath)
    {
        string previousRecordDir = Path.GetDirectoryName(previousHotFixRecordPath);
        if (string.IsNullOrEmpty(previousRecordDir))
        {
            Debug.LogError("could not found record dir:" + previousRecordDir);
            return string.Empty;
        }

        DirectoryInfo directoryInfo = new DirectoryInfo(previousRecordDir);
        if (directoryInfo.Parent == null)
        {
            Debug.LogError("could not found parent dir:" + previousRecordDir);
            return string.Empty;
        }

        return directoryInfo.Parent.Name;
    }

    /// <summary>
    /// 获得先前的版本时间戳目录
    /// </summary>
    /// <returns></returns>
    private string GetPreviousLastestVersionPath(AppVersion previousVersion)
    {
        string previousVersionOutputRoot = PlatformEditorUtils.GetBuildToolVersionPatchDir(_buildTarget, previousVersion);

        var previousPaths = Directory.GetDirectories(previousVersionOutputRoot);
        if (previousPaths.Length == 0)
        {
            Debug.LogError("could not found previous version info:" + previousVersionOutputRoot);
            return string.Empty;
        }

        // 根据时间戳升序，跟字符串序是一样
        Array.Sort(previousPaths);

        return previousPaths[previousPaths.Length-1];
    }

    /// <summary>
    /// 生成热更记录文件
    /// </summary>
    public void RecordHotFixInfos(string dir)
    {
        List<string> fileList = new List<string>();

        RecordAssetbundleInfos(fileList);

        RecordOtherFileInfos(fileList);

        string fileListPath = dir + "/" + HotFixFileRecordName;
        if (File.Exists(fileListPath))
        {
            File.Delete(fileListPath);
        }
        File.WriteAllText(fileListPath, string.Join("\n", fileList.ToArray()));
    }

    /// <summary>
    /// 记录非assetbundle相关的md5
    /// 目前这样文件都在根目录:包括banks和rawFileRecords.txt、还有总的manifest
    /// </summary>
    /// <param name="fileList"></param>
    private static void RecordOtherFileInfos(List<string> fileList)
    {
        string[] filePaths = Directory.GetFiles(VersionConst.StreamingVersionPath);
        string prefix = VersionConst.StreamingVersionPath + "/";
        foreach (var oneFilePath in filePaths)
        {
            // ignore .meta and .manifest
            if (oneFilePath.EndsWith(".meta") || oneFilePath.EndsWith(".manifest"))
            {
                continue;
            }

            // 只读的话md5file会被异常
            FileUtils.SetAttrAsNormal(oneFilePath);
            string md5 = GameUtil.md5file(oneFilePath);
            string relativeFilePath = oneFilePath.Replace("\\", "/").Replace(prefix, "");
            fileList.Add(string.Format("{0}|{1}", relativeFilePath, md5));
        }
    }

    /// <summary>
    /// 记录assetbundle的信息
    /// 为了加速，所以直接用assetbundle的hash. 只要meta文件不变，生成的assetbundle不会变
    /// 这里还要注意,lua是拷贝到temp目录，所以temp目录也不能删除。不然lua的assetbundle都会不一样
    /// </summary>
    /// <param name="fileList"></param>
    private void RecordAssetbundleInfos(List<string> fileList)
    {
        var assetbundle = AssetBundle.LoadFromFile(VersionConst.StreamingVersionPath + "/" + PlatformEditorUtils.GetPlatformDesc(_buildTarget));

        if (assetbundle != null)
        {
            var assetBundleManifest = assetbundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            if (assetBundleManifest != null)
            {
                var allAssetBundles = assetBundleManifest.GetAllAssetBundles();
                Debug.Log("allAssetBundles:" + allAssetBundles.Length);
                foreach (var oneAssetBundle in allAssetBundles)
                {
                    var hash = assetBundleManifest.GetAssetBundleHash(oneAssetBundle);
                    fileList.Add(string.Format("{0}|{1}", oneAssetBundle, hash.ToString()));
                }

                assetbundle.Unload(true);
            }
            else
            {
                Debug.LogError("LoadAsset AssetBundleManifest error:");
            }
        }
        else
        {
            Debug.LogError("cannt AssetBundle.LoadFromFile from :");
        }
    }
}
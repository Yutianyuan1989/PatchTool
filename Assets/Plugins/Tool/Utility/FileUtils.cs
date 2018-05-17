using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class FileUtils
{
    ///// <summary>
    ///// 在指定路径，下创建一个指定文件名的文件，把内容写进去
    ///// </summary>
    ///// <param name="dir"></param>
    ///// <param name="name"></param>
    ///// <param name="content"></param>
    //public static void WriteFile(string dir, string name, string content)
    //{
    //    WriteFile(dir + "/" + name, content);
    //}

    //public static void WriteFile(string path, string content)
    //{
    //    if (string.IsNullOrEmpty(content))
    //    {
    //        return;
    //    }

    //    FileInfo info = new FileInfo(path);
    //    if (info.Exists)
    //    {
    //        File.Delete(path);
    //    }
    //    StreamWriter sw = info.CreateText();
    //    sw.WriteLine(content);
    //    sw.Close();
    //    sw.Dispose();
    //}

    //public static void WriteFile(string dir, string name, byte[] content)
    //{
    //    if (content==null || content.Length==0)
    //    {
    //        return;
    //    }

    //    string localFile = dir + "/" + name;
    //    FileInfo info = new FileInfo(localFile);
    //    if (info.Exists)
    //    {
    //        File.Delete(localFile);
    //    }

    //    FileStream sw = File.Open(localFile, FileMode.OpenOrCreate);
    //    sw.Write(content, 0, content.Length);
    //    //FileStream sw = info.Create();
    //    //sw.Write(content,0,content.Length);
    //    sw.Close();
    //    sw.Dispose();
    //}

    /// <summary>
    /// 删除指定文件
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="removeMeta">删除对应的meta文件</param>
    public static void DelFile(string path, bool removeMeta = false)
    {
        DoDelFile(path);
        if (removeMeta)
        {
            DoDelFile(GetFileMetaPath(path));
        }
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="path"></param>
    private static void DoDelFile(string path)
    {
        if (File.Exists(path))
        {
            SetAttrAsNormal(path);

            File.Delete(path);
        }
    }

    /// <summary>
    /// 删除文件，并把meta文件也删除
    /// </summary>
    /// <param name="path">路径</param>
    public static void DelFileWithMeta(string path)
    {
        DelFile(path);
        DelFile(GetFileMetaPath(path));
    }

    /// <summary>
    /// 获得对应文件meta路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetFileMetaPath(string path)
    {
        return path + ".meta";
    }

    /// <summary>
    /// 设置文件属性为normal
    /// </summary>
    /// <param name="path"></param>
    public static void SetAttrAsNormal(string path)
    {
        if (File.GetAttributes(path) != FileAttributes.Normal)
        {
            File.SetAttributes(path, FileAttributes.Normal);
        }
    }

    /// <summary>
    /// 删除一个目录
    /// </summary>
    /// <param name="path"></param>
    /// <param name="removeMeta">是否也删除meta文件</param>
    public static void DelDir(string path, bool removeMeta=false)
    {
        if (Directory.Exists(path))
        {
            DelDir(new DirectoryInfo(path));

            if (removeMeta)
            {
                DelFile(GetFileMetaPath(path));
            }
        }
    }

    /// <summary>
    /// 删除一个目录
    /// </summary>
    /// <param name="dirInfo"></param>
    public static void DelDir(DirectoryInfo dirInfo)
    {
	    try
	    {
	        if (dirInfo==null || !dirInfo.Exists)
	        {
	            return;
	        }

		    foreach (DirectoryInfo directoryInfo in dirInfo.GetDirectories())
		    {
			    DelDir(directoryInfo);
		    }

		    foreach (FileInfo fileInfo in dirInfo.GetFiles())
		    {
			    fileInfo.Attributes = fileInfo.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
			    fileInfo.Delete();
		    }

	        dirInfo.Attributes = dirInfo.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
	        dirInfo.Delete();
		}
	    catch (Exception e)
	    {
	        if (dirInfo != null)
	        {
	            Debug.LogError("DelDir (" + dirInfo.Name + ") get error: " + e.Message);
            }
		    
	    }
    }
    
    /// <summary>
    /// 把路径过滤只保留文件名
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetFileName(string path)
    {
        int index = path.LastIndexOf("/", StringComparison.Ordinal);

        if (index > -1)
        {
            return path.Substring(index + 1);
        }
        else
        {
            return path;
        }
    }

    /// <summary>
    /// 获得文件后缀
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetFileExt(string name)
    {
        var index = name.LastIndexOf(".", StringComparison.Ordinal);

        return index > -1 ? name.Substring(index + 1) : name;
    }
    
    /// <summary>
    /// 清理目录的只读属性
    /// </summary>
    /// <param name="directoryPath"></param>
    public static void ClearDirectoryReadOnly(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            DirectoryInfo parentDirectory = new DirectoryInfo(directoryPath);
            ClearReadOnly(parentDirectory);
        }
    }

    /// <summary>
    /// 清理目录的只读属性
    /// </summary>
    /// <param name="parentDirectory"></param>
    public static void ClearReadOnly(DirectoryInfo parentDirectory)
    {
        if (parentDirectory != null && parentDirectory.Exists)
        {
            parentDirectory.Attributes = FileAttributes.Normal;
            foreach (FileInfo fi in parentDirectory.GetFiles())
            {
                fi.Attributes = FileAttributes.Normal;
            }
            foreach (DirectoryInfo di in parentDirectory.GetDirectories())
            {
                ClearReadOnly(di);
            }
        }
        else if(parentDirectory != null)
        {
            Debug.LogError("could not found dir path:" + parentDirectory.Name);
        }
    }

    /// <summary>
    /// 获得从assets开始的path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetPathFromAssets(string path)
    {
       string assetPath = Application.dataPath.Substring(0, path.LastIndexOf("/Assets", StringComparison.Ordinal));
        return path.Remove(0, assetPath.Length+1);
    }


    public static bool ClearDirectory(string fullPath)
    {
        try
        {
            string[] files = Directory.GetFiles(fullPath);
            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }
            string[] directories = Directory.GetDirectories(fullPath);
            for (int j = 0; j < directories.Length; j++)
            {
                Directory.Delete(directories[j], true);
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("ClearDirectory:" + e.Message);
            return false;
        }
    }

    public static bool ClearDirectory(string fullPath, string[] fileExtensionFilter, string[] folderFilter)
    {
        try
        {
            if (fileExtensionFilter != null)
            {
                string[] files = Directory.GetFiles(fullPath);
                for (int i = 0; i < files.Length; i++)
                {
                    if ((fileExtensionFilter.Length > 0))
                    {
                        for (int j = 0; j < fileExtensionFilter.Length; j++)
                        {
                            if (files[i].Contains(fileExtensionFilter[j]))
                            {
                                DeleteFile(files[i]);
                                break;
                            }
                        }
                    }
                }
            }
            if (folderFilter != null)
            {
                string[] directories = Directory.GetDirectories(fullPath);
                for (int k = 0; k < directories.Length; k++)
                {
                    if ((folderFilter.Length > 0))
                    {
                        for (int m = 0; m < folderFilter.Length; m++)
                        {
                            if (directories[k].Contains(folderFilter[m]))
                            {
                                DeleteDirectory(directories[k]);
                                break;
                            }
                        }
                    }
                }
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public static void CopyDirectory(string source, string dest, string searchPattern = "*.*", SearchOption option = SearchOption.AllDirectories)
    {
        string[] files = Directory.GetFiles(source, searchPattern, option);

        for (int i = 0; i < files.Length; i++)
        {
            string str = files[i].Remove(0, source.Length);
            string path = dest + "/" + str;
            string dir = Path.GetDirectoryName(path);
            if (dir != null) Directory.CreateDirectory(dir);
            File.Copy(files[i], path, true);
        }
    }
    public static string CombinePath(string path1, string path2)
    {
        if (path1.LastIndexOf('/') != (path1.Length - 1))
        {
            path1 = path1 + "/";
        }
        if (path2.IndexOf('/') == 0)
        {
            path2 = path2.Substring(1);
        }
        return (path1 + path2);
    }

    public static string CombinePaths(params string[] values)
    {
        if (values.Length <= 0)
        {
            return string.Empty;
        }
        if (values.Length == 1)
        {
            return CombinePath(values[0], string.Empty);
        }
        if (values.Length <= 1)
        {
            return string.Empty;
        }
        string str = CombinePath(values[0], values[1]);
        for (int i = 2; i < values.Length; i++)
        {
            str = CombinePath(str, values[i]);
        }
        return str;
    }

    public static void CopyFile(string srcFile, string dstFile)
    {
        File.Copy(srcFile, dstFile, true);
    }

    public static bool CreateDirectory(string directory)
    {
        if (IsDirectoryExist(directory))
        {
            return true;
        }
        int num = 0;
        while (true)
        {
            try
            {
                Directory.CreateDirectory(directory);
                return true;
            }
            catch (Exception)
            {
                num++;
                if (num >= 3)
                {
                    return false;
                }
            }
        }
    }

    public static bool DeleteDirectory(string directory)
    {
        if (!IsDirectoryExist(directory))
        {
            return true;
        }
        int num = 0;
        while (true)
        {
            try
            {
                Directory.Delete(directory, true);
                return true;
            }
            catch (Exception)
            {
                num++;
                if (num >= 3)
                {
                    return false;
                }
            }
        }
    }

    public static bool DeleteFile(string filePath)
    {
        if (!IsFileExist(filePath))
        {
            return true;
        }
        int num = 0;
        while (true)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {
                num++;
                if (num >= 3)
                {
                    return false;
                }
            }
        }
    }

    public static string EraseExtension(string fullName)
    {
        if (fullName == null)
        {
            return null;
        }
        int length = fullName.LastIndexOf('.');
        if (length > 0)
        {
            return fullName.Substring(0, length);
        }
        return fullName;
    }


    public static string GetExtension(string fullName)
    {
        int startIndex = fullName.LastIndexOf('.');
        if ((startIndex > 0) && ((startIndex + 1) < fullName.Length))
        {
            return fullName.Substring(startIndex);
        }
        return string.Empty;
    }

    public static int GetFileLength(string filePath)
    {
        if (!IsFileExist(filePath))
        {
            return 0;
        }
        int num = 0;
        while (true)
        {
            try
            {
                FileInfo info = new FileInfo(filePath);
                return (int)info.Length;
            }
            catch (Exception exception)
            {
                num++;
                if (num >= 3)
                {
                    Debug.Log("Get FileLength of " + filePath + " Error! Exception = " + exception);
                    return 0;
                }
            }
        }
    }


    public static string GetFullName(string fullPath)
    {
        if (fullPath == null)
        {
            return null;
        }
        int num = fullPath.LastIndexOf("/", StringComparison.Ordinal);
        if (num > 0)
        {
            return fullPath.Substring(num + 1, (fullPath.Length - num) - 1);
        }
        return fullPath;
    }

    public static string GetFullPathInResources(string fileFullPath)
    {
        fileFullPath = fileFullPath.Replace(@"\", "/");
        string str = "Assets/Resources/";
        int index = fileFullPath.IndexOf(str, StringComparison.Ordinal);
        if (index >= 0)
        {
            return fileFullPath.Substring(index + str.Length);
        }
        return string.Empty;
    }

    public static bool IsDirectoryExist(string directory)
    {
        return Directory.Exists(directory);
    }


    public static bool ExistExtension(string path, string extention)
    {
        return string.Compare(Path.GetExtension(path), extention, StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static bool IsFileExist(string filePath)
    {
        return File.Exists(filePath);
    }
        

    public static byte[] ReadFile(string filePath)
    {
        if (IsFileExist(filePath))
        {
            byte[] buffer;
            int num = 0;
            do
            {
                try
                {
                    buffer = File.ReadAllBytes(filePath);
                }
                catch (Exception)
                {
                    buffer = null;
                }
                if ((buffer != null) && (buffer.Length > 0))
                {
                    return buffer;
                }
                num++;
            }
            while (num < 3);
        }
        return null;
    }

    public static string RelativeToAbsolutePath(string relativePath)
    {
        List<string> list = new List<string>();
        relativePath = relativePath.Replace('\\', '/');
        char[] separator = new char[] { '/' };
        foreach (string str in relativePath.Split(separator))
        {
            switch (str)
            {
                case ".":
                    break;

                case "..":
                    if (list.Count > 0)
                    {
                        list.RemoveAt(list.Count - 1);
                    }
                    else
                    {
                        list.Add(str);
                    }
                    break;

                default:
                    list.Add(str);
                    break;
            }
        }
        return string.Join("/", list.ToArray());
    }

    public static bool WriteFile(string filePath, string content)
    {
        int num = 0;
        while (true)
        {
            try
            {
                File.WriteAllText(filePath, content);
                return true;
            }
            catch (Exception)
            {
                num++;
                if (num >= 3)
                {
                    DeleteFile(filePath);
                    return false;
                }
            }
        }
    }

    public static bool WriteFile(string filePath, byte[] data)
    {
        int num = 0;
        while (true)
        {
            try
            {
                File.WriteAllBytes(filePath, data);
                return true;
            }
            catch (Exception)
            {
                num++;
                if (num >= 3)
                {
                    DeleteFile(filePath);
                    return false;
                }
            }
        }
    }

    public static bool WriteFile(string filePath, byte[] data, int offset, int length)
    {
        FileStream stream = null;
        int num = 0;
        while (true)
        {
            try
            {
                stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                stream.Write(data, offset, length);
                stream.Close();
                return true;
            }
            catch (Exception)
            {
                if (stream != null)
                {
                    stream.Close();
                }
                num++;
                if (num >= 3)
                {
                    DeleteFile(filePath);
                    return false;
                }
            }
        }
    }

    public static string RemoveLastSeparator(string fullPath)
    {
        if (fullPath == null)
        {
            return null;
        }
        if (fullPath.Length>0 && fullPath[fullPath.Length -1] == '/')
        {
            return fullPath.Substring(0, fullPath.Length -1);
        }
        return fullPath;
    }

    /// <summary>
    /// 找到该目录下对应的带后缀的文件名
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="pathWithoutExtention"></param>
    /// <returns></returns>
    public static string[] GetFilePathWithExtention(string dir, string pathWithoutExtention)
    {
        var fileNames = Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly)
                        .Select(s=>s.Replace("\\", "/"));

        return fileNames.Where(s => (EraseExtension(s) == pathWithoutExtention) && Path.GetExtension(s) != ".meta").ToArray();
    }


    /// <summary>
    /// 根据不带后缀的path, 获得当前目录下的path.
    /// 以下条件就报错:
    /// 1. 找不到
    /// 2. 有多个名字一样， 但后缀不一样
    /// </summary>
    /// <param name="pathWithoutExtention">不带后缀的路径</param>
    /// <param name="tip">用于提示</param>
    /// <param name="resultPath">输出的结果路径</param>
    /// <returns></returns>
    public static bool ConvertToPathWithExtention(string pathWithoutExtention, string tip, out string resultPath)
    {
        resultPath = string.Empty;
        // 获得该目录
        var realAssetPaths = GetFilePathWithExtention(Path.GetDirectoryName(pathWithoutExtention), pathWithoutExtention);

        if (realAssetPaths == null || realAssetPaths.Length == 0)
        {
            Debug.LogError(string.Format("could not found asset: ({0}) in {1}", pathWithoutExtention, tip));
            return false;
        }
        else if (realAssetPaths.Length > 1)
        {
            Debug.LogError(string.Format("asset: ({0}) found conflict in {1}. may file names same but extention diffent", pathWithoutExtention, tip));
            return false;
        }

        resultPath = realAssetPaths[0];
        return true;
    }

    public static string MakePathRelativeToProject(string path)
    {
        string fullPath = Path.GetFullPath(path);
        string fullProjectPath = Path.GetFullPath(Environment.CurrentDirectory + Path.DirectorySeparatorChar);
        return fullPath.Replace(fullProjectPath, "");
    }
}

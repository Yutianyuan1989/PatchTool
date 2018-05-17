using UnityEngine;
using System.Collections;
using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Zip压缩与解压缩 
/// </summary>
public class ZipHelper
{
    /// <summary>
    /// 压缩单个文件
    /// </summary>
    /// <param name="fileToZip">要压缩的文件</param>
    /// <param name="zipedFile">压缩后的文件</param>
    /// <param name="compressionLevel">压缩等级</param>
    /// <param name="blockSize">每次写入大小</param>
    public static void ZipFile(string fileToZip, string zipedFile, int compressionLevel, int blockSize)
    {
        //如果文件没有找到，则报错
        if (!System.IO.File.Exists(fileToZip))
        {
            throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
        }

        using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
        {
            using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
            {
                using (System.IO.FileStream StreamToZip = new System.IO.FileStream(fileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);

                    ZipEntry ZipEntry = new ZipEntry(fileName);

                    ZipStream.PutNextEntry(ZipEntry);

                    ZipStream.SetLevel(compressionLevel);

                    byte[] buffer = new byte[blockSize];

                    int sizeRead = 0;

                    try
                    {
                        do
                        {
                            sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                            ZipStream.Write(buffer, 0, sizeRead);
                        }
                        while (sizeRead > 0);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }

                    StreamToZip.Close();
                }

                ZipStream.Finish();
                ZipStream.Close();
            }

            ZipFile.Close();
        }
    }

    /// <summary>
    /// 压缩单个文件
    /// </summary>
    /// <param name="fileToZip">要进行压缩的文件名</param>
    /// <param name="zipedFile">压缩后生成的压缩文件名</param>
    public static void ZipFile(string fileToZip, string zipedFile)
    {
        //如果文件没有找到，则报错
        if (!File.Exists(fileToZip))
        {
            throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
        }

        using (FileStream fs = File.OpenRead(fileToZip))
        {
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            using (FileStream ZipFile = File.Create(zipedFile))
            {
                using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                {
                    string fileName = fileToZip.Substring(fileToZip.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                    ZipEntry ZipEntry = new ZipEntry(fileName);
                    ZipStream.PutNextEntry(ZipEntry);
                    ZipStream.SetLevel(5);

                    ZipStream.Write(buffer, 0, buffer.Length);
                    ZipStream.Finish();
                    ZipStream.Close();
                }
            }
        }
    }

    /// <summary>
    /// 压缩多层目录
    /// </summary>
    /// <param name="strDirectory">The directory.</param>
    /// <param name="zipedFile">The ziped file.</param>
    public static void ZipFileDirectory(string strDirectory, string zipedFile)
    {
        using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
        {
            using (ZipOutputStream s = new ZipOutputStream(ZipFile))
            {
                ZipSetp(strDirectory, s, "");
            }
        }
    }

    /// <summary>
    /// 递归遍历目录
    /// </summary>
    /// <param name="strDirectory">The directory.</param>
    /// <param name="s">The ZipOutputStream Object.</param>
    /// <param name="parentPath">The parent path.</param>
    private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
    {
        if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
        {
            strDirectory += Path.DirectorySeparatorChar;
        }
        //Crc32 crc = new Crc32();

        string[] filenames = Directory.GetFileSystemEntries(strDirectory);

        foreach (string file in filenames)// 遍历所有的文件和目录
        {

            if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
            {
                string pPath = parentPath;
                pPath += file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                pPath += Path.DirectorySeparatorChar;
                ZipSetp(file, s, pPath);
            }

            else // 否则直接压缩文件
            {
                //打开压缩文件
                using (FileStream fs = File.OpenRead(file))
                {

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);

                    string fileName = parentPath + file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                    ZipEntry entry = new ZipEntry(fileName);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;

                    fs.Close();

                    //crc.Reset();
                    //crc.Update(buffer);

                    //entry.Crc = crc.Value;
                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);

                }
            }
        }
    }

    /// <summary>
    /// 解压缩一个 zip 文件。
    /// </summary>
    /// <param name="zipedFile">The ziped file.</param>
    /// <param name="strDirectory">The STR directory.</param>
    /// <param name="overWrite">是否覆盖已存在的文件。</param>
    public static void UnZip(string zipedFile, string strDirectory, bool overWrite)
    {
        Stream stream = File.OpenRead(zipedFile);
        UnZip(stream, strDirectory, overWrite);
    }

    public static void UnZip(byte[] bytes, string strDirectory, bool overWrite)
    {
        Stream stream = new MemoryStream(bytes);
        UnZip(stream, strDirectory, overWrite);
    }

    public static void UnZip(Stream stream, string strDirectory, bool overWrite)
    {
        if (strDirectory == "")
            strDirectory = Directory.GetCurrentDirectory();
        if (!strDirectory.EndsWith("/"))
            strDirectory = strDirectory + "/";

        using (ZipInputStream s = new ZipInputStream(stream))
        {
            //s.Password = password;
            ZipEntry theEntry;

            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = "";
                string pathToZip = "";
                pathToZip = theEntry.Name;
                pathToZip = pathToZip.Replace("\\", "/");

                if (pathToZip != "")
                    directoryName = Path.GetDirectoryName(pathToZip) + "/";

                string fileName = Path.GetFileName(pathToZip);
                Directory.CreateDirectory(strDirectory + directoryName);

                if (fileName != "")
                {
                    if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName)))
                    {
                        using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);

                                if (size > 0)
                                    streamWriter.Write(data, 0, size);
                                else
                                    break;
                            }
                            streamWriter.Close();
                        }
                    }
                }
            }
            s.Close();
        }
    }

    /// <summary>
    /// 获取文件的MD5码
    /// </summary>
    /// <param name="fileName">传入的文件名（含路径及后缀名）</param>
    /// <returns></returns>
    public static string GetMD5HashFromFile(byte[] buffer)
    {
        try
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(buffer);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
        }
    }

    static private SymmetricAlgorithm mCSP = new DESCryptoServiceProvider();  //声明对称算法变量
    private const string CIV = "Mi9l/+7Zujhy12se6Yjy111A";  //初始化向量
    private const string CKEY = "Longame1/9i="; //密钥（常量）

    /// <summary>
    /// 加密字符串
    /// </summary>
    /// <param name="Value">需加密的字符串</param>
    /// <returns></returns>
    public static string EncryptString(string Value)
    {
        ICryptoTransform ct; //定义基本的加密转换运算
        MemoryStream ms; //定义内存流
        CryptoStream cs; //定义将内存流链接到加密转换的流
        byte[] byt;
        //CreateEncryptor创建(对称数据)加密对象
        ct = mCSP.CreateEncryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV)); //用指定的密钥和初始化向量创建对称数据加密标准
        byt = Encoding.UTF8.GetBytes(Value); //将Value字符转换为UTF-8编码的字节序列
        ms = new MemoryStream(); //创建内存流
        cs = new CryptoStream(ms, ct, CryptoStreamMode.Write); //将内存流链接到加密转换的流
        cs.Write(byt, 0, byt.Length); //写入内存流
        cs.FlushFinalBlock(); //将缓冲区中的数据写入内存流，并清除缓冲区
        cs.Close(); //释放内存流
        return Convert.ToBase64String(ms.ToArray()); //将内存流转写入字节数组并转换为string字符
    }

    /// <summary>
    /// 解密字符串
    /// </summary>
    /// <param name="Value">要解密的字符串</param>
    /// <returns>string</returns>
    public static string DecryptString(string Value)
    {
        ICryptoTransform ct; //定义基本的加密转换运算
        MemoryStream ms; //定义内存流
        CryptoStream cs; //定义将数据流链接到加密转换的流
        byte[] byt;
        ct = mCSP.CreateDecryptor(Convert.FromBase64String(CKEY), Convert.FromBase64String(CIV)); //用指定的密钥和初始化向量创建对称数据解密标准
        byt = Convert.FromBase64String(Value); //将Value(Base 64)字符转换成字节数组
        ms = new MemoryStream();
        cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
        cs.Write(byt, 0, byt.Length);
        cs.FlushFinalBlock();
        cs.Close();
        return Encoding.UTF8.GetString(ms.ToArray()); //将字节数组中的所有字符解码为一个字符串
    }
}
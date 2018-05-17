using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VersionState
{
    /// <summary>
    /// 正常的开服
    /// </summary>
    Normal,

    /// <summary>
    /// 维护
    /// </summary>
    Maintain,

    /// <summary>
    /// 需要强更, 一般来说有新的版本的话，先前版本都应该设置为这个
    /// </summary>
    ForceUpdate,
}

[System.Serializable]
public class PatchInfo
{
    /// <summary>
    /// 补丁的zip包名， 最好以数字递增的方式来命名。
    /// 第一个补丁号为1，不要用0
    /// 这样比较清晰. 比如 1.zip
    /// </summary>
    public string PatchName;

    /// <summary>
    /// 该补丁包的md5码
    /// </summary>
    public string Md5;

    /// <summary>
    /// 文件大小
    /// </summary>
    public ulong FileSize;

    /// <summary>
    /// 这个补丁由哪几个补丁组成的 [MinRevisionNum, MaxRevisionNum]. 对应的文件名为 MaxRevisionNum-MinRevisionNum.zip
    /// 假设每3个补丁合一个包的话,补丁可能是这样的
    /// 4-1.zip
    /// 4.zip
    /// 3.zip
    /// 2.zip
    /// 1.zip
    /// </summary>
    //public int MaxRevisionNum;
    //public int MinRevisionNum;

    public ushort RevisionNum;
}

/// <summary>
/// 版本更新条目: 一个版本更新记录
/// </summary>
[System.Serializable]
public class VersionUpdateItem
{
    /// <summary>
    /// 版本号
    /// </summary>
    public AppVersion Version;

    /// <summary>
    /// 重定向的cdn地址
    /// </summary>
    //public string RedirectedCdnUrl;

    /// <summary>
    /// 可用于android的安装包下载或iphone的安装包跳转
    /// </summary>
    //public string Url;

    /// <summary>
    /// 版本状态
    /// </summary>
    //public VersionState State;

    /// <summary>
    /// 根据不同的State有不同的意义
    /// Maintain : 停服公告, 和MaintainEndTime配合起来用. 
    ///     MaintainEndTime 小于等于 0时，单独提示这个
    ///     MaintainEndTime 小于等于当前的时间时, 就说明停服结束了, 不再显示公告
    ///     MaintainEndTime 大于当前的时间时, 配合Announcement显示预计还要信服多久
    /// ForceUpdate :   强更提示, 接受最新的版本参数. 
    ///                 如果ForceUpdate为空给一个缺省的强更提示， 
    ///                 如果传入强更版本参数为空，就做一更新到最新版本的提示
    /// </summary>
    //public string Announcement;

    /// <summary>
    /// 停服结束时间 
    /// </summary>
    //public string MaintainEndTime;

    // todo 后面把它单独分离到一个文件内，这样版本文件大小会小一些
    /// <summary>
    /// 系统补丁
    /// </summary>
    public List<PatchInfo> Patches;
}
/// <summary>
/// 如果没有版本列表：
///     则认为自己的版本是最新。
/// 
/// 查找自己对应的版本信息
///     如果有:
///         当State为ForceUpdate时,         要做强更处理, 查找要强更的版本号(可能配置的问题有可能找不到)。 传给Announcement来做处理
///         当State为其他状态时，           做别的正常处理
///     如果没有: (由于没有配置该版本信息，如果有比自己大的就认为自己要强更)
///         查找最新版本版本号
///             没有找到:                   就认为自己是最新版本。 理论不应该出现， 这样就没有版本列表了
///             有找到:                     比自己小，则认为自己是最新版本不做强更处理。 比自己大，就做强更处理            
/// </summary>
[System.Serializable]
public class VersionUpdateInfo
{
    /// <summary>
    /// 用于验证
    /// </summary>
    //public string Md5;

    /// <summary>
    /// 各版本补丁信息. 
    /// 这里要求最新的版本放在前面。
    /// 程序不做版本排序，
    /// </summary>
    public List<VersionUpdateItem> VersionUpdateItems;
}

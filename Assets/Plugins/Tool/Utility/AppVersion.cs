using UnityEngine;

[System.Serializable]
public struct AppVersion
{
    public static AppVersion InitVersion = new AppVersion();

    public AppVersion(ushort majorVersion, ushort minorVersion, ushort revision)
    {
        // 这里都约束它为[0,99],  如果有变更的话，同时也改动GetVersionCode
        Debug.Assert(majorVersion<100 && minorVersion<100, "constraint: (majorVersion<100 && minorVersion<100)");
        MajorVersion = majorVersion;
        MinorVersion = minorVersion;
        Revision = revision;
    }

    public AppVersion(string versionString)
    {
        var versionVals = versionString.Split('.');
        int majorVersion = 0;
        int minorVersion = 0;
        int revision = 0;
        if (versionVals.Length >= 1)
        {
            int.TryParse(versionVals[0], out majorVersion);
        }

        if (versionVals.Length >= 2)
        {
            int.TryParse(versionVals[1], out minorVersion);
        }

        if (versionVals.Length >= 3)
        {
            int.TryParse(versionVals[2], out revision);
        }

        if (CheckConstraint(majorVersion, minorVersion))
        {
            MajorVersion = (ushort)majorVersion;
            MinorVersion = (ushort)minorVersion;
            Revision = (ushort)revision;
        }
        else
        {
            MajorVersion = 0;
            MinorVersion = 0;
            Revision = 0;
            Debug.LogError(string.Format("majorVersion:{0} minorVersion:{1} cannt pass CheckConstraint", majorVersion, minorVersion));
        }
    }

    /// <summary>
    /// 大版本更新时+1，重置下面字段为0
    /// </summary>
    public ushort MajorVersion;

    /// <summary>
    /// 平常的版本更新时+1, 重置下面字段为0
    /// </summary>
    public ushort MinorVersion;

    /// <summary>
    /// 修订版本号即补丁号， 缺省是0, 依次累加
    /// </summary>
    public ushort Revision;


    public string GetVersionString()
    {
        return string.Format("{0}.{1}.{2}", MajorVersion, MinorVersion, Revision);
    }

    public override string ToString()
    {
        return GetVersionString();
    }
    public override bool Equals(object obj)
    {
        if (!(obj is AppVersion))
        {
            return false;
        }

        return this.Equals((AppVersion)obj);
    }

    public bool Equals(AppVersion otherVersion)
    {
        return this.MajorVersion == otherVersion.MajorVersion
               && this.MinorVersion == otherVersion.MinorVersion
               && this.Revision == otherVersion.Revision;
    }

    /// <summary>
    /// 是否同一个版本， 不比较补丁
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public bool IsSameVersionSame(AppVersion b)
    {
        return MajorVersion == b.MajorVersion && MinorVersion == b.MinorVersion;
    }

    //public override int GetHashCode()
    //{
    //    return GetVersionCode() + 10000* Revision;
    //}


    /// <summary>
    /// 直接用版本号做versionCode
    /// </summary>
    /// <returns></returns>
    public int GetVersionCode()
    {
        return MajorVersion * 100 + MinorVersion;
    }

    public static bool operator ==(AppVersion a, AppVersion b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(AppVersion a, AppVersion b)
    {
        return !(a == b);
    }

    public static bool operator <(AppVersion a, AppVersion b)
    {
        return !(a > b || a == b);
    }
    public static bool operator >(AppVersion a, AppVersion b)
    {
        return a.MajorVersion > b.MajorVersion
            || a.MajorVersion == b.MajorVersion && a.MinorVersion > b.MinorVersion
            || a.MajorVersion == b.MajorVersion && a.MinorVersion == b.MinorVersion && a.Revision > b.Revision;
    }

    public static bool operator >=(AppVersion a, AppVersion b)
    {
        return a == b || a > b;
    }

    public static bool operator <=(AppVersion a, AppVersion b)
    {
        return a == b || a < b;
    }

    /// <summary>
    /// 验证约束
    /// </summary>
    /// <returns></returns>
    public static bool CheckConstraint(int majorVersion, int minorVersion)
    {
        return (majorVersion < 100) && (minorVersion < 100);
    }

    public bool CheckConstraint()
    {
        return CheckConstraint(MajorVersion, MinorVersion);
    }
}

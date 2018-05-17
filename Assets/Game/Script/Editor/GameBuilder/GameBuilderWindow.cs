using System;
using UnityEditor;

using UnityEngine;

public class GameBuilderWindow : EditorWindow
{
    /// <summary>
    /// 目标类型，Win，IOS，Android
    /// </summary>
    private BuildTarget _buildTarget;

    private bool _increativeBuildAssetBundles = true;
    private bool _bundleCompress = true;

    private bool _useMono2X = true;

	private bool _exportProject;

    private string _appName = "WGame";

	private bool _useGradle;

    private string _bundleIdentifier = "com.lilith.wgame";

    private bool _development;

    /// <summary>
    /// 是否启用热更
    /// </summary>
    private HotFixOption _hotFixOption = HotFixOption.None;
    private string _hotFixUrl = VersionConst.HotFixCdn;

    // 版本号
    private VersionOption _versionOption = VersionOption.CustomizeVersion;
    private AppVersion _customizeVersion;

    private BuildMode _buildMode = BuildMode.Package;
    private bool _applyAllRule = true;

    public GameBuilderWindow()
	{
		_exportProject = false;
	    _development = true;
	    _useGradle = true;
	}

	[MenuItem("WGame/Build/出包", false, 212)]
	public static void CreateGameBuilder()
	{
		GetWindow(typeof(GameBuilderWindow)).Show();
	}

    private void OnEnable()
    {
        _buildTarget = EditorUserBuildSettings.activeBuildTarget;

    }

    private void OnGUI()
    {
        GUILayout.Label("出包工具:", EditorStyles.boldLabel);

        _buildMode = (BuildMode)EditorGUILayout.EnumPopup("出包方式", _buildMode);

        GUILayout.Space(10);
        // -选择目标平台类型-//
        _buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("选择平台", _buildTarget);
        _versionOption = (VersionOption)EditorGUILayout.EnumPopup("版本号模式", _versionOption);
        EditorGUI.indentLevel++;
        if (_versionOption == VersionOption.CustomizeVersion)
        {
            _customizeVersion = new AppVersion(EditorGUILayout.TextField("3位(前两位0~99之间):", _customizeVersion.ToString()));
        }
        else
        {
            EditorGUILayout.LabelField("新的版本号:", GameBuilder.GetBuildVersion(_versionOption, _customizeVersion, _buildTarget).ToString());
        }
        EditorGUI.indentLevel--;

        _applyAllRule = EditorGUILayout.Toggle("强制应用所有Rule", _applyAllRule);
        _increativeBuildAssetBundles = EditorGUILayout.Toggle("增量式构建Bundle", _increativeBuildAssetBundles);
        _bundleCompress = EditorGUILayout.Toggle("开启Bundle压缩", _bundleCompress);


        if (_buildMode != BuildMode.Patch)
        {
            GUILayout.Space(10);
            _hotFixOption = (HotFixOption)EditorGUILayout.EnumPopup("热更url", _hotFixOption);
            EditorGUI.indentLevel++;
            if (_hotFixOption == HotFixOption.Customize)
            {
                _hotFixUrl = EditorGUILayout.TextField("热更Url:", _hotFixUrl);
            }
            EditorGUI.indentLevel--;

            _development = EditorGUILayout.Toggle("开发版本:", _development);
            if (_buildTarget != BuildTarget.iOS)
            {
                _useMono2X = EditorGUILayout.Toggle("是否使用mono打包", _useMono2X);
            }
            
            if (_buildTarget == BuildTarget.Android)
            {
                _exportProject = EditorGUILayout.Toggle("是否导出工程", _exportProject);
                if (_exportProject)
                {
                    _useGradle = EditorGUILayout.Toggle("是否使用gradle", _useGradle);
                }
            }

            _bundleIdentifier = EditorGUILayout.TextField("Bundle Identifier", _bundleIdentifier);

            _appName = EditorGUILayout.TextField("app名字", _appName);
        }

        if (GUILayout.Button("构建"))
        {
            BuildGame();
        }
    }


    private void BuildGame()
    {
        if (_hotFixOption == HotFixOption.Customize && string.IsNullOrEmpty(_hotFixUrl))
        {
            Debug.LogError("hotFixUrl is empty");
            return;
        }

        var buildPath = string.Empty;
        if (_buildMode != BuildMode.Patch)
        {
            buildPath = EditorUtility.SaveFilePanel(
                "SaveFile",
                PlayerPrefs.GetString("build_play_save_path"),
                _appName,
                _exportProject ? string.Empty : PlatformEditorUtils.GetAppExt(_buildTarget).Substring(1));

            if (string.IsNullOrEmpty(buildPath))
            {
                Debug.LogError("buildPath is empty");
                return;
            }

            PlayerPrefs.SetString("build_play_save_path", buildPath.Replace(_appName, string.Empty));
        }

        var buildOptions = BuildOptions.None;
        if (_development)
        {
            buildOptions |= BuildOptions.Development;
        }

	    if (_exportProject)
	    {
		    buildOptions |= BuildOptions.AcceptExternalModificationsToPlayer;
	    }

        GameBuilder.GameBuilderParameter para =
            new GameBuilder.GameBuilderParameter
                {
                    BuildMode = _buildMode,
                    BuildTarget = _buildTarget,
                    CopyFmod = true,
                    IncreativeBuildAssetBundles = _increativeBuildAssetBundles,
                    BundleCompress = _bundleCompress,
                    ApplyAllRule = _applyAllRule,
                    BuildVersion =  GameBuilder.GetBuildVersion(_versionOption, _customizeVersion, _buildTarget),
                    BuildOptions = buildOptions,
                    OutputPath = buildPath,
                    UseMono2X = _useMono2X,
                    ExportProject = _exportProject,
                    UseGradle = _useGradle,
                    BundleIdentifier = _bundleIdentifier,
                    Multithreaded = true,
                    EnableHotFix = _hotFixOption != HotFixOption.None,
                    HotFixUrl = GameBuilder.GetHotFixUrl(_hotFixOption, _hotFixUrl),
                    BuildNumber = DateTimeToUnixTimestamp(DateTime.Now),
            };

        GameBuilder.BuildGame(para);
    }


    public static long DateTimeToUnixTimestamp(DateTime dateTime)
    {
        var start = new DateTime(1970,1,1,0,0,0, dateTime.Kind);
        return Convert.ToInt64((dateTime - start).TotalSeconds);
    }
}

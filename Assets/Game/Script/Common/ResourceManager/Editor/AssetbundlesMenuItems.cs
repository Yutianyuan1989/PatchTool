using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AssetBundles
{
    public class AssetBundlesMenuItems
    {
        const string kSimulationMode = "Tools/AssetBundle/Simulation Mode";

        [MenuItem(kSimulationMode)]
        public static void ToggleSimulationMode()
        {
           // AssetBundleManager.SimulateAssetBundleInEditor = !AssetBundleManager.SimulateAssetBundleInEditor;
        }

        [MenuItem(kSimulationMode, true)]
        public static bool ToggleSimulationModeValidate()
        {
            //Menu.SetChecked(kSimulationMode, AssetBundleManager.SimulateAssetBundleInEditor);
            return true;
        }

        [MenuItem("Tools/AssetBundle/Build AssetBundles")]
        static public void BuildAssetBundles()
        {
            BuildScript.BuildAssetBundles();
        }

        [MenuItem ("Tools/AssetBundle/Build Player (for use with engine code stripping)")]
        static public void BuildPlayer ()
        {
            BuildScript.BuildPlayer();
        }

        [MenuItem("Tools/AssetBundle/Build AssetBundles from Selection")]
        private static void BuildBundlesFromSelection()
        {
            // Get all selected *assets*
            var assets = Selection.objects.Where(o => !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o))).Select(AssetDatabase.GetAssetPath).ToArray();
            
            var assetBundleBuilds = BuildScript.GetAssetBundleBuilds(assets);

            if (assetBundleBuilds.Count > 0)
            {
                BuildScript.BuildAssetBundles(assetBundleBuilds.ToArray());
            }
            else
            {
                Debug.LogError("no assetbundle to build");
            }
        }

        
    }
}
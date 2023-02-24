using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using XWDataStructure;
using XWEditorResource;
using XWUtility;
namespace XWTool
{
    /// <summary>
    /// 一键构建
    /// </summary>
    [XWFilePath("BuildTool.asset",XWFilePathAttribute.PathType.InXWEditor)]
    [Tool("调试/打包")]
    public class BuildTool : ScriptableObjectSingleton<BuildTool>
    {
        [Button]
        public void Build()
        {
            try {
                // 同步Addressables
                AddressableAutoSyncSO.Instance.SyncAddressalbes();
                // Addressables打包
                AddressableAssetSettings.BuildPlayerContent();
                // todo:其他
                // Build
                BuildPlayerOptions options = new BuildPlayerOptions();
                options.options = BuildOptions.ShowBuiltPlayer;
                BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(BuildPlayerWindow.DefaultBuildMethods.GetBuildPlayerOptions(options));
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
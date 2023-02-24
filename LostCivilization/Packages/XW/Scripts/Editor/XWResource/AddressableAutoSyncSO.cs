#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using XWDataStructure;
using XWTool;
using XWUtility;

namespace XWEditorResource
{
    /// <summary>
    /// Addressables资源同步目录
    /// </summary>
    [Tool("资源管理")]
    [CreateAssetMenu]
    [XWFilePath("AddressableAutoSync.asset",XWFilePathAttribute.PathType.InXWEditor)]
    public class AddressableAutoSyncSO : ScriptableObjectSingleton<AddressableAutoSyncSO>
    {
        [LabelText("目录路径")]
        [FolderPath]
        public string[] dirs;
        [LabelText("使用简单路径")]
        public bool simpleName;

        [Button(ButtonSizes.Medium,Name = "Addressables 同步")]
        public void SyncAddressalbes()
        {
            AddressablesAutoSync.Excute(simpleName);
        }
    }
}
#endif
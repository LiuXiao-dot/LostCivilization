using Unity.Collections;
using UnityEngine;
using XWDataStructure;
using XWUtility;

namespace XWUI
{
    /// <summary>
    /// 窗口管理器配置
    /// </summary>
    [XWFilePath("WindowManagerConfigSO.asset",XWFilePathAttribute.PathType.InXW)]
    public class WindowManagerConfigSO : ScriptableObjectSingleton<WindowManagerConfigSO>
    {
        [Header("窗口层级(该层级的最小Order in Layer值)")]
        public SerializableDictionary<WindowLayer, int> layers = new SerializableDictionary<WindowLayer, int>()
        {
            { WindowLayer.MAIN, 0 },
            { WindowLayer.CHILD, 10 },
            { WindowLayer.PANEL, 30 },
            { WindowLayer.POPUP, 60 },
            { WindowLayer.LOADING, 90 },
        };

        [ReadOnly]
        public SerializableDictionary<string,WindowData> windowDatas;

        #region Utils

        public static int GetLayer(string key)
        {
            return (int)Instance.windowDatas[key].layer;
        }
        #endregion
    }
}
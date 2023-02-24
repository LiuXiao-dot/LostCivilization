using System;
using UnityEngine.AddressableAssets;
using XWDataStructure;

namespace XWUI
{
    /// <summary>
    /// 窗口的基础数据
    /// </summary>
    [Serializable]
    public class WindowData
    {
        public string name;
        
        /// <summary>
        /// 窗口背景
        /// </summary>
        public WindowBg bg;

        /// <summary>
        /// 窗口层级
        /// </summary>
        public WindowLayer layer;

        /// <summary>
        /// 窗口顶部栏
        /// </summary>
        public WindowHeader header;

        /// <summary>
        /// 窗口prefab的Addressables引用
        /// </summary>
        public AssetReferenceGameObject reference;

        /// <summary>
        /// ctl脚本实际类型
        /// </summary>
        public SerializableType ctlType;

        public AWindowCtl GetCtl()
        {
            return (AWindowCtl)Activator.CreateInstance(ctlType);
        }
    }
}
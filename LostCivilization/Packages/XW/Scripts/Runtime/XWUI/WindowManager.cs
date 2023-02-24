using System;
using System.Collections.Generic;
using UnityEngine;
using XWDataStructure;
using XWResource;
using XWUtility;

namespace XWUI
{
    /// <summary>
    /// 窗口管理器
    /// 每个窗口将拥有一个Canvas和CanvasGroup
    /// </summary>
    public class WindowManager : IManager, IPreloader
    {
        private static WindowManager instance = new WindowManager();

        public static WindowManager Instance => instance;

        private WindowViewManager _viewManager;
        public Stack<AWindowCtl>[] ctlStack;
        private WindowManagerConfigSO config;

        /// <summary>
        /// 打开窗口的Blcok数量
        /// </summary>
        private int openBlockNum;
        /// <summary>
        /// 关闭窗口的Block数量
        /// </summary>
        private int closeBlockNum;

        /// <summary>
        /// 初始化窗口管理器
        /// </summary>
        /// <param name="viewParent"></param>
        public static void InitWindowManager(Transform viewParent = null)
        {
            if (instance == null)
            {
                instance = viewParent == null ? new WindowManager() : new WindowManager(viewParent);
            }
            else
            {
                instance._viewManager = viewParent == null ? null : new WindowViewManager(viewParent);
            }
            
        }
        
        /// <summary>
        /// 创建时自动创建出全部GameObject
        /// </summary>
        private WindowManager(Transform viewParent)
        {
            _viewManager = new WindowViewManager(viewParent);
        }

        /// <summary>
        /// 忽略UI
        /// </summary>
        public WindowManager()
        {
        }
        
        /// <summary>
        /// 初始化窗口管理器(该方法没有添加UI逻辑)
        /// </summary>
        public void Init()
        {
            var length = Enum.GetNames(typeof(WindowLayer)).Length;
            ctlStack = new Stack<AWindowCtl>[length];
            for (int i = 0; i < length; i++)
            {
                ctlStack[i] = new Stack<AWindowCtl>();
            }
        }
        
        /// <summary>
        /// 由于可能会需要加载加载窗口的配置，该文件需要提前加载
        /// </summary>
        /// <param name="onProgress"></param>
        public void Load(Action<float> onProgress)
        {
            AddressablesPoolManager.LoadScriptableObject<WindowManagerConfigSO>("WindowManagerConfigSO.asset",so =>
            {
                config = so;
                onProgress.Invoke(1);
            });
        }
        
        /// <summary>
        /// 添加一层Open窗口的block
        /// </summary>
        public static void AddOpenBlock()
        {
            instance.openBlockNum++;
        }
        
        public static void RemoveOpenBlock()
        {
            instance.openBlockNum--;
        }

        /// <summary>
        /// 添加一层Close窗口的block
        /// </summary>
        public static void AddCloseBlock()
        {
            instance.closeBlockNum++;
        }
        
        public static void RemoveCloseBlock()
        {
            instance.closeBlockNum--;
        }
        
        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <param name="windowId"></param>
        /// <returns>0:成功打开</returns>
        public static int OpenWindow(string windowKey,params object[] values)
        {
            if (instance.openBlockNum > 0) return WindowConstant.OpenBlocked;
            return instance.OpenWindowS(windowKey,values);
        }

        /// <summary>
        /// 清空layer层的全部窗口
        /// </summary>
        /// <param name="layer"></param>
        public void ClearWindows(int layer)
        {
            var count = ctlStack[layer].Count;
            for (int i = 0; i < count; i++)
            {
                CloseWindow(ctlStack[layer].Peek().data.name);
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <returns>0:成功关闭</returns>
        public static int CloseWindow(string windowKey)
        {
            if (instance.closeBlockNum > 0) return WindowConstant.CloseBlocked;
            return instance.CloseWindowS(windowKey);
        }

        private int OpenWindowS(string windowKey, object[] values)
        {
            #if UNITY_EDITOR
            if (!WindowManagerConfigSO.Instance.windowDatas.ContainsKey(windowKey))
            {
                XWLogger.Error($"窗口key错误:{windowKey}");
                return WindowConstant.UNEXIT_KEY;
            }
            #endif
            var layer = WindowManagerConfigSO.GetLayer(windowKey);
            if (layer <= (int)WindowLayer.CHILD)
            {
                // 高于layer的窗口会被关闭（Loading层自己控制开关不处理）
                for (int i = (int)WindowLayer.POPUP; i > layer; i--)
                {
                    ClearWindows(i);
                }
                // 上一级窗口的暂停判断(弹窗和面板不会使子窗口暂停)
                for (int i = layer; i >= 0; i--)
                {
                    if (ctlStack[i].Count > 0)
                    {
                        var pauseCtl = ctlStack[i].Peek();
                        UIMessageQueue.SendGameEventS(UIMessage.BeforeWindowPause,pauseCtl.data.name);
                        pauseCtl.Pause();
                        if(layer == i) ((Component)pauseCtl.GetView()).gameObject.SetActive(false);
                        UIMessageQueue.SendGameEventS(UIMessage.AfterWindowPause,pauseCtl.data.name);
                        break;
                    }
                }
            }

            if (ctlStack[layer].TryPeek(out var tempCtl) && tempCtl.data.name.Equals(windowKey)) {
                // 已开启的界面，再刷新一次
                tempCtl.InitModel(values);
                tempCtl.ReOpen();
                return 0;
            }
            
            var data = WindowManagerConfigSO.Instance.windowDatas[windowKey];
            var ctl = data.GetCtl();
            ctl.data = data;
            ctlStack[layer].Push(ctl);
            ctl.InitModel(values);

            if (_viewManager != null)
            {
                ctl.InitView(_viewManager.OpenView(windowKey));
                UIMessageQueue.SendGameEventS(UIMessage.AfterWindowLoaded, windowKey);
            }
            UIMessageQueue.SendGameEventS(UIMessage.BeforeWindowEnter, windowKey);
            ctl.Open();
            UIMessageQueue.SendGameEventS(UIMessage.AfterWindowEnter, windowKey);
            return 0;
        }

        private int CloseWindowS(string windowKey)
        {
            var layer = WindowManagerConfigSO.GetLayer(windowKey);
            if (ctlStack[layer].Peek().data.name != windowKey) return 0;
            var ctl = ctlStack[layer].Pop();
            
            UIMessageQueue.SendGameEventS(UIMessage.BeforeWindowExit,windowKey);
            ctl.Close();

            if (_viewManager != null)
            {
                _viewManager.CloseView(windowKey);
            }
            UIMessageQueue.SendGameEventS(UIMessage.AfterWindowExit,windowKey);

            if (layer <= (int)WindowLayer.CHILD)
            {
                // 上一级窗口的恢复判断(弹窗和面板不会使子窗口恢复)
                for (int i = 1; i >= 0; i--)
                {
                    if (ctlStack[i].Count > 0)
                    {
                        var pauseCtl = ctlStack[i].Peek();
                        UIMessageQueue.SendGameEventS(UIMessage.BeforeWindowResume,pauseCtl.data.name);
                        if(layer == i) ((Component)pauseCtl.GetView()).gameObject.SetActive(true);
                        pauseCtl.Resume();
                        UIMessageQueue.SendGameEventS(UIMessage.AfterWindowResume,pauseCtl.data.name);
                        break;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// 替换窗口
        /// </summary>
        /// <returns></returns>
        private int ReplaceWindowS(string fromWindow, string toWindow)
        {
            return 0;
        }

        /// <summary>
        /// 窗口UI控制,依赖viewParent创建UI
        /// </summary>
        private class WindowViewManager
        {
            private Transform[] windowLayers;
            private Stack<GameObject>[] windows;
            private string[] windowLayerNames;
            private WindowLayer lastLayer;

            public WindowViewManager(Transform viewParent)
            {
                windowLayerNames = Enum.GetNames(typeof(WindowLayer));
                var length = windowLayerNames.Length;
                var layers = Enum.GetValues(typeof(WindowLayer));
                lastLayer = (WindowLayer)layers.GetValue(length - 1);
                windowLayers = new Transform[length];
                windows = new Stack<GameObject>[length];
                // 注册加载回调，加载完成创建Prefab
                for (var i = 0; i < length; i++)
                {
                    windows[i] = new Stack<GameObject>();
                    var layerGo = new GameObject(windowLayerNames[i]);
                    var rectTransform = layerGo.AddComponent<RectTransform>();
                    rectTransform.SetParent(viewParent);
                    rectTransform.localScale = Vector3.one;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.offsetMin = Vector2.zero;
                    rectTransform.offsetMax = Vector2.zero;
                    windowLayers[i] = rectTransform;
                }
            }
            
            public IPrefabView OpenView(string key)
            {
                var res = WindowManagerConfigSO.Instance.windowDatas[key].reference;
                var layerIndex = WindowManagerConfigSO.GetLayer(key);
                var newWindow = AddressablesPoolManager.InstantiateGameObjectSync(res, windowLayers[layerIndex]);
                windows[layerIndex].Push(newWindow);
                RefreshSortingOrder();
                return newWindow.GetComponent<IPrefabView>();
            }

            public void CloseView(string key)
            {
                var layerIndex = WindowManagerConfigSO.GetLayer(key);
                var poped = windows[layerIndex].Pop();
                GameObject.Destroy(poped);
            }

            /// <summary>
            /// 刷新Canvas的sorting order
            /// </summary>
            private void RefreshSortingOrder()
            {
                var layer = WindowLayer.MAIN;
                var sortingOrder = WindowManagerConfigSO.Instance.layers[layer] + windows.Length - 1;
                foreach (var tempWindows in windows)
                {
                    foreach (var tempWindow in tempWindows)
                    {
                        tempWindow.GetComponent<Canvas>().sortingOrder = sortingOrder;
                        sortingOrder--;
                    }

                    if (layer == lastLayer) return;
                    layer++;
                    sortingOrder = WindowManagerConfigSO.Instance.layers[layer] + windows.Length - 1;
                }
            }
        }
    }
}
using System;
using XWResource;
using XWDataStructure;

namespace XWUI
{
    /// <summary>
    /// 窗口部分预加载逻辑：
    /// 配置SO+窗口Prefab
    /// </summary>
    public class WindowPreloader : IPreloader
    {
        public WindowManagerConfigSO config;

        public WindowPreloader()
        {
        }
        
        public void Load(Action<float> onProgress)
        {
            AddressablesPoolManager.LoadScriptableObject<WindowManagerConfigSO>("Generates/WindowManagerConfigSO.asset",so =>
            {
                config = so;
                onProgress.Invoke(1);
                /*onProgress.Invoke(0.1f);

                var windows = config.windowDatas;
                var total = windows.Count;
                var loaded = 0;
                foreach (var window in windows)
                {
                    AddressablesPoolManager.CacheGameObject(window.Value.reference, () =>
                    {
                        loaded++;
                        onProgress.Invoke(0.9f * loaded / total + 0.1f);
                    });
                }
                if(total == 0) onProgress.Invoke(1);*/
            });
        }
    }
}
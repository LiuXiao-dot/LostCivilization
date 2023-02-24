using System;
using System.Collections.Generic;
using System.Linq;
using XWDataStructure;

namespace XWResource
{
    /// <summary>
    /// 游戏中资源预加载,IOCComponent需要继承IPreloader
    /// </summary>
    public class GamePreloader
    {
        public static GamePreloader Instance = new GamePreloader();

        private Dictionary<string, Action> onLoadedDic;

        private Action<float> onProgress;
        private Action onAllLoaded;

        private Dictionary<string, float> progressDic;

        private Dictionary<string,IPreloader> preloaders;

        /// <summary>
        /// 父对象中创建IOCComponnet
        /// </summary>
        public GamePreloader()
        {
            preloaders = new Dictionary<string, IPreloader>();
            progressDic = new Dictionary<string, float>();
            onLoadedDic = new Dictionary<string, Action>();
        }

        /// <summary>
        /// 添加预加载器
        /// </summary>
        public void Add(string key,IPreloader preloader)
        {
            preloaders.Add(key,preloader);
        }

        public IPreloader Get(string key)
        {
            return preloaders[key];
        }
        
        /// <summary>
        /// 加载全部预加载器的内容
        /// </summary>
        public void Load()
        {
            var cache = preloaders;
            foreach (var temp in cache)
            {
                var key = temp.Key;
                progressDic.Add(key, 0);
                (temp.Value as IPreloader).Load(progress =>
                {
                    progressDic[key] = progress;
                    if (progress == 1)
                    {
                        if(onLoadedDic.ContainsKey(key))
                            onLoadedDic[key]?.Invoke();
                    }
                        
                    RefreshProgress();
                });
            }

            if (progressDic.Count == 0)
            {
                onAllLoaded?.Invoke();
            }
        }
        
        /// <summary>
        /// 刷新进度
        /// </summary>
        private void RefreshProgress()
        {
            var totalProgress = progressDic.Sum(temp => temp.Value) / progressDic.Count;
            onProgress?.Invoke(totalProgress);
            if (totalProgress == 1)
                onAllLoaded?.Invoke();
        }

        /// <summary>
        /// 注册回调，Preloader只执行一次，不提供UnRegister方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="onLoaded"></param>
        public void Register(string key, Action onLoaded)
        {
            if (!onLoadedDic.ContainsKey(key))
            {
                onLoadedDic.Add(key,delegate { });
            }
            
            onLoadedDic[key] += onLoaded;
        }

        /// <summary>
        /// 注册全部加载完毕的回调
        /// </summary>
        public void Register(Action onAllLoaded)
        {
            this.onAllLoaded += onAllLoaded;
        }

        public void RegisterProgress(Action<float> onProgress)
        {
            this.onProgress += onProgress;
        }
    }
}
using System;
using UnityEngine;
using XWInput;
using XWMessageQueue;
using XWResource;
using XWScene;
using XWUI;

namespace XWCore
{
    /// <summary>
    /// 游戏管理器(在启动场景中加载)
    /// 1.加载游戏中各个模块的管理器
    /// 2.加载预加载器
    /// 3.开始预加载
    /// </summary>
    public abstract class GameManager : MonoBehaviour, IGameEventListener<UIMessage>, IGameEventListener<SceneMessage>
    {
        /// <summary>
        /// 各个框架的数据初始化
        /// </summary>
        private void Awake()
        {
            Application.targetFrameRate = 60;
            InitManagers();
            InitPreloaders();
            InitScenes();
            // 初始化UI框架
            SceneMessageQueue.SubscribeS(this, SceneMessage.OnSceneInited);
            UIMessageQueue.Instance.Subscribe(this, UIMessage.AfterWindowEnter);
        }

        /// <summary>
        /// 初始化所有管理器(没有依赖项的管理器)
        /// </summary>
        protected virtual void InitManagers()
        {
            AddressablesPoolManager.Instance.Init();
            InputManager.Instance.Init();
            WindowManager.Instance.Init();
            GameModelManager.Instance.Init();
            XWSceneManager.Instance.Init();
        }

        protected virtual void InitScenes()
        {
            XWSceneManager.Instance.Load();
        }

        /// <summary>
        /// 已加载了存档，初始化所有数据
        /// </summary>
        protected virtual void InitModels()
        {
            
        }

        /// <summary>
        /// 初始化预加载器
        /// </summary>
        protected virtual void InitPreloaders()
        {
            GamePreloader.Instance.Add("AddressablesPoolManager", AddressablesPoolManager.Instance);
            GamePreloader.Instance.Add("InputManager", InputManager.Instance);
            //GamePreloader.Instance.Add("WindowManager", WindowManager.Instance); 窗口的文件已提前加载完成
            GamePreloader.Instance.Add("GameModelManager", GameModelManager.Instance);
        }

        /// <summary>
        /// 开始加载资源
        /// </summary>
        private void StartLoad()
        {
            GamePreloader.Instance.Register("GameModelManager",()=>
            {
                InitModels();
                GameModelManager.Instance.Save();
            });
            GamePreloader.Instance.Register(() =>
            {
                GamePreloader.Instance = null; // 加载完成，释放preloader
            });
            GamePreloader.Instance.Load();
        }
        

        public void OnMessage(GameEvent<UIMessage> inGameEvent)
        {
            switch (inGameEvent.operate)
            {
                case UIMessage.AfterWindowEnter:
                    UIMessageQueue.Instance.UnSubscribe(this, inGameEvent.operate);
                    SceneMessageQueue.SendGameEventImmediateS(SceneMessage.OnFirstWindowLoaded);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            UIMessageQueue.Instance.Callback();
        }

        public void OnMessage(GameEvent<SceneMessage> inGameEvent)
        {
            var windowManager = GameObject.FindWithTag("WindowManager");
            WindowManager.InitWindowManager(windowManager.transform);
            WindowManager.Instance.Load(progress =>
            {
                AddressablesPoolManager.StartLoadingRecord();
                Action onLoaded = () =>
                {
                    WindowManager.OpenWindow("MainWindow");
                };
                WindowManager.OpenWindow("LoadingWindow", onLoaded);
                StartLoad();
                AddressablesPoolManager.StopLoadingRecord();
                SceneMessageQueue.Instance.Callback();
            });
        }

        protected virtual void OnDestroy()
        {
            GameModelManager.Instance.Save();
        }
    }
}
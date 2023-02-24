using System;
using UnityEngine.SceneManagement;
using XWDataStructure;
using XWMessageQueue;
using XWResource;

namespace XWScene
{
    /// <summary>
    /// 场景管理器
    /// </summary>
    public class XWSceneManager : IManager, IGameEventListener<SceneMessage>
    {
        private static XWSceneManager instance = new XWSceneManager();
        public static XWSceneManager Instance => instance;

        private Scene launcherScene; 
        
        public void Init()
        {
            launcherScene = SceneManager.GetActiveScene();
            SceneMessageQueue.SubscribeS(this, SceneMessage.OnFirstWindowLoaded);
        }

        public void Load()
        {
            var progress = 0;
            Action onLoaded = () =>
            {
                progress +=1;
                if (progress == 2)
                {
                    SceneMessageQueue.SendGameEventS(SceneMessage.OnSceneInited);
                }
            };
            AddressablesPoolManager.OpenScene("UIScene.unity", onLoaded);
            AddressablesPoolManager.OpenScene("GameScene.unity", onLoaded);
        }

        public void OnMessage(GameEvent<SceneMessage> inGameEvent)
        {
            SceneMessageQueue.UnSubscribeS(this, SceneMessage.OnFirstWindowLoaded);
            SceneManager.UnloadSceneAsync(launcherScene);
            SceneMessageQueue.Instance.Callback();
        }
    }
}

// 自动生成的代码，请勿修改
using UnityEngine;
using XWMessageQueue;
namespace XWScene{
    public class SceneMessageQueue : AMainThreadMessageQueue<SceneMessage>
    {
        public static SceneMessageQueue Instance
        {
            get{
                if(_instance == null)
                {
                    var newObject = new GameObject("SceneMessageQueue");
                    GameObject.DontDestroyOnLoad(newObject);
                    _instance = newObject.AddComponent<SceneMessageQueue>();
                }
                return _instance;
            }
        }
        private static SceneMessageQueue _instance;

        public static bool SubscribeS(IGameEventListener<SceneMessage> newListener, params SceneMessage[] operates)
        {
            return Instance.Subscribe(newListener, operates);
        }

        public static bool UnSubscribeS(IGameEventListener<SceneMessage> newListener, params SceneMessage[] operates)
        {
            return Instance.UnSubscribe(newListener, operates);
        }

        public static void SendGameEventS(SceneMessage operate, params object[] args)
        {
            Instance.SendGameEvent(operate, args);
        }

        public static void SendGameEventImmediateS(SceneMessage operate, params object[] args)
        {
            Instance.SendGameEventImmediate(operate, args);
        }
    }
}
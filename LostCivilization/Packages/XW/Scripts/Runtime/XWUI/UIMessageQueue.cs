
// 自动生成的代码，请勿修改
using UnityEngine;
using XWMessageQueue;
namespace XWUI{
    public class UIMessageQueue : AMainThreadMessageQueue<UIMessage>
    {
        public static UIMessageQueue Instance
        {
            get{
                if(_instance == null)
                {
                    var newObject = new GameObject("UIMessageQueue");
                    GameObject.DontDestroyOnLoad(newObject);
                    _instance = newObject.AddComponent<UIMessageQueue>();
                }
                return _instance;
            }
        }
        private static UIMessageQueue _instance;

        public static bool SubscribeS(IGameEventListener<UIMessage> newListener, params UIMessage[] operates)
        {
            return Instance.Subscribe(newListener, operates);
        }

        public static bool UnSubscribeS(IGameEventListener<UIMessage> newListener, params UIMessage[] operates)
        {
            return Instance.UnSubscribe(newListener, operates);
        }

        public static void SendGameEventS(UIMessage operate, params object[] args)
        {
            Instance.SendGameEvent(operate, args);
        }

        public static void SendGameEventImmediateS(UIMessage operate, params object[] args)
        {
            Instance.SendGameEventImmediate(operate, args);
        }
    }
}
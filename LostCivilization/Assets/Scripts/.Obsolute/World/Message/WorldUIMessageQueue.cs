
// 自动生成的代码，请勿修改
using UnityEngine;
using XWMessageQueue;
namespace LostCivilization.World{
    public class WorldUIMessageQueue : AMainThreadMessageQueue<WorldUIMessage>
    {
        public static WorldUIMessageQueue Instance
        {
            get{
                if(_instance == null)
                {
                    var newObject = new GameObject("WorldUIMessageQueue");
                    GameObject.DontDestroyOnLoad(newObject);
                    _instance = newObject.AddComponent<WorldUIMessageQueue>();
                }
                return _instance;
            }
        }
        private static WorldUIMessageQueue _instance;

        public static bool SubscribeS(IGameEventListener<WorldUIMessage> newListener, params WorldUIMessage[] operates)
        {
            return Instance.Subscribe(newListener, operates);
        }

        public static bool UnSubscribeS(IGameEventListener<WorldUIMessage> newListener, params WorldUIMessage[] operates)
        {
            if (_instance == null) return true;
            return Instance.UnSubscribe(newListener, operates);
        }

        public static void SendGameEventS(WorldUIMessage operate, params object[] args)
        {
            Instance.SendGameEvent(operate, args);
        }

        public static void SendGameEventImmediateS(WorldUIMessage operate, params object[] args)
        {
            Instance.SendGameEventImmediate(operate, args);
        }
    }
}
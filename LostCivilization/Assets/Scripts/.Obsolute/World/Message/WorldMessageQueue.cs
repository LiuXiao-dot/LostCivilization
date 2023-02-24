
// 自动生成的代码，请勿修改
using UnityEngine;
using XWMessageQueue;
namespace LostCivilization.World{
    public class WorldMessageQueue : AChildThreadMessageQueue<WorldMessage>
    {
        public static WorldMessageQueue Instance
        {
            get{
                if(_instance == null)
                {
                    var newObject = new GameObject("WorldMessageQueue");
                    GameObject.DontDestroyOnLoad(newObject);
                    _instance = newObject.AddComponent<WorldMessageQueue>();
                }
                return _instance;
            }
        }
        private static WorldMessageQueue _instance;

        public static bool SubscribeS(IGameEventListener<WorldMessage> newListener, params WorldMessage[] operates)
        {
            return Instance.Subscribe(newListener, operates);
        }

        public static bool UnSubscribeS(IGameEventListener<WorldMessage> newListener, params WorldMessage[] operates)
        {
            return Instance.UnSubscribe(newListener, operates);
        }

        public static void SendGameEventS(WorldMessage operate, params object[] args)
        {
            Instance.SendGameEvent(operate, args);
        }

        public static void SendGameEventImmediateS(WorldMessage operate, params object[] args)
        {
            Instance.SendGameEventImmediate(operate, args);
        }
    }
}
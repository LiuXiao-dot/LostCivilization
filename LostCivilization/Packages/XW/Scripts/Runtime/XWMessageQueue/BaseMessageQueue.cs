using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using XWUtility;

namespace XWMessageQueue
{
    /// <summary>
    /// 游戏内事件管理器 T一般是枚举类型
    /// 修改：使用事件池，所有事件不需要自己再new，只需传入参数，将从事件池中自动分配一个事件。
    /// </summary>
    public abstract class BaseMessageQueue<T> : MonoBehaviour
    {
        protected int zero = 0;
        protected Queue<GameEvent<T>> eventQueue = new Queue<GameEvent<T>>();

        protected Dictionary<T, List<IGameEventListener<T>>> listeners =
            new Dictionary<T, List<IGameEventListener<T>>>();

        protected int eventCount = 0;
        protected int taskCount = 0;

        protected MessagePool<T> pool = new MessagePool<T>();

        /// <summary>
        /// 订阅事件
        /// </summary>
        public bool Subscribe(IGameEventListener<T> newListener, T operate)
        {
            if (!listeners.ContainsKey(operate))
            {
                listeners.Add(operate, new List<IGameEventListener<T>>());
            }

            if (listeners.TryGetValue(operate, out List<IGameEventListener<T>> operateListeners) &&
                !operateListeners.Contains(newListener))
            {
                operateListeners.Add(newListener);
                return true;
            }
            else
            {
                XWLogger.Warning($"重复添加监听,listener:{newListener.GetType()} operate:{operate}");
                return false;
            }
        }

        /// <summary>
        /// 订阅多个事件
        /// </summary>
        /// <param name="newListener"></param>
        /// <param name="operates"></param>
        /// <returns></returns>
        public bool Subscribe(IGameEventListener<T> newListener, params T[] operates)
        {
            var hadWarning = false;
            foreach (var operate in operates)
            {
                hadWarning |= Subscribe(newListener, operate);
            }

            return hadWarning;
        }

        /// <summary>
        /// 取消事件订阅
        /// </summary>
        public bool UnSubscribe(IGameEventListener<T> oldListener, T operate)
        {
            if (!listeners.ContainsKey(operate)) return false;

            if (listeners.TryGetValue(operate, out List<IGameEventListener<T>> operateListeners) &&
                operateListeners.Contains(oldListener))
            {
                operateListeners.Remove(oldListener);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 取消多个事件的订阅
        /// </summary>
        /// <param name="newListener"></param>
        /// <param name="operates"></param>
        /// <returns></returns>
        public bool UnSubscribe(IGameEventListener<T> newListener, params T[] operates)
        {
            var hadWarning = false;
            foreach (var operate in operates)
            {
                hadWarning |= UnSubscribe(newListener, operate);
            }

            return hadWarning;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="newInGameEvent">新的消息</param>
        protected virtual void SendGameEvent(GameEvent<T> newInGameEvent)
        {
/*#if UNITY_EDITOR
            var XWLogger = "添加：" + typeof(T).ToString() + " : " + newInGameEvent.operate;
            var args = newInGameEvent.args;
            if (args != null)
            {
                XWLogger += "\n";
                foreach (var arg in args)
                {
                    XWLogger += " " + arg.ToString();
                }
            }*/

            // XWLogger.Log(XWLogger);
//#endif
            eventQueue.Enqueue(newInGameEvent);
            eventCount++;
        }

        public void SendGameEvent(T operate, params object[] args)
        {
            var message = pool.Get();
            message.operate = operate;
            message.args = args;
            SendGameEvent(message);
        }

        /// <summary>
        /// 立即执行（不参与队列执行顺序，慎用）
        /// </summary>
        public void SendGameEventImmediate(T operate, params object[] args)
        {
            var message = pool.Get();
            message.operate = operate;
            message.args = args;
            if (listeners.TryGetValue(message.operate, out List<IGameEventListener<T>> currentListeners))
            {
                currentListeners.RemoveAll(listener => listener == null);
                var length = currentListeners.Count;
                taskCount = length;

                for (int i = length - 1; i >= 0; i--)
                {
                    currentListeners[i].OnMessage(message);
                }
            }
        }

        protected void Act()
        {
            if (taskCount > 0) return;
            if (eventQueue.Count == 0) return;
            currentEvent = eventQueue.Dequeue();
            if (listeners.TryGetValue(currentEvent.operate, out List<IGameEventListener<T>> currentListeners))
            {
                //currentListeners.RemoveAll(listener => listener == null);
                var length = currentListeners.Count;
                taskCount = length;

                for (int i = length - 1; i >= 0; i--)
                {
                    if (currentListeners[i] == null) {
                        currentListeners.RemoveAt(i);
                        Callback();
                        continue;
                    }
                    currentListeners[i].OnMessage(currentEvent);
                }
            }
        }

        private GameEvent<T> currentEvent;
        
#if UNITY_EDITOR
        [SerializeField] private string currentEventName;
        private string _empty = "无";
#endif
        public void Callback()
        {
            taskCount--;
            if (taskCount != 0) return;
            // 消息监听器全部执行完毕，释放Message到队列中
            pool.Release(currentEvent);
            eventCount--;
#if UNITY_EDITOR
            currentEventName = _empty;
#endif
        }
    }
}
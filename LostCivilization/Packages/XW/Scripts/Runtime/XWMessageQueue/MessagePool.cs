/// <summary>
/// @author TTM
/// @Modified 2022年01月30日 星期日 20:35
/// </summary>

using System.Collections.Concurrent;
using UnityEngine;

namespace XWMessageQueue
{
    /// <summary>
    /// MessagePool:
    /// 描述:消息池，从池中拿取消息对象，并设定参数，在消息事件执行完毕后归还到池中。
    /// 使用了多线程安全的ConcurrentStack,该栈在执行Push操作时会new一个Node指向头节点，产生GC，T为枚举产生32B的GC
    /// </summary>
    public class MessagePool<T>
    {
        /// <summary>
        /// 池中的事件栈(使用多线程安全的栈)
        /// </summary>
        public ConcurrentStack<GameEvent<T>> _stack;

        /// <summary>
        /// 池的大小
        /// </summary>
        private int _maxSize;

        public int MAXSize
        {
            get => _maxSize;
        }

        /// <summary>
        /// 正在使用中的实例数量。此处未存储使用中的实例。
        /// </summary>
        private int _usedInstanceAmount;

        /// <summary>
        /// 池的构造函数
        /// </summary>
        /// <param name="resource">源GameObejct，每个GameObject需要一个单独的池</param>
        /// <param name="maxSize">池的大小</param>
        public MessagePool(int maxSize = 4)
        {
            this._maxSize = maxSize;
            this._usedInstanceAmount = 0;
            _stack = new ConcurrentStack<GameEvent<T>>();
            for (int i = 0; i < maxSize; i++) {
                _stack.Push(new GameEvent<T>());
            }
        }

        /// <summary>
        /// 或取当前空闲资源的数量
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return _stack.Count;
        }

        /// <summary>
        /// 获取一个GameEvent
        /// </summary>
        /// <returns></returns>
        public GameEvent<T> Get()
        {
            if (_stack.IsEmpty) {
                // 池扩容 2倍
                for (int i = 0; i < _maxSize; i++) {
                    _stack.Push(new GameEvent<T>());
                }

                _maxSize *= 2;
            }
            _stack.TryPop(out var evt);
            if (evt == null)
                Debug.LogError("获取了错误的element");
            _usedInstanceAmount++;
            return evt;
        }


        /// <summary>
        /// 清空池
        /// </summary>
        public void Clear()
        {
            _stack.Clear();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="element">需要释放的资源</param>
        public void Release(GameEvent<T> element)
        {
            ;
            if (_stack.Count > 0) {
                _stack.TryPeek(out var old);
                if (ReferenceEquals(old, element))
                    Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
            }
            if (element == null)
                Debug.LogError("释放了错误的element");
            if (_stack.Count < MAXSize) {
                _stack.Push(element);
                _usedInstanceAmount--;
            }
        }

        /// <summary>
        /// 释放一般资源。减小池的容量
        /// </summary>
        /*public void RealseHalf()
        {
            var releaseSize = _maxSize / 2;
            for (int i = 0; i < releaseSize; i++)
            {
                _stack.Pop();
            }

            _maxSize /= 2;
        }*/
    }
}
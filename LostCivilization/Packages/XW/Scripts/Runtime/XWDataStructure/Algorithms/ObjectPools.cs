using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace XWDataStructure
{
    /// <summary>
    /// 池
    /// </summary>
    public class ObjectPools
    {
        /// <summary>
        /// 通过new创建的对象的池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ObjectPool<T> where T : new()
        {
            private readonly Stack<T> _stack;
            private readonly UnityAction<T> _actionOnGet;
            private readonly UnityAction<T> _actionOnRelease;
            private readonly bool _collectionCheck;

            /// <summary>
            /// 池的大小
            /// </summary>
            public int countAll { get; private set; }

            /// <summary>
            /// 池中的不可用对象数量
            /// </summary>
            public int CountInactive
            {
                get {
                    return _stack.Count;
                }
            }

            /// <summary>
            /// 池中的可用对象数量
            /// </summary>
            public int CountActive
            {
                get {
                    return countAll - CountInactive;
                }
            }

            public ObjectPool(UnityAction<T> actionOnGet = null, UnityAction<T> actionOnRelease = null, bool collectionCheck = true)
            {
                _stack = new Stack<T>();
                _collectionCheck = true;

                _actionOnGet = actionOnGet;
                _actionOnRelease = actionOnRelease;
                _collectionCheck = collectionCheck;
            }

            /// <summary>
            /// 获取池中的对象
            /// </summary>
            /// <returns></returns>
            public T Get()
            {
                T element;
                if (CountInactive == 0)
                {
                    element = new T();
                    countAll++;
                }
                else
                {
                    element = _stack.Pop();
                }
                _actionOnGet?.Invoke(element);
                return element;
            }
            
            public void Release(T element)
            {
#if UNITY_EDITOR // 在编辑器中进行检查
                if (_collectionCheck && _stack.Count > 0)
                {
                    if (_stack.Contains(element))
                        Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
                }
#endif
                if (_actionOnRelease != null)
                    _actionOnRelease(element);
                _stack.Push(element);
            }
            
            public PooledObject Get(out T v) => new PooledObject(v = Get(), this);
            
            /// <summary>
            /// 池化的对象,可以直接通过销毁释放
            /// </summary>
            public struct PooledObject : IDisposable
            {
                private readonly T m_ToReturn;
                private readonly ObjectPool<T> m_Pool;

                internal PooledObject(T value, ObjectPool<T> pool)
                {
                    m_ToReturn = value;
                    m_Pool = pool;
                }

                void IDisposable.Dispose() => m_Pool.Release(m_ToReturn);
            }
        }

        /// <summary>
        /// GameObject的池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class GameObjectPool
        {
            
        }

        /// <summary>
        /// SO对象的池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class SOPool<T> where T : ScriptableObject
        {
        }
        
        /// <summary>
        /// List对象的池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ListPool<T> : ObjectPool<List<T>>
        {
        }
        
        /// <summary>
        /// HashSet对象的池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class HashSetPool<T> : ObjectPool<HashSet<T>>
        {
            
        }
        
        /// <summary>
        /// Dictionary元素的池
        /// </summary>
        /// <typeparam name="TKEY"></typeparam>
        /// <typeparam name="TVALUE"></typeparam>
        public class DictionaryPool<TKEY,TVALUE> : ObjectPool<Dictionary<TKEY,TVALUE>>
        {
        }
    }
}
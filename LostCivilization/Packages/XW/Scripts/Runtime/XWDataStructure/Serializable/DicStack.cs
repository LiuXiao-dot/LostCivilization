using System.Collections.Generic;

namespace XWDataStructure
{
    /// <summary>
    /// 有字典特性的Stack
    /// </summary>
    public class DicStack<K, V>
    {
        private List<K> ks;

        private List<V> vs;

        //private int capacity;
        public int Count;


        public DicStack() : this(3)
        {
        }

        public DicStack(int capacity)
        {
            //this.capacity = capacity;
            this.Count = 0;
            ks = new List<K>(capacity);
            vs = new List<V>(capacity);
        }

        public void Push(K k, V v)
        {
            if (ContainsKey(k))
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError($"已有key:{k}");
#endif
                return;
            }

            ks.Add(k);
            vs.Add(v);
            Count++;
        }

        public void Pop(out K k, out V v)
        {
            if (Count == 0)
            {
                k = default;
                v = default;
            }
            else
            {
                var index = Count - 1;
                k = ks[index];
                v = vs[index];
                RemoveAt(index);
            }
        }

        /// <summary>
        /// 一直pop，直到遇到k,如果不包含k，不会进行pop
        /// </summary>
        public bool PopTo(K k, bool popK = false)
        {
            if (!ContainsKey(k)) return false;
            for (int i = Count - 1; i >= 0; i--)
            {
                if (!ks[i].Equals(k))
                {
                    RemoveAt(i);
                }
                else if (popK)
                {
                    RemoveAt(i);
                    break;
                }
                else
                {
                    break;
                }
            }

            return true;
        }

        private void RemoveAt(int index)
        {
            Count--;
            ks.RemoveAt(index);
            vs.RemoveAt(index);
        }

        public bool ContainsKey(K key)
        {
            return ks.Contains(key);
        }

        /// <summary>
        /// 按key移除(默认从后往前遍历)
        /// </summary>
        /// <param name="key"></param>
        public bool Remove(K key)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                if (ks[i].Equals(key))
                {
                    RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public void Peek(out K k, out V v)
        {
            k = ks[Count - 1];
            v = vs[Count - 1];
        }
    }
}
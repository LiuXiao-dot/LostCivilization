using System.Collections.Generic;
using UnityEngine;

namespace XWDataStructure
{
    /// <summary>
    /// 可序列化的字典
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    [System.Serializable]
    public class SerializableDictionary<K,V> : Dictionary<K,V>,ISerializationCallbackReceiver
    {
        #region 序列化用缓存,用完即释放,不支持多线程
        [SerializeField]private K[] keysCache;
        [SerializeField]private V[] valuesCache;

        public void OnBeforeSerialize()
        {
            if(Count == 0) return;

            keysCache = new K[Count];
            valuesCache = new V[Count];
            var index = 0;
            foreach (var kv in this)
            {
                keysCache[index] = kv.Key;
                valuesCache[index] = kv.Value;
                index++;
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            if (keysCache.IsEmptyOrNull()) return;
            var count = keysCache.Length;
            for (int i = 0; i < count; i++)
            {
                Add(keysCache[i],valuesCache[i]);
            }

            keysCache = null;
            valuesCache = null;
        }
        #endregion


    }
}
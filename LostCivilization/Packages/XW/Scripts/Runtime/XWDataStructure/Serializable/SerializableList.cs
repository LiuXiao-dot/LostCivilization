using System.Collections.Generic;
using UnityEngine;

namespace XWDataStructure
{
    /// <summary>
    /// 支持序列化不同对象类型的list
    /// </summary>
    public class SerializableList : List<object>, ISerializationCallbackReceiver
    {
        public SerializableType[] types;
        public string[] temps;
        public void OnBeforeSerialize()
        {
            var count = this.Count;
            temps = new string[count];
            types = new SerializableType[count];
            for (var i = 0; i < count; i++)
            {
                temps[i] = JsonUtility.ToJson(this[i]);
                types[i] = this[i].GetType();
            }
        }

        public void OnAfterDeserialize()
        {
            if(temps == null) return;
            var count = temps.Length;
            for (var i = 0; i < count; i++)
            {
                Add(JsonUtility.FromJson(temps[i],types[i]));
            }

            temps = null;
            types = null;
        }
    }
}
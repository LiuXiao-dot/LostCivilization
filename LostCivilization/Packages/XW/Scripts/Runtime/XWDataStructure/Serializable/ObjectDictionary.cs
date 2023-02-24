using System;
using System.Collections.Generic;
using UnityEngine;

namespace XWDataStructure
{
    [Serializable]
    public class ObjectDictionary : Dictionary<Type,object>,ISerializationCallbackReceiver
    {
        [SerializeField]private SerializableType[] types;
        [SerializeField]private string[] temps;
        
        public void OnBeforeSerialize()
        {
            if(Count == 0) return;

            var count = this.Count;
            temps = new string[count];
            types = new SerializableType[count];
            var index = 0;
            foreach (var kv in this)
            {
                types[index] = kv.Key;
                temps[index] = JsonUtility.ToJson(kv.Value);
                index++;
            }
        }

        public void OnAfterDeserialize()
        {
            if(types == null) return;
            var count = types.Length;
            for (int i = 0; i < count; i++)
            {
                Add(types[i],JsonUtility.FromJson(temps[i],types[i]));
            }

            types = null;
            temps = null;
        }
    }
}
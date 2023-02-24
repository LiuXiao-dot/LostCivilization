using System;
using System.Collections.Generic;
using System.Reflection;

namespace XWDataStructure
{
    /// <summary>
    /// 可序列化的Type,编辑器中展示为Type和所有继承自Type的类
    /// </summary>
    [Serializable]
    public class SerializableType
    {
        /// <summary>
        /// 同一个type对应同一个SerializableType
        /// </summary>
        private static Dictionary<Type, SerializableType> pools = new Dictionary<Type, SerializableType>();

        public string assmblyName;
        public string typeName;
        public string namespaceName;

        public SerializableType(Type type)
        {
            assmblyName = type.Assembly.FullName;
            typeName = type.Name;
            namespaceName = type.Namespace;
        }

        public static implicit operator SerializableType(Type type)
        {
            if (!pools.ContainsKey(type))
            {
                pools.Add(type,new SerializableType(type));
            }
            return pools[type];
        }

        public static implicit operator Type(SerializableType type)
        {
            return Assembly.Load(type.assmblyName).GetType($"{type.namespaceName}.{type.typeName}");
        }
    }
}
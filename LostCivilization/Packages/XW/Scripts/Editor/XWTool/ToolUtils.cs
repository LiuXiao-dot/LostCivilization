using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using XWUtility;

namespace XWTool
{
    public static class ToolUtils
    {
        /// <summary>
        /// 查找所有使用ToolDataAttribute/T标记的对象（不包含抽象类和接口）
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="temp"></param>
        /// <typeparam name="T"></typeparam>
        public static void GetAllMarkedType<T>(this AssemblyDefinitionAsset[] assemblies, List<Type> temp) where T : Attribute
        {
            foreach (var assembly in assemblies)
            {
                if(assembly == null) continue;
                if (temp == null) temp = new List<Type>();
                Assembly.Load(assembly.name).GetAllMarkedType<T>(temp);
            }
        }
        
        /// <summary>
        /// 查找所有使用ToolDataAttribute/T标记的对象（不包含抽象类和接口）
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="target"></param>
        /// <param name="temp"></param>
        /// <typeparam name="T"></typeparam>
        public static void GetAllMarkedType<T>(this Assembly[] assemblies, Assembly target,List<Type> temp) where T : Attribute
        {
            foreach (var assembly in assemblies)
            {
                if(!assembly.GetReferencedAssemblies().Any(temp=>temp.FullName == target.FullName)) continue;
                if(assembly == null) continue;
                if (temp == null) temp = new List<Type>();
                assembly.GetAllMarkedType<T>(temp);
            }
        }

        /// <summary>
        /// 获取所有继承与T的类
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="types"></param>
        /// <typeparam name="T"></typeparam>
        public static void GetAllChildType<T>(this AssemblyDefinitionAsset[] assemblies,List<Type> temp)
        {
            foreach (var assembly in assemblies)
            {
                if(assembly == null) continue;
                if (temp == null) temp = new List<Type>();
                Assembly.Load(assembly.name).GetAllChildType<T>(temp);
            }
        }
        
        public static void GetAllChildType<T>(this DefaultAsset[] assemblies,List<Type> temp)
        {
            foreach (var assembly in assemblies)
            {
                if(assembly == null) continue;
                if (temp == null) temp = new List<Type>();
                Assembly.Load(assembly.name).GetAllChildType<T>(temp);
            }
        }
        
        public static void RemoveAllUnMarkedType<T>(this List<Type> types) where T : Attribute
        {
            if(types == null) return;
            types.RemoveAll(temp => temp.GetCustomAttribute(typeof(T)) == null);
        }
    }
}
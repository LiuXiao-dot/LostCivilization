using System;
using System.Collections.Generic;
using System.Reflection;

namespace XWUtility
{
    public static class ReflectionExtension
    {

        public static void GetAllGenericChildType(this IEnumerable<Assembly> assemblies,string typeName, List<Type> temp)
        {
            foreach (var assembly in assemblies)
            {
                assembly.GetAllGenericChildType(typeName,temp);
            }
        }

        public static void GetAllGenericChildType(this Assembly assembly, string typeName, List<Type> temp)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsAbstract || type.IsInterface) continue;
                if(!type.IsChildOf(typeName)) continue;
                temp.Add(type);
            }
        }
        
        /// <summary>
        /// 如果是继承自parent，将返回type,否则返回null
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Type GetTargetParent<T>(this Type self)
        {
            return self.GetTargetParent(typeof(T));
        }

        public static Type GetTargetParent(this Type self, Type parent)
        {
            var tempParent = self.BaseType;
            do
            {
                if (tempParent.FullName == parent.FullName) return tempParent;
            } while (self.BaseType != typeof(object));

            return null;
        }

        public static FieldInfo FindFieldInfo(this Type self, string fieldName, BindingFlags bindingFlags)
        {
            var tempType = self;
            do
            {
                var field = tempType.GetField(fieldName, bindingFlags);
                if (field != null) return field;
                tempType = tempType.BaseType;
            } while (tempType != null);

            return null;
        }


        /// <summary>
        /// 查找所有使用ToolDataAttribute/T标记的对象（不包含抽象类和接口）
        /// </summary>
        /// <param name="assembly"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void GetAllMarkedType<T>(this Assembly assembly, List<Type> temp) where T : Attribute
        {
            if (temp == null) temp = new List<Type>();
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsAbstract || type.IsInterface) continue;
                if (type.GetCustomAttribute<T>(false) == null) continue;
                temp.Add(type);
            }
        }

        public static bool IsChildOf(this Type child, Type parent)
        {
            var temp = child;
            do
            {
                if (temp.Name == parent.Name) return true;
                temp = temp.BaseType;
            } while (temp != null);

            return false;
        }

        public static bool IsChildOf<T>(this Type child)
        {
            return child.IsChildOf(typeof(T));
        }

        public static bool IsChildOf(this Type child, string parent)
        {
            var temp = child;
            do
            {
                if (temp.Name == parent) return true;
                temp = temp.BaseType;
            } while (temp != null);

            return false;
        }

        public static void GetAllChildType<T>(this Assembly assembly, List<Type> temp)
        {
            if (temp == null) temp = new List<Type>();
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsAbstract || type.IsInterface) continue;
                if (!type.IsChildOf<T>()) continue;
                temp.Add(type);
            }
        }


        public static void GetAllChildType<T>(this Assembly[] assemblies, List<Type> temp)
        {
            foreach (var assembly in assemblies)
            {
                assembly.GetAllChildType<T>(temp);
            }
        }
    }
}
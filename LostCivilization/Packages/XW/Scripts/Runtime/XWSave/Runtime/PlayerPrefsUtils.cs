using System;
using System.Collections.Generic;
using UnityEngine;
using XWUtility;

namespace XWSave
{
    /// <summary>
    /// 可存储 int float string 类型的数据
    /// </summary>
    public sealed class PlayerPrefsUtils
    {
        public static void Set(string key,object value,bool overwrite = true)
        {
            var realKey = key;
            if(!overwrite)
            {
                var index = 0;
                while (PlayerPrefs.HasKey(realKey))
                {
                    realKey = key + index;
                    index++;
                }
            }

            if(value is string)
            {
                PlayerPrefs.SetString(realKey, (string)value);
                return;
            }
            if(value is int)
            {
                PlayerPrefs.SetInt(realKey, (int)value);
                return;
            }
            if (value is float)
            {
                PlayerPrefs.SetFloat(realKey, (float)value);
                return;
            }
            XWLogger.Error("存储类型错误:" + value.GetType() + "-" + realKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getAll">是否获取所有</param>
        public static List<string> GetString(string key,bool getAll = false)
        {
            var realKey = key;
            var values = new List<string>();
            if(getAll)
            {
                var index = 0;
                while (PlayerPrefs.HasKey(realKey))
                {
                    values.Add(PlayerPrefs.GetString(realKey));
                    realKey = key + index;
                    index++;
                }
            }
            else if (PlayerPrefs.HasKey(realKey))
            {
                values.Add(PlayerPrefs.GetString(realKey));
            }
            return values;
        }

        /// <summary>
        /// 存档
        /// </summary>
        /// <typeparam name=""></typeparam>
        public static void Set(object dataInstance)
        {
            Type type = dataInstance.GetType();
            var fields = type.GetFields();
            if (fields == null || fields.Length <= 0)
            {
                Debug.LogWarning(type.FullName + "don't have a field");
                return;
            }

            foreach (var field in fields)
            {
                var typeName = field.FieldType.Name;
                var fieldName = field.Name;
                var value = field.GetValue(dataInstance);
                switch (typeName) {
                    case "Int32":
                        PlayerPrefs.SetInt(fieldName,(int)value + 1);
                        break;
                    case "Single":
                        PlayerPrefs.SetFloat(fieldName, (float)value);
                        break;
                    case "String":
                        PlayerPrefs.SetString(fieldName, (string)value);
                        break;
                    default:
                        XWLogger.Warning($"{typeName}类型数据将不会被存储");
                        break;
                }
            }
        }

        /// <summary>
        /// 读档
        /// </summary>
        /// <typeparam name=""></typeparam>
        public static void Get(object dataInstance)
        {
            Type type = dataInstance.GetType();
            var fields = type.GetFields();
            if (fields == null || fields.Length <= 0)
            {
                XWLogger.Warning(type.FullName + "don't have a field");
                return;
            }

            foreach (var field in fields)
            {
                var typeName = field.FieldType.Name;
                var fieldName = field.Name;
                var value = field.GetValue(dataInstance);
                switch (typeName) {
                    case "Int32":
                        value = PlayerPrefs.GetInt(fieldName, (int)value);
                        break;
                    case "Single":
                        value = PlayerPrefs.GetFloat(fieldName, (float)value);
                        break;
                    case "String":
                        value = PlayerPrefs.GetString(fieldName, (string)value);
                        break;
                    default:
                        break;
                }
                field.SetValue(dataInstance,value);
            }
        }
    }
}

namespace XWUtilityEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using XWUtility;

    public static class ScriptableSingletonManager
    {
        [InitializeOnLoadMethod]
        public static void InitAllSingletons()
        {
            var types = new List<Type>();
            AppDomain.CurrentDomain.GetAssemblies().GetAllGenericChildType("ScriptableObjectSingleton`1", types);
            foreach (var type in types)
            {
                // 生成一个实例
                var xWFilePathAttribute = type.GetCustomAttribute<XWFilePathAttribute>();
                if (xWFilePathAttribute == null) continue;

                var paths = AssetDatabase.FindAssets($"t:{type.Name}");
                var sos = paths.Select(temp =>
                    AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(temp)));
                if (!sos.Any())
                {
                    var newSO = ScriptableObject.CreateInstance(type);
                    AssetDatabase.CreateAsset(newSO,xWFilePathAttribute.filePath);
                    /*var temp = $"Assets/{Path.GetFileName(xWFilePathAttribute.filePath)}";
                    AssetDatabase.CreateAsset(newSO, temp);
                    File.Move($"{EditorUtils.BasePath}/{temp}",$"{EditorUtils.BasePath}/{xWFilePathAttribute.filePath}");*/
                }
                else
                {
                    var oldPath = AssetDatabase.GetAssetPath(sos.First());
                    if (oldPath != xWFilePathAttribute.filePath)
                        AssetDatabase.MoveAsset(oldPath, xWFilePathAttribute.filePath);
                }

                var fieldInfo = type.GetField("instance", BindingFlags.Static | BindingFlags.NonPublic);
                if (fieldInfo == null) continue;
                fieldInfo.SetValue(null, sos.First());
            }
        }
    }
}
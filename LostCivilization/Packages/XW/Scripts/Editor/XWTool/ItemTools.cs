using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using XWUtility;
using XWUtilityEditor;

namespace XWTool
{
    /// <summary>
    /// 非界面中的工具项
    /// </summary>
    internal static class ItemTools
    {
        /// <summary>
        /// 展示工具菜单
        /// </summary>
        [MenuItem("XWTool/工具菜单")]
        private static void ExcuteToolMenu()
        {
            var window = EditorWindow.GetWindow<ToolMenu>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        [MenuItem("XWTool/刷新程序集")]
        private static void ExcuteRefreshASM()
        {
            EditorUtility.RequestScriptReload();
        }

        [MenuItem("XWTool/刷新SO单例")]
        private static void InitAllSingletons()
        {
            ScriptableSingletonManager.InitAllSingletons();
            /*var types = new List<Type>();
            AppDomain.CurrentDomain.GetAssemblies().GetAllGenericChildType("ScriptableObjectSingleton`1", types);
            foreach (var type in types) {
                // 生成一个实例
                var xWFilePathAttribute = type.GetCustomAttribute<XWFilePathAttribute>();
                if (xWFilePathAttribute == null) continue;

                var paths = AssetDatabase.FindAssets($"t:{type.Name}");
                var sos = paths.Select(temp =>
                    AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(temp)));
                if (!sos.Any()) {
                    var newSO = ScriptableObject.CreateInstance(type);
                    AssetDatabase.CreateAsset(newSO, xWFilePathAttribute.filePath);
                } else {
                    var oldPath = AssetDatabase.GetAssetPath(sos.First());
                    if (oldPath != xWFilePathAttribute.filePath)
                        AssetDatabase.MoveAsset(oldPath, xWFilePathAttribute.filePath);
                }

                var fieldInfo = type.GetField("instance", BindingFlags.Static | BindingFlags.NonPublic);
                if (fieldInfo == null) continue;
                fieldInfo.SetValue(null, sos.First());
            }*/
        }
    }
}
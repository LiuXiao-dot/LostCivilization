using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XWUtility;
using Object = UnityEngine.Object;

namespace XWTool
{
    public static class EditorUtils
    {
        private static string basePath = null;

        public static string BasePath
        {
            get
            {
                if (basePath == null)
                {
                    basePath = Application.dataPath.Remove(Application.dataPath.Length - 7,7);
                }

                return basePath;
            }
        }
        
        /// <summary>
        /// 检测->创建目标路径的目录
        /// </summary>
        /// <param name="filePath"></param>
        public static void CreateDir(string filePath)
        {
            var dir = GetDir(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static string GetDir(string filePath)
        {
            return filePath.Replace(Path.GetFileName(filePath), "");
        }

        public static void CheckAsset<T>() where T : SerializedScriptableObject
        {
            var type = typeof(T);
            // 生成一个实例
            var xWFilePathAttribute = type.GetCustomAttribute<XWFilePathAttribute>();
            if (xWFilePathAttribute == null) return;

            var paths = AssetDatabase.FindAssets($"t:{type.Name}");
            var sos = paths.Select(temp =>
                AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(temp)));
            if (!sos.Any())
            {
                var newSO = ScriptableObject.CreateInstance(type);
                /*AssetDatabase.CreateAsset(newSO,Path.GetFileName(xWFilePathAttribute.filePath));
                AssetDatabase.MoveAsset(Path.GetFileName(xWFilePathAttribute.filePath), xWFilePathAttribute.filePath);*/
                AssetDatabase.CreateAsset(newSO, xWFilePathAttribute.filePath);
            }
            else
            {
                if (sos.Count() > 1)
                {
                    XWLogger.Error("存在多个UIPrefabConfigSO");
                    return;
                }

                var oldPath = AssetDatabase.GetAssetPath(sos.First());
                if (oldPath != xWFilePathAttribute.filePath)
                    AssetDatabase.MoveAsset(oldPath, xWFilePathAttribute.filePath);
            }
        }

        public static void CreateAsset<T>(T t, string path) where T : Object
        {
            //var folder = GetDir(path);
            CreateDir(path);
            AssetDatabase.CreateAsset(t, path);
        }

        public static List<T> FindAssets<T>() where T : Object
        {
            var ts = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            return ts.Select(temp => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(temp))).ToList();
        }

        /*public static void CombineWindows()
        {
            Type sceneViewType = typeof(SceneView);
            //创建最外层容器
            object containerInstance = ContainerWindowWrap.CreateInstance();
            //创建分屏容器
            object splitViewInstance = SplitViewWrap.CreateInstance();
            //设置根容器
            ContainerWindowWrap.SetRootView(containerInstance, splitViewInstance);
    
            //tool面板与timeline面板分割面板
            object window_sceneSplitView = SplitViewWrap.CreateInstance();
            SplitViewWrap.SetPosition(window_sceneSplitView, new Rect(0, 0, 1920, 1080));
            //设置垂直状态
            SplitViewWrap.SetVertical(window_sceneSplitView, false);
            object sceneDockArea = DockAreaWrap.CreateInstance();
            var sceneWidth = 1080 * 1080 / 1920;
            DockAreaWrap.SetPosition(sceneDockArea, new Rect(0, 0, sceneWidth, 1080));
            var sceneWindow = ScriptableObject.CreateInstance(sceneViewType) as SceneView;
            sceneWindow.orthographic = true;
            sceneWindow.in2DMode = true;
            
            DockAreaWrap.AddTab(sceneDockArea, sceneWindow);
            SplitViewWrap.AddChild(window_sceneSplitView, sceneDockArea);
    
            //添加timeline窗体
            object windwoDock = DockAreaWrap.CreateInstance();
            DockAreaWrap.SetPosition(windwoDock, new Rect(sceneWidth, 0, 1920 - sceneWidth, 1080));
            EditorWindow windowEditorWindow = (EditorWindow)ScriptableObject.CreateInstance(typeof(WindowEditorWindow));
            //windowEditorWindow.minSize = new Vector2(1920 - sceneWidth, 1080);
            DockAreaWrap.AddTab(windwoDock, windowEditorWindow);
            SplitViewWrap.AddChild(window_sceneSplitView, windwoDock);
    
            //添加tool_timeline切割窗体
            SplitViewWrap.AddChild(splitViewInstance, window_sceneSplitView);
    
            EditorWindowWrap.MakeParentsSettingsMatchMe(sceneWindow);
            EditorWindowWrap.MakeParentsSettingsMatchMe(windowEditorWindow);
    
            ContainerWindowWrap.SetPosition(containerInstance, new Rect(0, 0, 1920, 1080));
            SplitViewWrap.SetPosition(splitViewInstance, new Rect(0, 0, 1920, 1080));
            ContainerWindowWrap.Show(containerInstance, 0, true, false, true);
            ContainerWindowWrap.OnResize(containerInstance);
        }*/
    }
}
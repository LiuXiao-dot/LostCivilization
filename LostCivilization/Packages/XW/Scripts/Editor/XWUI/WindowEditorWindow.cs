using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using XWDataStructure;
using XWTool;
using XWUI;
using XWUtility;

namespace XWUIEditor
{
    /// <summary>
    /// 窗口编辑窗口
    /// </summary>
    public class WindowEditorWindow : OdinMenuEditorWindow
    {
        /// <summary>
        /// 生成Prefab的路径
        /// </summary>
        [FolderPath] public string prefabPath;
        [FolderPath] public string codePath;
        private bool dirty;

        /// <summary>
        /// 展示工具菜单
        /// </summary>
        [MenuItem("XWTool/窗口编辑")]
        private static void ExcuteToolMenu()
        {
            var window = EditorWindow.GetWindow<WindowEditorWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
            window.prefabPath = EditorPrefs.GetString("Window/PrefabFolder",
                $"{XWFilePathAttribute.XWEDITORPATH}/Windows/Prefabs");
            window.codePath = EditorPrefs.GetString("Window/CodeFolder",$"{XWFilePathAttribute.XWEDITORPATH}/Windows/Prefabs" );
        }

        #region NonSerialized

        private List<WindowPrefabSO> windows = new List<WindowPrefabSO>();

        private Dictionary<WindowLayer, string> menuPathDic = new Dictionary<WindowLayer, string>()
        {
            { WindowLayer.MAIN, "主窗口" },
            { WindowLayer.CHILD, "子窗口" },
            { WindowLayer.PANEL, "面板" },
            { WindowLayer.POPUP, "弹窗" },
            { WindowLayer.LOADING, "加载界面" },
        };

        #endregion

        /// <summary>
        /// 构建窗口菜单
        /// </summary>
        /// <returns></returns>
        protected override OdinMenuTree BuildMenuTree()
        {
            RefreshAllWindows();
            OdinMenuTree tree = new OdinMenuTree();
            tree.Config.DrawSearchToolbar = true;
            tree.Config.DefaultMenuStyle.Height = 22;
            tree.Add("设置",this);
            foreach (var window in windows)
            {
                tree.Add($"{menuPathDic[window.windowLayer]}/{window.Name}", window);
            }
            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("创建Window")))
                {
                    ScriptableObjectCreateTool.CreateScriptableObject<WindowPrefabSO>(
                        "Assets/XW/EditorResources/Windows",
                        "NewWindow",
                        (so, name) =>
                        {
                            so.PrefabDirectory = prefabPath;
                            so.CodeDirectory = codePath;

                            if (selected is { Value: WindowPrefabSO template })
                            {
                                so.windowBg = template.windowBg;
                                so.windowLayer = template.windowLayer;
                                so.windowHeader = template.windowHeader;
                            }
                            so.InitAllAssets(Path.GetFileNameWithoutExtension(name));
                            dirty = true;
                        });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("删除Window")))
                {
                    var windowNames = "";
                    this.MenuTree.Selection.Where(temp => temp.Value is WindowPrefabSO).ForEach(temp => windowNames = $"{windowNames}\n{temp.Name}");
                    if(EditorUtility.DisplayDialog("删除窗口", $"是否删除以下窗口:\n{windowNames}\n注意:会同时删除Prefab和脚本文件", "确认", "取消"))
                    {
                        this.MenuTree.Selection.Where(temp => temp.Value is WindowPrefabSO).ForEach(temp =>
                        {
                            ((WindowPrefabSO)temp.Value).DestroyWindow();
                            dirty = true;
                        });
                    }
                }

                /*
                if (SirenixEditorGUI.ToolbarButton(new GUIContent("清除没有配置数据的Window")))
                {
                    
                }*/
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EditorPrefs.SetString("Window/PrefabFolder", prefabPath);
            EditorPrefs.SetString("Window/CodeFolder", codePath);
            if (dirty)
            {
                SyncWindowData();
            }
        }

        #region Utils

        internal void RefreshAllWindows()
        {
            windows.Clear();
            windows = EditorUtils.FindAssets<WindowPrefabSO>();
        }

        [Button("同步窗口数据")]
        public void SyncAll()
        {
            SyncWindowData();
            var tempWindows  = EditorUtils.FindAssets<WindowPrefabSO>();
            foreach (var tempWindow in tempWindows)
            {
                tempWindow.InitAllAssets(tempWindow.Name);
            }
        }
        public void SyncWindowData()
        {
            var tempWindows  = EditorUtils.FindAssets<WindowPrefabSO>();
            var so = WindowManagerConfigSO.Instance;
            if(so.windowDatas == null)
                so.windowDatas = new SerializableDictionary<string,WindowData>();
            so.windowDatas.Clear();
            foreach (var tempWindow in tempWindows)
            {
                so.windowDatas.Add(tempWindow.Name,new WindowData()
                {
                    name = tempWindow.Name,
                    bg = tempWindow.windowBg,
                    layer = tempWindow.windowLayer,
                    ctlType = tempWindow.ctlCode.GetClass(),
                    reference = new AssetReferenceGameObject(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(tempWindow.customWindow)))
                });
            }
            EditorUtility.SetDirty(so);
            AssetDatabase.Refresh();
        }
        #endregion
    }
}
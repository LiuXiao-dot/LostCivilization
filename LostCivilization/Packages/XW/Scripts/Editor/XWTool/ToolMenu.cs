using XWUtility;

namespace XWTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// 工具的菜单界面
    /// </summary>
    internal sealed class ToolMenu : OdinMenuEditorWindow
    {
        #region NonSerialized
        /// <summary>
        /// 当前脚本所在的程序集
        /// </summary>
        private Assembly defaultAssembly = typeof(ToolMenu).Assembly;
        /// <summary>
        /// 所有工具的缓存列表
        /// </summary>
        private List<object> tools = new List<object>();
        #endregion

        /// <summary>
        /// 构建工具菜单
        /// </summary>
        /// <returns></returns>
        protected override OdinMenuTree BuildMenuTree()
        {
            RefreshAllTools();
            OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true);
            tree.Config.DrawSearchToolbar = true;
            tree.Config.DefaultMenuStyle.Height = 22;
            foreach (var temp in tools)
            {
                var tempTool = temp.GetType().GetCustomAttribute<ToolAttribute>();
                //if (tempTool.icon == ToolIconEnum.NONE)
                    tree.Add(tempTool.path, temp);
                /*else
                    tree.Add(tempTool.path, temp, ToolConstant.DefaultIcons[tempTool.icon]);*/
            }
            
            tree.SortMenuItemsByName();
            return tree;
        }
        
        #region Utils

        /// <summary>
        /// 获取所有工具
        /// 1.继承自ScriptableObject的会查找.asset资产，有多个会展示多个
        /// 2.非ScriptableObject会创建一个实例
        /// </summary>
        /// <returns></returns>
        internal void RefreshAllTools()
        {
            tools.Clear();
            var types = new List<Type>();
            defaultAssembly.GetAllMarkedType<ToolAttribute>(types);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            assemblies.GetAllMarkedType<ToolAttribute>(defaultAssembly, types);

            foreach (var temp0 in types)
            {
                if (temp0.IsChildOf(typeof(UnityEngine.ScriptableObject)))
                {
                    var tempSos = AssetDatabase.FindAssets($"t:{temp0.Name}")
                        .Select(temp => AssetDatabase.GUIDToAssetPath(temp))
                        .Select(temp => AssetDatabase.LoadAssetAtPath<ScriptableObject>(temp));
                    if (tempSos.Count() == 0)
                    {
                        XWLogger.Error($"{temp0.FullName}没有创建实例");
                    }

                    tools.AddRange(tempSos);
                }
                else
                {
                    tools.Add(Activator.CreateInstance(temp0));
                }
            }
        }

        #endregion
    }
}
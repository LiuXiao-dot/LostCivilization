using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XWDataStructure;
using XWUtility;

namespace XWTool
{   
    /// <summary>
    /// 工具菜单的基础配置
    /// </summary>
    [Tool( ToolConstant.Home)]
    [XWFilePath("ToolMenuConfig.asset",XWFilePathAttribute.PathType.InXWEditor)]
    public class ToolMenuConfig : ScriptableObjectSingleton<ToolMenuConfig>
    {
        [LabelText("程序集")]
        [Tooltip("可供部分工具使用，只查找部分程序集")]
        public AssemblyDefinitionAsset[] assemblies;
        
        [AssetList(CustomFilterMethod = "CheckName")]
        public DefaultAsset[] dlls;

        public AssemblyDefinitionAsset[] ugui;

        public bool CheckName(DefaultAsset asset)
        {
            return Path.GetExtension(AssetDatabase.GetAssetPath(asset)) == ".dll";
        }

        public void GetAllChildType<T>(List<Type> temp, bool needUgui = false)
        {
            dlls.GetAllChildType<T>(temp);
            assemblies.GetAllChildType<T>(temp);
            if (!needUgui) return;
            var unityCore = typeof(Transform).Assembly;
            unityCore.GetAllChildType<T>(temp);
            ugui.GetAllChildType<T>(temp);
        }

        public void GetAllMarkedType<T>(List<Type> temp, bool needUgui = false)  where T : Attribute
        {
            assemblies.GetAllMarkedType<T>(temp);
            if (!needUgui) return;
            var unityCore = typeof(Transform).Assembly;
            unityCore.GetAllMarkedType<T>(temp);
            ugui.GetAllMarkedType<T>(temp);
        }
    }
}
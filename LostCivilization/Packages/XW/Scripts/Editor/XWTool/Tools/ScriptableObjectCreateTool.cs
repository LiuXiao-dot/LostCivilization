using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XWTool
{
    /// <summary>
    /// ScriptableObject创建工具
    /// </summary>
    [Tool(ToolConstant.Home + "/ScriptableObject创建")]
    [Serializable]
    public class ScriptableObjectCreateTool
    {
        /// <summary>
        /// 所有ScriptableObject类型（不包含抽象类）
        /// </summary>
        [Searchable] [SerializeField] [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        private List<TempButton> allScriptableObjectTypes = new List<TempButton>();

        /// <summary>
        /// 全部的type
        /// </summary>
        private List<Type> types;

        public ScriptableObjectCreateTool()
        {
            var toolMenuConfig = ToolMenuConfig.Instance;
            types = new List<Type>();
            //typeof(ToolMenuConfig).Assembly.GetAllChildType<ScriptableObject>(types);
            toolMenuConfig.GetAllChildType<ScriptableObject>(types);
            var length = types.Count;
            for (int i = 0; i < length; i++)
            {
                allScriptableObjectTypes.Add(new TempButton(){index = i,name = types[i].Name});
            }
        }

        public void CreateScriptableObject(TempButton temp)
        {
            var type = types[temp.index];
            var path = EditorUtility.SaveFilePanelInProject("创建ScriptableObject",type.Name,"asset",$"创建{type.Name}的实例");
            if(string.IsNullOrEmpty(path)) return;
            var ins = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(ins,path);
        }

        [FieldButton("CreateScriptableObject")]
        [Serializable]
        public class TempButton
        {
            public int index;
            public string name;

            public override string ToString()
            {
                return name;
            }
        }

        public static void CreateScriptableObject<T>(string defaultPath, string name, Action<T,string> beforeSave = null) where T : ScriptableObject
        {
            var path = EditorUtility.SaveFilePanelInProject("创建ScriptableObject",name,"asset","创建SO实例",defaultPath);
            //UnityEngine.Debug.Log($"0创建{path}");
            if (string.IsNullOrEmpty(path)) return;
            var ins = ScriptableObject.CreateInstance<T>();
            beforeSave?.Invoke(ins,path);
            AssetDatabase.CreateAsset(ins,path);
            //UnityEngine.Debug.Log($"1创建{path}");
            AssetDatabase.Refresh();
        }
    }
}
using XWTool;
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using XWUI;
using XWUtility;
namespace XWUIEditor
{
    [Tool("UI管理/Prefab转C#")]
    [XWFilePath("PrefabComponentConfigSO.asset", XWFilePathAttribute.PathType.InXWEditor)]
    public class UIPrefabConfigSO : SerializedScriptableObject
    {
        [TextArea(10, 10)] [Sirenix.OdinInspector.ReadOnly] public readonly string Tip =
            @"code中，部分字符串表示以下含义:
{PrefabName}:prefab的名字
{CName}:组件名
{CFName}:组件的Key值
{FName}:组件的field名
命名方式：组件A|组件B_Name 如：TF_a,TF|BTN_b
";

        [SerializeField] [ReadOnly]public Dictionary<string, TempType> componentConfig;

        private void OnEnable()
        {
            Refresh();
        }

        public void Reset()
        {
            Refresh();
        }

        private void Refresh()
        {
            if(ToolMenuConfig.Instance == null) return;
            componentConfig = new Dictionary<string, TempType>();
            componentConfig.Add("TF", new TempType(typeof(Transform)));
            componentConfig.Add("BTN", new TempType(typeof(Button)));
            componentConfig.Add("TOG", new TempType(typeof(Toggle)));
            componentConfig.Add("IMG", new TempType(typeof(Image)));

            // 查找所有ShortKey标记的类
            var temp = new List<Type>();
            ToolMenuConfig.Instance.GetAllMarkedType<ShortKeyAttribute>(temp);
            foreach (var type in temp) {
                var attr = type.GetCustomAttribute<ShortKeyAttribute>();
                var shortkey = attr.key;
                shortkey = (string.IsNullOrEmpty(shortkey) ? type.Name: shortkey).ToUpper();
                componentConfig.Add( shortkey, new TempType(type));
            }
        }

        [InlineProperty]
        public struct TempType
        {
            //[ValueDropdown("CheckType")] 
            [ReadOnly]public Type type;
            //[TextArea]
            /// <summary>
            /// 暂时隐藏掉
            /// </summary>
            [HideInInspector] public string code;

            public TempType(Type type)
            {
                this.code = "";
                this.type = type;
            }

            public IEnumerable<Type> CheckType()
            {
                var toolConfig = ToolMenuConfig.Instance;
                var temp = new List<Type>();
                toolConfig.GetAllChildType<Component>(temp, true);
                return temp;
            }
        }
    }
}
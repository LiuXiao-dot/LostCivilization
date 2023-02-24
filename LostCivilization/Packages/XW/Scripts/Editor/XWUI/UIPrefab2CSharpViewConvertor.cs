using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using XWConverter;

namespace XWUIEditor
{
    /// <summary>
    /// 为Unity的prefab生成UI控制脚本
    /// </summary>
    public class UIPrefab2CSharpViewConvertor : AILDataConvert<DictionaryILData, CSharpILData>
    {
        /// <summary>
        /// 命名空间
        /// </summary>
        private string _namespaceText;

        public UIPrefab2CSharpViewConvertor(string namespaceText)
        {
            this._namespaceText = namespaceText;
        }

        public override CSharpILData Convert(DictionaryILData from)
        {
            var cSharpILData = new CSharpILData();
            var prefabName = from[PrefabReader.PrefabName][0];
            var className = $"{prefabName}View";
            GenerateInitText(from, out string usings, out string fields, out string awakes);
            cSharpILData.csharpText = $@"
// 由UnitySourceConvertor生成,请勿手动修改
{usings}
namespace {_namespaceText}
{{
    public class {className} : MonoBehaviour, IPrefabView
    {{
        private Transform _transform;
        {fields}
        private void Awake()
        {{
            _transform = transform;
            {awakes}
        }}
    }}
}}";
            return cSharpILData;
        }

        /// <summary>
        /// 生成初始化文本
        /// </summary>
        private void GenerateInitText(DictionaryILData from, out string usings, out string fields, out string awakes)
        {
            var fromData = from.ilDatas;
            usings = "";
            fields = "";
            awakes = "";
            var usingSet = new HashSet<string>();
            usingSet.Add("XWDataStructure");
            usingSet.Add("UnityEngine");
            var so = AssetDatabase.FindAssets("t:UIPrefabConfigSO");
            if (so == null || so.Length == 0) UnityEngine.Debug.LogError("UIPrefabConfigSO需要一个实例");
            var dic = AssetDatabase.LoadAssetAtPath<UIPrefabConfigSO>(AssetDatabase.GUIDToAssetPath(so.First()))
                .componentConfig;
            foreach (var temp in fromData)
            {
                switch (temp.Key)
                {
                    case PrefabReader.PrefabName:
                        break;
                    default:
                        if(!dic.ContainsKey(temp.Key)) continue;
                        var type = dic[temp.Key].type;
                        usingSet.Add(type.Namespace);
                        foreach (var path in temp.Value)
                        {
                            var fieldName = $"_{path.Split('_').Last()}{temp.Key}";
                            fields = $"{fields}public {type.Name} {fieldName};\n\t\t";
                            awakes =
                                $"{awakes}\n\t\t\t{fieldName} = _transform.Find(\"{path}\").GetComponent<{type.Name}>();";
                        }

                        break;
                }
            }

            foreach (var temp in usingSet)
            {
                usings = $"{usings}using {temp};\n";
            }
        }
    }
}
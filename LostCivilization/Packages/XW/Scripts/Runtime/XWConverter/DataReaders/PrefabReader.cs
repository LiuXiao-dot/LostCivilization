#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace XWConverter
{
    /// <summary>
    /// Unity的Prefab文件解析,生成DictionaryILData.
    /// </summary>
    public class PrefabReader : ADataReader
    {
        public const string PrefabName = "PREFABNAME";
        private DictionaryILData _ilData;

        /// <summary>
        /// 遍历prefab的每个层级，并生成DictionaryILData
        /// </summary>
        /// <param name="prefab"></param>
        public AILData Read(GameObject prefab)
        {
            _ilData = new DictionaryILData();
            _ilData.Add(PrefabName, prefab.name);
            var childs = prefab.transform.childCount;
            for (int i = 0; i < childs; i++)
            {
                CheckChilds(prefab.transform.GetChild(i));
            }
            return _ilData;
        }

        /// <summary>
        /// 遍历结束后会将所有Component加入DictionaryILData中
        /// </summary>
        /// <param name="target"></param>
        /// <param name="parentPath"></param>
        private void CheckChilds(Transform target, string parentPath = "")
        {
            var allComponents = GetAllComponents(target);
            var targetPath = parentPath != "" ? $"{parentPath}/{target.name}" : target.name;
            if (allComponents != null)
                foreach (var temp in allComponents)
                {
                    _ilData.Add(temp, targetPath);
                }

            var childCount = target.childCount;
            if (childCount == 0) return;
            for (int i = 0; i < childCount; i++)
            {
                CheckChilds(target.GetChild(i), targetPath);
            }
        }

        /// <summary>
        /// 以第一个“-”为分割点，前面部分为组件，后面部分为名字，组件间使用&分隔
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string[] GetAllComponents(Transform target)
        {
            var name = target.name;
            var splits = name.Split('_');
            if (splits.Length <= 1) return null;
            var components = splits[0];
            return components.Split('|');
        }

        public override AILData Read(string path)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return prefab ? Read(prefab) : null;
        }
    }
}
#endif
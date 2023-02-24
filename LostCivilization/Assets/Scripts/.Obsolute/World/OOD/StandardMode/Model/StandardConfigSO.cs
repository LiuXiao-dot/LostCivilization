using Sirenix.OdinInspector;
using XWDataStructure;
#if UNITY_EDITOR
using UnityEditor;
using XWTool;
#endif
using XWUtility;
namespace LostCivilization.World
{
    /// <summary>
    /// 标准模式配置数据
    /// </summary>
    [XWFilePath("Assets/Arts/Mode/StandardConfigSO.asset",XWFilePathAttribute.PathType.Absolute)]
    public class StandardConfigSO : ScriptableObjectSingleton<StandardConfigSO>
    {
        public SerializableDictionary<string,StandardGroupSO> groups;

        #if UNITY_EDITOR
        [Button]
        private void Refresh()
        {
            if(Instance == null) return;
            Instance.groups = new SerializableDictionary<string, StandardGroupSO>();
            
            var groups = EditorUtils.FindAssets<StandardGroupSO>().ToArray();
            foreach (StandardGroupSO group in groups) {
                Instance.groups.Add(group.name,group);
            }
            EditorUtility.SetDirty(Instance);
        }
        #endif
    }
}
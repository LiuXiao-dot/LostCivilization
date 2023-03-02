using System.Globalization;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XWDataStructure;
namespace XWGrid.MarchingCube
{
    /// <summary>
    /// Module的配置数据
    /// </summary>
    public class ModuleConfig : ScriptableObject
    {
        [ReadOnly]public SerializableDictionary<int, Module> modules = new SerializableDictionary<int, Module>();
        
        #if UNITY_EDITOR
        public GameObject fbx;
        [Button]
        public void Refresh()
        {
            if(fbx == null) return;
            var childCount = fbx.transform.childCount;
            for (int i = 0; i < childCount; i++) {
                var temp = fbx.transform.GetChild(i);
                var meshFilter = temp.GetComponent<MeshFilter>();
                var mesh = meshFilter.sharedMesh;
                modules.Add(NameParse(temp.gameObject.name),new Module(mesh));
            }
            EditorUtility.SetDirty(this);
        }

        private int NameParse(string name)
        {
            var splits = name.Split(' ');
            var value = 0;
            foreach (string split in splits){
                foreach (char c in split) {
                    var cInt = CharUnicodeInfo.GetDigitValue(c);
                    value = (value << 1) + cInt;
                }
            }
            Debug.Log($"{name}:{value}");
            return value;
        }
        #endif
    }
}
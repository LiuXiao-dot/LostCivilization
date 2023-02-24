using UnityEditor;
using XWConverter;

namespace XWUIEditor
{
    public class WindowPrefabReader : PrefabReader
    {
        public AILData Read(WindowPrefabSO prefabSo)
        {
            if (prefabSo == null || prefabSo.customWindow == null) return null;
            return this.Read(prefabSo.customWindow);
        }
        
        public override AILData Read(string path)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<WindowPrefabSO>(path);
            return Read(prefab);
        }
    }
}
#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace XWLaba
{
    /// <summary>
    /// 拉霸测试
    /// </summary>
    public class LabaTester : MonoBehaviour
    {
        public LabaView view;
        public GameObject prefab;
        public int targetIndex;

        private void Awake()
        {
            view.Set(new List<ILabaItemModel>()
            {
                new DefaultLabaItemModel(),
                new DefaultLabaItemModel(),
                new DefaultLabaItemModel(),
                new DefaultLabaItemModel(),
                null,
                new DefaultLabaItemModel(),
            },prefab);
        }

        [Button]
        public void Run()
        {
            view.Scroll(targetIndex);
        }
    }
}
#endif
#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace XWLaba
{
    /// <summary>
    /// 多列拉霸测试
    /// </summary>
    public class LabaGroupTest : MonoBehaviour
    {
        public int[] indexs;
        public LabaGroupView view;

        private List<ILabaItemModel>[] _models;
        private void Awake()
        {
            _models = new List<ILabaItemModel>[3]
            {
                new List<ILabaItemModel>()
                {
                    new DefaultLabaItemModel(), new DefaultLabaItemModel(), new DefaultLabaItemModel(), new DefaultLabaItemModel(), null, new DefaultLabaItemModel(),   
                },
                new List<ILabaItemModel>()
                {
                    new DefaultLabaItemModel(), null, new DefaultLabaItemModel(), new DefaultLabaItemModel(), null, new DefaultLabaItemModel(),   
                },
                new List<ILabaItemModel>()
                {
                    null, new DefaultLabaItemModel(), new DefaultLabaItemModel(), null, null, new DefaultLabaItemModel(),   
                }
            };
            view.SetModels(_models);
        }

        [Button]
        public void Scroll()
        {
            view.Scroll(indexs);
        }
    }
}
#endif
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using XWUI;
using XWUtility;
namespace XWLaba
{
    public partial class LabaView
    {
        [Button]
        public void Refresh()
        {
            _group = gameObject.GetOrAddComponent<ControllableGridLayoutGroup>();
            _loopExtension = gameObject.GetOrAddComponent<LoopGridLayoutGroupExtension>();
            _loopExtension.Refresh(LoopGridLayoutGroupExtension.ScrollLoopType.垂直);
            _group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _group.constraintCount = 1;
            _group.padding = new RectOffset(10, 10, 10, 10);
            _group.spacing = new Vector2(10, 10);
            var rectTransform = (RectTransform)transform;
            RectTransformUtils.SetTopStretch(rectTransform);
        }
    }
}
#endif
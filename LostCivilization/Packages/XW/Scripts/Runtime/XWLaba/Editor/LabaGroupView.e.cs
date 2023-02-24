#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using XWUtility;
namespace XWLaba
{
    /// <summary>
    /// 拉霸UI设置
    /// </summary>
    public partial class LabaGroupView
    {
        [SerializeField] private GameObject labaColPrefab;
        [SerializeField] private int colCount = 5;

        [Button]
        public void Refresh()
        {
            if (labaColPrefab == null) {
                XWLogger.Error("需要添加一个拉霸单独一列的prefab");
                return;
            }
            if (_labaViews == null)
                _labaViews = new List<LabaView>();
            if (_animationCurves == null) {
                _animationCurves = new AnimationCurve[colCount];
                for (int i = 0; i < colCount; i++) {
                    _animationCurves[i] = new AnimationCurve();
                    
                }
            }
            else {
                // 复制动画到新数组中
                var tempCurves = new AnimationCurve[colCount];
                for (int i = 0; i < colCount; i++) {
                    if (_animationCurves.Length > i) {
                        tempCurves[i] = _animationCurves[i];
                    } else {
                        tempCurves[i] = new AnimationCurve();
                    }
                }
                _animationCurves = tempCurves;
            }
            
            // 清除原有的子对象
            var childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0 ; i--) {
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            }
            _labaViews.Clear();
            
            for (int i = 0; i < colCount; i++) {
                var temp = GameObject.Instantiate(labaColPrefab, transform);
                _labaViews.Add(temp.GetComponent<LabaView>());
            }
            // 临时添加一个水平方向的列表，刷新子元素位置，再移除掉
            var horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
            GameObject.DestroyImmediate(horizontalLayoutGroup);
        }
    }
}
#endif
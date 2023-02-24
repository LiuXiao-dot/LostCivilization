using System.Collections.Generic;
using UnityEngine;
using XWUtility;
namespace XWLaba
{
    /// <summary>
    /// 拉霸的UI控制
    /// 实现方式：
    /// 1.多个循环列表
    /// 当前支持项:
    /// todo支持项:
    /// 1.每列不同时开始旋转与停止
    /// 2.提前设定旋转到哪一个物品处
    /// 3.支持空物品
    /// 4.动态改变拉霸的行列数
    /// 5.多种旋转效果(非必要)
    /// 6.支持旋转前更改数据的index(循环列表已支持)
    /// 7.配置AnimationCurve
    /// </summary>
    public partial class LabaGroupView : MonoBehaviour
    {
        [SerializeField] private List<LabaView> _labaViews;
        [SerializeField]private GameObject _prefab;
        [SerializeField] private AnimationCurve[] _animationCurves;

        /// <summary>
        /// 设置数据
        /// List<ILabaItemModel>不需要长度相等.
        /// </summary>
        /// <param name="itemModels"></param>
        public void SetModels(List<ILabaItemModel>[] itemModels,GameObject prefab = null)
        {
            if (_labaViews == null)
                _labaViews = new List<LabaView>();
            var length = itemModels.Length;
            if (_labaViews.Count != length) {
                XWLogger.Error($"已有拉霸列数与数据需要的不一样，数据：{length} 实际：{_labaViews.Count}");
                length = Mathf.Min(length, _labaViews.Count);
            }
            _prefab = prefab == null ? _prefab: prefab;
            for (int i = 0; i < length; i++) {
                _labaViews[i].SetCurve(_animationCurves == null ? new AnimationCurve(): _animationCurves[i]);
                _labaViews[i].Set(itemModels[i], _prefab);
            }
        }

        /// <summary>
        /// 开始滚动
        /// </summary>
        /// <param name="indexs">第index行停留在indexs[index]处</param>
        public void Scroll(int[] indexs)
        {
            var length = indexs.Length;
            for (int i = 0; i < length; i++) {
                _labaViews[i].Scroll(indexs[i]);
            }
        }
        
        public void Scroll(int[] indexs,AnimationCurve[] curves)
        {
            var length = indexs.Length;
            for (int i = 0; i < length; i++) {
                _labaViews[i].SetCurve(curves[i]);
                _labaViews[i].Scroll(indexs[i]);
            }
        }
    }
}
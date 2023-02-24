using System;
using System.Collections.Generic;
using UnityEngine;
using XWResource;
using XWUI;
using XWUtility;
namespace XWLaba
{
    /// <summary>
    /// 单个拉霸的UI
    /// </summary>
    public partial class LabaView : MonoBehaviour
    {
        [SerializeField] private ControllableGridLayoutGroup _group;
        [SerializeField] private LoopGridLayoutGroupExtension _loopExtension;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float scrollDuration = 5f;
        [SerializeField] private int rotateSpeed = 4;

        private Transform _transform;
        public Action onScrollComplete;
        /// <summary>
        /// 目标元素的index
        /// </summary>
        private int _targetIndex;

        private List<ILabaItemView> _views;

        /// <summary>
        /// 数据数量
        /// </summary>
        private int _modelsAmount;

        private void Awake()
        {
            _transform = transform; // 提前保存，防止每次调用执行过多逻辑
        }

        /// <summary>
        /// todo:设置数据
        /// </summary>
        public void SetWithoutComponent<T>(List<ILabaItemModel> models) where T : ILabaItemView, new()
        {
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Set(List<ILabaItemModel> models, GameObject prefab)
        {
            if (!prefab.TryGetComponent<ILabaItemView>(out var outResult)) {
                XWLogger.Warning($"{prefab.name}.prefab需要添加一个继承自ILabaItemView的组件进行控制");
                prefab.AddComponent<DefaultLabaItemView>();
            }
            if (prefab.TryGetComponent<LabaView>(out var outResult2)) {
                XWLogger.Error("子对象暂不允许包含labaView");
                return;
            }
            if (_views == null) _views = new List<ILabaItemView>();
            var length = models.Count;
            for (int i = 0; i < length; i++) {
                _views.Add(Instantiate(prefab).GetComponent<ILabaItemView>());
            }
            _modelsAmount = models.Count;
            _loopExtension.Init(-1, prefab, (go, index) =>
            {
                var realIndex = index % _modelsAmount;
                realIndex = realIndex >= 0 ? realIndex : (_modelsAmount + realIndex);
                go.GetComponent<ILabaItemView>().Refresh(models[realIndex]);
            });
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="models"></param>
        /// <param name="prefabPath"></param>
        public void Set(List<ILabaItemModel> models, string prefabPath)
        {
            var prefab = AddressablesPoolManager.LoadGameObjectSync(prefabPath);
            Set(models, prefab);
        }

        public void SetCurve(AnimationCurve curve)
        {
            this._curve = curve;
        }

        /// <summary>
        /// 滚动
        /// </summary>
        public void Scroll(int targetIndex)
        {
            this._targetIndex = targetIndex + _modelsAmount * rotateSpeed; // 为了多旋转一会儿，需要额外增加index
            _loopExtension.DoScroll(this._targetIndex, scrollDuration, _curve);
        }
    }
}
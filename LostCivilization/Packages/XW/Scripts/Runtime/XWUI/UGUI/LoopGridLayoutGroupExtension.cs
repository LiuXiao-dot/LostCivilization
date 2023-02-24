using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using XWUtility;
namespace XWUI
{
    /// <summary>
    /// 循环列表扩展
    /// </summary>
    public partial class LoopGridLayoutGroupExtension : MonoBehaviour
    {
        /// <summary>
        /// 可控布局
        /// </summary>
        [SerializeField]
        private ControllableGridLayoutGroup layoutGroup;
        private RectTransform content;

        /// <summary>
        /// 是否无限循环(忽略cellAmount)
        /// </summary>
        [SerializeField] private bool limited;

        /// <summary>
        /// 是否数据被修改
        /// </summary>
        private bool dirty;

        /// <summary>
        /// 总的子对象数量index到index了会停止
        /// </summary>
        [ShowIf("limited")]
        [SerializeField]
        private int cellAmount = 100;

        private int actualCellCountX;
        private int cellsPerMainAxis;
        private int actualCellCountY;

        /// <summary>
        /// 刷新第i格GameObject
        /// </summary>
        public Action<GameObject, int> onRefresh;

        private void Awake()
        {
            content = (RectTransform)transform;
        }

        /// <summary>
        /// 设置子元素（如果已有，可以不调用这个方法）
        /// </summary>
        public void Init(int cellAmount = -1, GameObject prefab = null, Action<GameObject, int> onRefresh = null)
        {
            if (onRefresh != null)
                this.onRefresh += onRefresh;
            limited = cellAmount > 0;
            content = (RectTransform)transform;
            this.cellAmount = cellAmount;
            if (prefab == null) {
                if (content.childCount == 0) {
                    XWLogger.Error("需要一个子对象或prefab");
                    return;
                }
                prefab = content.GetChild(0).gameObject;
            }

            // 创建足够多个实例
            var x = layoutGroup.GetCellCountX();
            var y = layoutGroup.GetCellCountY();
            var realAmount = layoutGroup.startAxis == GridLayoutGroup.Axis.Horizontal ? x * (y + 4) : (x + 4) * y;
            for (int i = 0; i < realAmount; i++) {
                var newObj = Instantiate(prefab, content);
                newObj.SetActive(true);
                onRefresh?.Invoke(newObj, i);
            }
        }

        /// <summary>
        /// 滚动中
        /// 需要调用该方法滚动列表
        /// </summary>
        /// <param name="scrollPosition"></param>
        public void OnScroll(Vector2 scrollPosition)
        {
            SetDirty();
        }

        public void SetDirty()
        {
            dirty = true;
        }

        private void LateUpdate()
        {
            if (!dirty) return;
            layoutGroup.GetMaxCellCount(out actualCellCountX, out actualCellCountY, out cellsPerMainAxis);
            Loop();
        }

        /// <summary>
        /// 循环逻辑
        /// </summary>
        private void Loop()
        {
            dirty = false;

            // 坐标判断
            var size = layoutGroup.cellSize + layoutGroup.spacing;

            var dif = (content.anchoredPosition - new Vector2((layoutGroup.OffsetX - 2f) * size.x, (layoutGroup.OffsetY + 2f) * size.y)) / size; // 位置差值(个数)

            if (layoutGroup.startAxis == GridLayoutGroup.Axis.Horizontal) {
                // 水平排列
                bool isMoveUp = dif.y > 0;
                int moveCountY = (int)Mathf.Abs(dif.y);

                if (moveCountY > 0)
                    MoveItemsY(isMoveUp, moveCountY);
            } else {
                // 垂直排列
                bool isMoveLeft = dif.x < 0;
                int moveCountX = (int)Math.Abs(dif.x);

                if (moveCountX > 0)
                    MoveItemsY(isMoveLeft, moveCountX);
            }
        }

        /// <summary>
        /// 根据计算值移动对象位置
        /// </summary>
        private void MoveItemsY(bool isMoveUpLeft, int moveCount)
        {
            var actualCount = layoutGroup.startAxis == GridLayoutGroup.Axis.Horizontal ? actualCellCountX : actualCellCountY;
            for (int j = 0; j < moveCount; j++) {
                if (isMoveUpLeft) {
                    // 上滑，对象移动到最下面
                    var lastIndex = GetLastIndex();
                    if (limited && lastIndex + 1 == cellAmount) {
                        return;
                    }
                    for (int i = 0; i < actualCount; i++) {
                        var top = content.GetChild(i);
                        top.SetAsLastSibling();

                        if (limited && lastIndex + 1 == cellAmount) {
                            top.gameObject.SetActive(false);
                            continue;
                        }
                        lastIndex++;
                        GameObject o;
                        (o = top.gameObject).SetActive(true);
                        onRefresh?.Invoke(o, lastIndex);
                    }
                } else {
                    var firstIndex = GetFirstIndex();
                    if (limited && firstIndex == 0) {
                        return;
                    }
                    var count = content.childCount - 1;
                    for (int i = 0; i < actualCount; i++) {
                        var bottom = content.GetChild(count - i);
                        bottom.SetAsFirstSibling();

                        if (limited && firstIndex == 0) {
                            bottom.gameObject.SetActive(false);
                            continue;
                        }
                        firstIndex--;
                        GameObject o;
                        (o = bottom.gameObject).SetActive(true);
                        onRefresh?.Invoke(o, firstIndex);
                    }
                }
                if (layoutGroup.startAxis == GridLayoutGroup.Axis.Horizontal)
                    layoutGroup.OffsetY += isMoveUpLeft ? 1 : -1;
                else
                    layoutGroup.OffsetX += isMoveUpLeft ? -1 : 1;
            }
        }

        /// <summary>
        /// 获取当前第一个数据的Index
        /// </summary>
        /// <returns></returns>
        private int GetFirstIndex()
        {
            if (layoutGroup.startAxis == GridLayoutGroup.Axis.Horizontal) {
                return layoutGroup.OffsetY * actualCellCountX - layoutGroup.OffsetX;
            } else {
                return -layoutGroup.OffsetX * actualCellCountY + layoutGroup.OffsetY;
            }
        }

        /// <summary>
        /// 最后一个index
        /// </summary>
        /// <returns></returns>
        private int GetLastIndex()
        {
            var firstIndex = GetFirstIndex();
            if (!limited) return firstIndex + actualCellCountX * actualCellCountY - 1;
            return Mathf.Min(cellAmount - 1, firstIndex + actualCellCountX * actualCellCountY - 1);
        }

        public Vector3 SetTarget(int offsetY)
        {
            var size = layoutGroup.cellSize + layoutGroup.spacing;
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, size.y * offsetY);
            return content.anchoredPosition;
        }

        public bool GetIsLimited()
        {
            return limited;
        }

        private Action tempScroll;
        
        /// <summary>
        /// 播放旋转动画
        /// </summary>
        public void DoScroll(int offsetY, float duration, AnimationCurve curve)
        {
            var size = layoutGroup.cellSize + layoutGroup.spacing;
            var targetPosition = new Vector2(content.anchoredPosition.x, size.y * offsetY);
            var oldPosition = content.anchoredPosition;
            var time = 0f;
            if (tempScroll != null) {
                CoroutineManager.Instance.StopCoroutine(tempScroll);
            }
            tempScroll = () =>
            {
                time += 0.1f;
                content.anchoredPosition = Vector2.LerpUnclamped(oldPosition, targetPosition, curve.Evaluate(time / duration));
                SetDirty();
            };
            CoroutineManager.Instance.AddCoroutineWaitTime(tempScroll, Mathf.CeilToInt(duration / 0.1f), 0.1f);
        }
    }
}
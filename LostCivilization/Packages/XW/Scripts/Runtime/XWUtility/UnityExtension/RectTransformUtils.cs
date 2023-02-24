using UnityEngine;
namespace XWUtility
{
    /// <summary>
    /// RectTransform的工具方法
    /// </summary>
    public sealed class RectTransformUtils
    {
        public static void SetTopStretch(RectTransform rectTransform)
        {
            rectTransform.pivot = Vector2.up;
            rectTransform.anchorMin = Vector2.up;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}
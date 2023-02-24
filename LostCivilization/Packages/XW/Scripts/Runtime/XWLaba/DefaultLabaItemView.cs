using UnityEngine;
using UnityEngine.UI;
namespace XWLaba
{
    /// <summary>
    /// 默认的拉霸子对象view
    /// </summary>
    public class DefaultLabaItemView : MonoBehaviour,ILabaItemView
    {
        public void Refresh(ILabaItemModel itemModel)
        {
            if (itemModel is DefaultLabaItemModel defaultLabaItemModel) {
                GetComponent<Image>().enabled = true;

                GetComponent<Image>().color = defaultLabaItemModel.color;
            } else {
                GetComponent<Image>().enabled = false;
            }
        }
    }
}
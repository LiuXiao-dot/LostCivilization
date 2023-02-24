using UnityEngine.UI;
using XWUtility;

namespace XWUI
{
    public class TestGridLayoutGroup : GridLayoutGroup
    {
        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            //XWLogger.Log("refresh");
        }
    }
}
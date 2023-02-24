
// 由UnitySourceConvertor生成,请勿手动修改
using XWDataStructure;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class GameWindowView : MonoBehaviour, IPrefabView
    {
        private Transform _transform;
        public Button _exitBTN;
		
        private void Awake()
        {
            _transform = transform;
            
			_exitBTN = _transform.Find("BTN_exit").GetComponent<Button>();
        }
    }
}
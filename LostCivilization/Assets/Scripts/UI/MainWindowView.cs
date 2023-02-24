
// 由UnitySourceConvertor生成,请勿手动修改
using XWDataStructure;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class MainWindowView : MonoBehaviour, IPrefabView
    {
        private Transform _transform;
        public Button _settingBTN;
		public Button _startBTN;
		
        private void Awake()
        {
            _transform = transform;
            
			_settingBTN = _transform.Find("Home/BTN_setting").GetComponent<Button>();
			_startBTN = _transform.Find("Home/BTN_start").GetComponent<Button>();
        }
    }
}
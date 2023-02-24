
// 由UnitySourceConvertor生成,请勿手动修改
using XWDataStructure;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class SettingWindowView : MonoBehaviour, IPrefabView
    {
        private Transform _transform;
        public Button _logoutBTN;
		public Button _backBTN;
		
        private void Awake()
        {
            _transform = transform;
            
			_logoutBTN = _transform.Find("Settings/BTN_logout").GetComponent<Button>();
			_backBTN = _transform.Find("Settings/TopBar/BTN_back").GetComponent<Button>();
        }
    }
}
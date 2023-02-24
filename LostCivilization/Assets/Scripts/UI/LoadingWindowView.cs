
// 由UnitySourceConvertor生成,请勿手动修改
using XWDataStructure;
using UnityEngine;
using XWUI;

namespace GameUI
{
    public class LoadingWindowView : MonoBehaviour, IPrefabView
    {
        private Transform _transform;
        public XWLoading _progressXWLOADING;
		
        private void Awake()
        {
            _transform = transform;
            
			_progressXWLOADING = _transform.Find("XWLOADING_progress").GetComponent<XWLoading>();
        }
    }
}
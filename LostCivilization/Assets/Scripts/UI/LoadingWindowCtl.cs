
using System;
using UnityEngine;
using UnityEngine.Events;
using XWResource;
using XWUI;
using XWUtility;

namespace GameUI
{
    public class LoadingWindowCtl : AWindowCtl
    {
        /// <summary>
        /// 最大进度条增长速度 单位：进度/s
        /// </summary>
        private float _maxSpeed = 2f;
        /// <summary>
        /// 加载完成后的回调
        /// </summary>
        private Action _loadedCallback;
        /// <summary>
        /// 真实的加载进度
        /// </summary>
        private float _realProgress;
        /// <summary>
        /// 假的加载进度
        /// </summary>
        private float _fakeProgress;
        /// <summary>
        /// 进度条管理器
        /// </summary>
        private XWLoading _progress;
        protected override void OnOpen()
        {
            _progress = (this._view as LoadingWindowView)._progressXWLOADING;
            _loadedCallback = (Action)((this._values != null & this._values.Length > 0) ? this._values[0] : null);
            AddressablesPoolManager.Instance.onLoading += OnLoading;
            CoroutineManager.Instance.AddCoroutineEndFrame(RefreshProgress, -1,1);
        }

        private void RefreshProgress()
        {
            _fakeProgress = Mathf.Min((_realProgress - _fakeProgress) / Time.deltaTime, _maxSpeed) * Time.deltaTime + _fakeProgress;
            _progress.Refresh(_fakeProgress);
            if(_fakeProgress == 1)
            {
                // 加载完成
                WindowManager.CloseWindow("LoadingWindow");
                _loadedCallback?.Invoke();
                CoroutineManager.Instance.StopCoroutine(RefreshProgress);
            }
        }

        private void OnLoading(int total, int finished)
        {
            this._realProgress = 1f * finished / total;
        }

        protected override void OnPause()
        {
        }
        
        protected override void OnResume()
        {
        }
        
        protected override void OnClose()
        {
            AddressablesPoolManager.Instance.onLoading -= OnLoading;
        }
    }
}
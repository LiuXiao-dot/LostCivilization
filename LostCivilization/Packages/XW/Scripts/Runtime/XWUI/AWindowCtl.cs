using XWDataStructure;

namespace XWUI
{
    /// <summary>
    /// 窗口控制器抽象类
    /// </summary>
    public abstract class AWindowCtl
    {
        protected IPrefabView _view;
        protected object[] _values;
        public WindowData data;

        public AWindowCtl()
        {
        }

        /// <summary>
        /// 刷新状态：包括sorting order, bg, header等
        /// </summary>
        private void RefreshStatus()
        {
            
        }

        public void InitModel(params object[] values)
        {
            this._values = values;
        }

        public void InitView(IPrefabView view)
        {
            this._view = view;
        }
        
        /// <summary>
        /// 打开
        /// </summary>
        public void Open()
        {
            OnOpen();
        }


        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            OnPause();
        }

        /// <summary>
        /// 恢复
        /// </summary>
        public void Resume()
        {
            OnResume();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            OnClose();
        }

        /// <summary>
        /// 已开启界面的情况下再次打开(只有该界面在同层界面中的最上层会触发)
        /// </summary>
        public virtual void ReOpen()
        {
            
        }
        
        protected abstract void OnOpen();
        protected abstract void OnPause();
        protected abstract void OnResume();
        protected abstract void OnClose();

        public IPrefabView GetView()
        {
            return _view;
        }
    }
}

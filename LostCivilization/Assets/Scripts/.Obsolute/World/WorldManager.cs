#define XW_ENABLE_DOTS
using System;
using System.Collections.Generic;
using UnityEngine;
using XWDataStructure;
namespace LostCivilization.World
{
    /// <summary>
    /// 管理所有世界，为世界分配不同的控制器
    /// </summary>
    public sealed class WorldManager : MonoBehaviour, IManager, IPreloader
    {
        private static WorldManager _instance;
        public static WorldManager Instance
        {
            get {
                return _instance;
            }
        }
        private Transform _worldControllerTrans;
        private Component _tempWorldView;
        private Dictionary<string, Type> _worldControllers;
        private Dictionary<string, Type> _worldViews;

        private void Awake()
        {
            if(_instance != null) return;
            _instance = this;
            _worldControllerTrans = transform.Find("WorldController");
        }
        public void Init()
        {
            _worldControllers = new Dictionary<string, Type>()
            {
                {"Standard",typeof(StandardWorldController)}
            };
            _worldViews = new Dictionary<string, Type>()
            {
                {"Standard",typeof(StandardWorldView)}
            };
        }
        
        public void Load(Action<float> onProgress)
        {
            
        }
        
        /// <summary>
        /// 打开世界
        /// </summary>
        public void EnterWorld(string worldName)
        {
            var worldController = (IWorldController)Activator.CreateInstance(_worldControllers[worldName]);
            GameContext.Instance.currentWorldController = worldController;
            if(_tempWorldView != null) GameObject.Destroy(_tempWorldView);
            _tempWorldView = _worldControllerTrans.gameObject.AddComponent(_worldViews[worldName]);
            
            worldController.Init();
            ((IWorldView)_tempWorldView).Init(worldController);
        }

        /// <summary>
        /// 退出世界
        /// </summary>
        public void ExitWorld()
        {
            if(_tempWorldView != null)
                GameObject.Destroy(_tempWorldView);
            if (GameContext.Instance.currentWorldController != null) {
                GameContext.Instance.currentWorldController.Dispose();
                GameContext.Instance.currentWorldController = null;
            }
        }
    }
}
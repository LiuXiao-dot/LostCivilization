using System;
using UnityEngine;
using XWDataStructure;
using XWInput;
using XWMessageQueue;
using XWResource;
using XWUI;
using XWUtility;
namespace LostCivilization.World
{
    /// <summary>
    /// 标准世界的UI控制
    /// </summary>
    public class StandardWorldView : MonoBehaviour, IGameEventListener<WorldUIMessage>, IWorldView
    {
        private StandardWorldController _controller;
        private StandardWorldContext _context;

        private StandardCharacterSpawnView _spawnView;
        private StandardOperateView _operateView;

        private void Awake()
        {
            _spawnView = gameObject.GetOrAddComponent<StandardCharacterSpawnView>();
            _operateView = gameObject.GetOrAddComponent<StandardOperateView>();
            WorldUIMessageQueue.SubscribeS(this,
                WorldUIMessage.Init,
                WorldUIMessage.Pause,
                WorldUIMessage.Resume,
                WorldUIMessage.Run,
                WorldUIMessage.Stop,
                WorldUIMessage.RefreshUI,
                WorldUIMessage.NextWave);
        }

        public void Init(IWorldController controller)
        {
            this._controller = (StandardWorldController)controller;
            _context = (StandardWorldContext)_controller.GetContext();
        }

        public void OnMessage(GameEvent<WorldUIMessage> inGameEvent)
        {
            switch (inGameEvent.operate) {

                case WorldUIMessage.Init:
                    AddressablesPoolManager.StartLoadingRecord();

                    // 加载场景
                    AddressablesPoolManager.InstantiateGameObject("TERRAIN_standard.prefab", transform);

                    // 加载主堡
                    AddressablesPoolManager.InstantiateGameObject("CASTLE_standard.prefab", transform);

                    // todo:先加载角色列表,再根据角色列表加载所有组
                    // 加载角色组配置
                    AddressablesPoolManager.LoadScriptableObject<StandardConfigSO>("StandardConfigSO.asset");

                    Action loadedCallback = () =>
                    {
                        WindowManager.OpenWindow("GameWindow");
                        WorldMessageQueue.SendGameEventS(WorldMessage.Run);
                    };
                    WindowManager.OpenWindow("LoadingWindow", loadedCallback);
                    AddressablesPoolManager.StopLoadingRecord();
                    break;
                case WorldUIMessage.Run:
                    // 此时预载资源完成，场景主堡已经创建
                    InputManager.GetInputReader().EnableGameplayInput();
                    break;
                case WorldUIMessage.Pause:
                    enabled = false;
                    break;
                case WorldUIMessage.Resume:
                    enabled = true;
                    WorldMessageQueue.SendGameEventS(WorldMessage.Update);
                    break;
                case WorldUIMessage.Stop:
                    break;
                case WorldUIMessage.NextWave:
                    break;
                case WorldUIMessage.RefreshUI:
                    // 同步角色信息
                    _spawnView.Check(_context);
                    _spawnView.Sync(_context);
                    // 同步资源信息
                    // 
                    if (enabled)
                        WorldMessageQueue.SendGameEventS(WorldMessage.Update);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            WorldUIMessageQueue.Instance.Callback();
        }

        private void OnDestroy()
        {
            WorldUIMessageQueue.UnSubscribeS(this,
                WorldUIMessage.Init,
                WorldUIMessage.Pause,
                WorldUIMessage.Resume,
                WorldUIMessage.Run,
                WorldUIMessage.Stop,
                WorldUIMessage.RefreshUI,
                WorldUIMessage.NextWave);
            // 销毁所有子对象
            var childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--) {
                AddressablesPoolManager.DestroyPool(transform.GetChild(i).gameObject);
            }
            Destroy(_spawnView);
            Destroy(_operateView);
        }
    }
}
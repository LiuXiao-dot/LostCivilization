using Sirenix.OdinInspector;
using UnityEngine;
using XWInput;
using XWMessageQueue;
namespace LostCivilization.World
{
    /// <summary>
    /// 标准模式的一些玩家操作
    /// </summary>
    public class StandardOperateView : MonoBehaviour,IGameEventListener<WorldMessage>
    {
        #if UNITY_EDITOR
        public Vector3 spawnPosition = new Vector3(1,0,0);
                
        /// <summary>
        /// 生成角色
        /// </summary>
        [Button]
        private void SpawCharacter()
        {
            WorldMessageQueue.SendGameEventS(WorldMessage.SpawnCharacter, "GROUP_1", spawnPosition);
        }
        #endif

        /// <summary>
        /// 选择的角色组
        /// </summary>
        private string selectGroup = "GROUP_1";
        
        private void Awake()
        {
            var inputReader = InputManager.GetInputReader();
            inputReader.EnableGameplayInput();
            inputReader.onClick_GAME += OnClick;
            WorldMessageQueue.SubscribeS(this, WorldMessage.DestroyCharacter);
        }

        /// <summary>
        /// 设置当前选择的角色组
        /// </summary>
        public void SetSelectGroup(string selectGroup)
        {
            this.selectGroup = selectGroup;
        }
        
        /// <summary>
        /// 部署己方士兵(只负责部署，部署哪个需要外部设置)
        /// </summary>
        /// <param name="arg0"></param>
        private void OnClick(Vector3 arg0)
        {
            WorldMessageQueue.SendGameEventS(WorldMessage.SpawnCharacter, selectGroup, arg0);
        }

        [Button]
        private void DeleteRandomEnemy()
        {
            WorldMessageQueue.SendGameEventS(WorldMessage.DestroyCharacter);
        }

        [Button]
        private void Pause()
        {
            WorldUIMessageQueue.SendGameEventS(WorldUIMessage.Pause);
        }
        
        [Button]
        private void Resume()
        {
            WorldUIMessageQueue.SendGameEventS(WorldUIMessage.Resume);
        }
        public void OnMessage(GameEvent<WorldMessage> inGameEvent)
        {
            switch (inGameEvent.operate) {
                case WorldMessage.DestroyCharacter:
                    var controller = (StandardWorldController)GameContext.Instance.currentWorldController;
                    var context = (StandardWorldContext)controller.GetContext();
                    var enemys = context.curCharacters[1];
                    if (enemys.Count == 0) break;
                    var v = context.random.Roll(0,enemys.Count);
                    var deleted = enemys[v];
                    enemys.RemoveAt(v);
                    context.deathCharacters[1].Add(deleted);
                    break;
                default:
                    break;
            }
            WorldMessageQueue.Instance.Callback();
        }

        private void OnDestroy()
        {
            var inputReader = InputManager.GetInputReader();
            inputReader.DisableGameplayInput();
            inputReader.onClick_GAME -= OnClick;
        }
    }
}
using UnityEngine;
using UnityEngine.Profiling;
using XWDataStructure;
using XWMessageQueue;
#if !XW_ENABLE_DOTS
namespace LostCivilization.World
{
    /// <summary>
    /// 标准世界控制器
    /// 1.世界中心创建主堡
    /// 2.标准关卡控制器提供关卡参数
    /// 3.标准敌人生成器根据关卡参数生成敌人
    /// 4.标准世界事件控制器根据世界中的事件控制关卡切换，战斗结束等关卡流程性的内容
    /// 5.扩展控制器根据世界逻辑提供分数等额外记录信息
    /// 6.标准玩家控制器：购买士兵，部署士兵等
    /// tip:暂停期间,玩家可以操作,但不执行主逻辑
    /// </summary>
    public class StandardWorldController : AWorldController
    {
        private StandardWorldContext contex;
        private AStandardController[] _controllers;
        private int controllerCount;
        
        protected override AWorldContext GetInitContext()
        {
            contex = new StandardWorldContext((StandardWorldModel)_model)
            {
                state = 0,
                wave = 1,
                degree = 1
            };
            return contex;
        }

        protected override IWorldModel GetInitModel()
        {
            return new StandardWorldModel();
        }

        protected override void InitControllers()
        {
            if (World2D.Instance == null) World2D.Instance = new World2D(40,20);
            WorldMessageQueue.SubscribeS(this, WorldMessage.SpawnCharacter);
            _controllers = new AStandardController[]
            {
                new StandardWaveController(contex) , // 关卡切换判断
                new StandardBuildingController(contex) , // 建筑相关逻辑
                new StandardCharacterController(contex) ,// 敌人生成计算
                new StandardMainController(contex) , // 主逻辑
                new StandardStoreController(contex) , // 购买士兵相关逻辑
                new StandardStateController(contex) , // 世界状态判断
            };
            controllerCount = _controllers.Length;
        }

        /// <summary>
        /// 每次执行后根据上下文信息刷新UI
        /// tip:不继承父类，减少GC
        /// </summary>
        public override void Act()
        {
            for (int i = 0; i < controllerCount; i++) {
                _controllers[i].Act();
            }
        }

        public override void OnMessage(GameEvent<WorldMessage> inGameEvent)
        {
            switch (inGameEvent.operate) {
                case WorldMessage.SpawnCharacter:
                    var newGroup = StandardFactory.Instance.CreateGroup((string)inGameEvent.args[0], (Vector3)inGameEvent.args[1], 0);
                    contex.tempNewCharacters[0].Add(newGroup);
                    newGroup.SetRotation(Quaternion.AngleAxis(contex.random.Roll(0,360),Vector3.up));
                    break;
                default:
                    break;
            }
            base.OnMessage(inGameEvent);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing) {
                WorldMessageQueue.Instance.UnSubscribe(this, WorldMessage.SpawnCharacter);
            }
            World2D.Instance.Clear();
        }
    }
}
#endif
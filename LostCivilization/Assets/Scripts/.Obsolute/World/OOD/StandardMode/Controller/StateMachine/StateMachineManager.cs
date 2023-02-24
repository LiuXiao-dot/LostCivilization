using XWDataStructure;
namespace LostCivilization.World
{
    /// <summary>
    /// 状态机管理
    /// </summary>
    public class StateMachineManager : IManager
    {
        private static StateMachineManager _instace;
        private IState<StandardCharacter>[] chaStates;
        public void Init()
        {
            _instace = this;
        }
        
        /// <summary>
        /// 初始化角色状态机
        /// </summary>
        public static void InitCharacterStateMachine()
        {
            var temp = new IState<StandardCharacter>[]
            {
                new MoveState(),
                new NormalAttackState()
            };
            _instace.chaStates = temp;
        }
        
    }
}
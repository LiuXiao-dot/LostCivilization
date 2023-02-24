namespace LostCivilization.World
{
    /// <summary>
    /// 角色状态机
    /// </summary>
    public abstract class ACharacterState : IState<StandardCharacter>
    {
        /// <summary>
        /// 更新角色状态
        /// </summary>
        /// <param name="t"></param>
        public abstract void Update(StandardCharacter t);
    }
}
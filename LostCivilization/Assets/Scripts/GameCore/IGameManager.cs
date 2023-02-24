namespace LostCivilization
{
    /// <summary>
    /// 每个玩法有一个GameManager
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// 进入世界
        /// </summary>
        void Enter();
        void Pause();
        void Resume();
        void Exit();
        void Save();
        void Load();
    }
}
namespace LostCivilization
{
    /// <summary>
    /// 游戏中的一些重要数据在游戏中临时存储(上下文数据，不会存档)
    /// </summary>
    public sealed class GameContext
    {
        public static GameContext Instance = new GameContext();
        /// <summary>
        /// 当前世界控制器
        /// </summary>
        //public IWorldController currentWorldController;
    }
}
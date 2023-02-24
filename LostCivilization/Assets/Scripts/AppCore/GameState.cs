namespace LostCivilization
{
    /// <summary>
    /// 游戏当前状态
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// 游戏外
        /// </summary>
        OutGame = 0,
        /// <summary>
        /// 游戏进行中
        /// </summary>
        InGameRunning = 1,
        /// <summary>
        /// 游戏暂停中
        /// </summary>
        InGamePause = 2,
    }
}
namespace LostCivilization
{
    /// <summary>
    /// 游戏中的一些基础配置信息
    /// 使用PlayerPrefs存档，只支持string,float,int
    /// </summary>
    public sealed class GameConfig
    {
        public static GameConfig Instance = new GameConfig();
        /// <summary>
        /// 不是第一次进入游戏
        /// </summary>
        public bool notFirstEnterGame => enterGameTime != 0;

        public int enterGameTime = 0;
    }
}
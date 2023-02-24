using System;
using XWDataStructure;

namespace XWDataStructure
{
    /// <summary>
    /// 游戏全部可缓存数据(该类会自动保存数据，需要自行保存的数据请勿使用)
    /// (该类默认并不包含任何实际数据，需要各个模块自行注册)
    /// </summary>
    [Serializable]
    public class AppModel : AModel
    {
        public static AppModel Instance { get; private set; }

        public static void InitGameModel(AppModel model = null)
        {
            Instance = model ?? new AppModel();
        }

        public static AppModel GetCurModel()
        {
            return Instance;
        }
    }
}
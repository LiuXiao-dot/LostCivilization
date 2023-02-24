#if !XW_ENABLE_DOTS
using System;
using XWDataStructure;

namespace LostCivilization.World
{
    /// <summary>
    /// 世界接口
    /// </summary>
    public interface IWorldController : IController,IDisposable
    {
        /// <summary>
        /// 初始化世界
        /// </summary>
        void Init(IWorldModel worldModel = null);

        /// <summary>
        /// 开始运行世界
        /// </summary>
        void Run();

        /// <summary>
        /// 停止世界（会造成游戏胜利/结束/存档）
        /// </summary>
        void Stop();

        /// <summary>
        /// 获取世界数据
        /// </summary>
        /// <returns></returns>
        IWorldModel GetModel();
    }
}
#endif
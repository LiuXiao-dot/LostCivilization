using UnityEngine;

namespace LostCivilization.World
{
    /// <summary>
    /// 世界的基础上下文
    /// </summary>
    public abstract class AWorldContext
    {
        /// <summary>
        /// 世界的状态
        /// 默认：
        /// 0:未运行
        /// 1:运行中
        /// </summary>
        public int state;
    }
}
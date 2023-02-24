using XWMessageQueue;
namespace LostCivilization.World
{
    /// <summary>
    /// 世界逻辑执行消息
    /// </summary>
    [MessageQueue(isMainThread = false)]
    public enum WorldMessage
    {
        /// <summary>
        /// 停止世界
        /// </summary>
        Stop,
        /// <summary>
        /// 开始运行
        /// </summary>
        Run,
        /// <summary>
        /// 执行一帧
        /// </summary>
        Update,
        /// <summary>
        /// 召唤角色
        /// </summary>
        SpawnCharacter,
        /// <summary>
        /// 销毁角色
        /// </summary>
        DestroyCharacter,
    }
}
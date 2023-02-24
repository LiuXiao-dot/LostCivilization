using XWMessageQueue;
namespace LostCivilization.World
{
    /// <summary>
    /// 世界中的消息,由WorldSystem所在线程发送给主线程执行UI表现。
    /// 存在UI时，每执行一次逻辑，需要等待UI通知进行下一次逻辑。
    /// </summary>
    [MessageQueue(isMainThread = true)]
    public enum WorldUIMessage
    {
        /// <summary>
        /// 初始化世界
        /// </summary>
        Init,
        /// <summary>
        /// 运行世界
        /// </summary>
        Run,
        /// <summary>
        /// 暂停世界
        /// </summary>
        Pause,
        /// <summary>
        /// 恢复世界
        /// </summary>
        Resume,
        /// <summary>
        /// 停止世界
        /// </summary>
        Stop,
        /// <summary>
        /// 进入下一波
        /// </summary>
        NextWave,
        /// <summary>
        /// 刷新UI
        /// </summary>
        RefreshUI,
        
    }
}
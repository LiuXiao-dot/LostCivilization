using System;

namespace XWMessageQueue
{
    /// <summary>
    /// 游戏事件监听者
    /// </summary>
    public interface IGameEventListener<T>
    {
        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="inGameEvent">消息数据</param>
        /// <param name="callback">执行完毕，需要调用callback，否则会卡在当前事件</param>
        void OnMessage(GameEvent<T> inGameEvent);
    }
}

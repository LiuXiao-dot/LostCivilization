using System;

namespace XWMessageQueue
{
    /// <summary>
    /// 消息队列的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public class MessageQueueAttribute : Attribute
    {
        public bool isMainThread;
        public MessageQueueAttribute(bool isMainThread = true)
        {
            this.isMainThread = isMainThread;
        }
    }
}
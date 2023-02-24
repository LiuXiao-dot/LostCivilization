namespace XWMessageQueue
{
    /// <summary>
    /// 事件基类
    /// </summary>
    public class GameEvent<T>
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public T operate;

        /// <summary>
        /// 参数
        /// </summary>
        public object[] args;

        public GameEvent()
        {

        }

        public GameEvent(T operate, params object[] arg)
        {
            this.operate = operate;
            this.args = arg;
        }
    }
}

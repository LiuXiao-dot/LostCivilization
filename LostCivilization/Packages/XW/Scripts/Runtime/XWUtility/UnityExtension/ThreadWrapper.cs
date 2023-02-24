using System;
using System.Threading;

namespace XWUtility
{
    /// <summary>
    /// 线程封装
    /// </summary>
    public class ThreadWrapper
    {
        private Action action;
        private ManualResetEvent resetEvent;

        public ThreadWrapper()
        {
            resetEvent = new ManualResetEvent(false);
        }

        public void Run()
        {
            action?.Invoke();
        }

        /// <summary>
        /// 加入新的事件
        /// </summary>
        /// <param name="newAction"></param>
        public void Join(Action newAction)
        {
            action += newAction;
        }

        /// <summary>
        /// 不重复添加已加的事件
        /// </summary>
        public void JoinCover(Action newAction)
        {
            action -= newAction;
            action += newAction;
        }

        public void Pause()
        {
            resetEvent.Reset();
            resetEvent.WaitOne();
        }

        public void Resume()
        {
            resetEvent.Set();
        }
    }
}

using System.Threading;
using UnityEngine.Profiling;
using XWUtility;
using ThreadPool = XWUtility.ThreadPool;

namespace XWMessageQueue
{
    /// <summary>
    /// 子线程中运行 线程启动时循环执行.当没有消息存在时，线程将进入休眠状态。
    /// </summary>
    public abstract class AChildThreadMessageQueue<T> : BaseMessageQueue<T>
    {
        private Thread thread;
        private ThreadWrapper threadWrapper;
        private void Awake()
        {
            thread = ThreadPool.Instance.Obtain();
            threadWrapper = ThreadPool.Instance.GetWrapper(thread);
            threadWrapper.JoinCover(Run);
        }

        protected override void SendGameEvent(GameEvent<T> newInGameEvent)
        {
            base.SendGameEvent(newInGameEvent);
            if(thread.ThreadState == ThreadState.Unstarted)
            {
                thread.Start();
            }
            threadWrapper.Resume();
        }

        private void Run()
        {
            while(true)
            {
                if ((eventCount | taskCount) == zero)
                {
                    threadWrapper.Pause();
                    continue;
                }
                Act();
            }
        }
    }
}

using UnityEngine.Profiling;
namespace XWMessageQueue
{
    /// <summary>
    /// 主线程中运行 Update时执行Act 无任务时会停用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AMainThreadMessageQueue<T> : BaseMessageQueue<T>
    {
        private void Update()
        {
            if ((eventCount | taskCount) == zero) {
                return;
            }
            Act();
        }
    }
}
using UnityEngine.Events;
namespace XWTween
{
    /// <summary>
    /// Tween的消息处理器
    /// </summary>
    public class TweenHandler
    {
        public enum TweenStatus
        {
            /// <summary>
            /// 刚刚开始，未执行过
            /// </summary>
            Start,
            Run,
            Sleep,
            Resum,
            Finish,
        }
        public UnityEvent OnComplete;

        private ITween tween;

        /// <summary>
        /// 向队列添加动画
        /// </summary>
        public void Push(ITween nextTween)
        {
            
        }
    }
}
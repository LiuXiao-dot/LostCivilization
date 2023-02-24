using System;
namespace XWDataStructure
{
    /// <summary>
    /// 状态属性
    /// </summary>
    [Serializable]
    public class StateProperty : AProperty
    {
        /// <summary>
        /// 当前状态(由使用该Property的数据自行定义)
        /// </summary>
        public int state;
        /// <summary>
        /// 当前状态剩余时间
        /// </summary>
        public int time;
        /// <summary>
        /// 当前状态总共持续的时间
        /// </summary>
        public int duration;
        
        public StateProperty(int state = 0, int time = -1, int duration = -1)
        {
            this.state = state;
            this.time = time;
            this.duration = duration;
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="time"></param>
        /// <param name="duration"></param>
        public void SetState(int state, int time = -1, int duration = -1)
        {
            this.state = state;
            this.time = time;
            this.duration = duration;
        }
    }
}
using System;
namespace XWDataStructure
{
    /// <summary>
    /// 移动属性
    /// </summary>
    [Serializable]
    public class MoveProperty : AProperty
    {
        /// <summary>
        /// 最大速度
        /// </summary>
        public int maxSpeed;
        
        /// <summary>
        /// 速度
        /// </summary>
        public int speed;

        /// <summary>
        /// 加速度
        /// </summary>
        public int acceleration;

        public MoveProperty GetCopy()
        {
            return new MoveProperty()
            {
                maxSpeed = maxSpeed,
                speed = speed,
                acceleration = acceleration
            };
        }
    }
}
using System;
using UnityEngine;
namespace XWActor
{
    /// <summary>
    /// Actor基础数据
    /// </summary>
    [Serializable]
    public class ActorBaseProperty : AProperty
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 position;
        /// <summary>
        /// 方向
        /// </summary>
        public Quaternion rotation;

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }

        /// <summary>
        /// 设置角度
        /// </summary>
        /// <param name="rotation"></param>
        public void SetRotation(Quaternion rotation)
        {
            this.rotation = rotation;
        }
    }
}
using UnityEngine;

namespace XWDataStructure
{
    /// <summary>
    /// 形状
    /// </summary>
    public abstract class AActorShape
    {
        public AnRanShapeConfig.ActorShapeType actorShapeType;
        public Vector2 center;
        /// <summary>
        /// 二进制位比对
        /// </summary>
        public ushort triggerIndex = 0b0001;

        public AActorShape(AnRanShapeConfig.ActorShapeType actorShapeType, Vector2 center)
        {
            this.actorShapeType = actorShapeType;
            this.center = center;
        }

        /// <summary>
        /// 单位法线方向
        /// </summary>
        /// <param name="otherShape"></param>
        /// <returns></returns>
        public abstract Vector2 GetNormalize(AActorShape otherShape);

        /// <summary>
        /// 检测碰撞
        /// </summary>
        /// <param name="otherShape"></param>
        /// <returns></returns>
        public abstract bool CheckCollider(AActorShape otherShape);

        /// <summary>
        /// 复制一份数据
        /// </summary>
        /// <returns></returns>
        public abstract AActorShape GetCopy();
    }
}

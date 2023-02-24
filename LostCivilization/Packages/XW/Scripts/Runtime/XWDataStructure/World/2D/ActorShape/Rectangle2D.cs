using UnityEngine;

namespace XWDataStructure
{
    /// <summary>
    /// 矩形
    /// </summary>
    public class Rectangle2D : AActorShape
    {
        public float width;
        public float height;

        public Rectangle2D(Vector2 center, float width, float height) : base(AnRanShapeConfig.ActorShapeType.RECTANGLE, center)
        {
            this.width = width;
            this.height = height;
        }

        public override bool CheckCollider(AActorShape otherShape)
        {
            throw new System.NotImplementedException();
        }
        public override AActorShape GetCopy()
        {
            throw new System.NotImplementedException();
        }

        public override Vector2 GetNormalize(AActorShape otherShape)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 获取四个顶点坐标
        /// </summary>
        /// <param name="isLeft"></param>
        /// <param name="isLow"></param>
        /// <returns></returns>
        public Vector2 GetPoint(bool isLeft, bool isLow)
        {
            var xDif = isLeft ? -0.5f : 0.5f;
            var yDif = isLow ? -0.5f : 0.5f;
            return center + new Vector2(xDif * width, yDif * height);
        }
    }
}

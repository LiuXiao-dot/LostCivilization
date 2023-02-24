using System;
using UnityEngine;

namespace XWDataStructure
{
    /// <summary>
    /// 圆形
    /// </summary>
    [Serializable]
    public class Circle2D : AActorShape
    {
        /// <summary>
        /// 半径
        /// </summary>
        public float radius;

        public Circle2D(Vector2 center, float radius) : base(AnRanShapeConfig.ActorShapeType.CIRCLE, center)
        {
            this.radius = radius;
        }

        public override bool CheckCollider(AActorShape otherShape)
        {
            switch (otherShape.actorShapeType)
            {
                case AnRanShapeConfig.ActorShapeType.POINT:
                    break;
                case AnRanShapeConfig.ActorShapeType.LINE:
                    break;
                case AnRanShapeConfig.ActorShapeType.TRIANGLES:
                    break;
                case AnRanShapeConfig.ActorShapeType.RECTANGLE:
                    return CheckWithRectangle(otherShape as Rectangle2D) != Vector2.zero;
                case AnRanShapeConfig.ActorShapeType.CIRCLE:
                    return CheckWithCircle(otherShape as Circle2D) != Vector2.zero;
                default:
                    break;
            }


            return false;
        }
        public override AActorShape GetCopy()
        {
            return new Circle2D(center,radius);
        }

        public override Vector2 GetNormalize(AActorShape otherShape)
        {
            switch (otherShape.actorShapeType)
            {
                case AnRanShapeConfig.ActorShapeType.POINT:
                    break;
                case AnRanShapeConfig.ActorShapeType.LINE:
                    break;
                case AnRanShapeConfig.ActorShapeType.TRIANGLES:
                case AnRanShapeConfig.ActorShapeType.RECTANGLE:
                    return CheckWithRectangle(otherShape as Rectangle2D);
                case AnRanShapeConfig.ActorShapeType.CIRCLE:
                    return CheckWithCircle(otherShape as Circle2D);
                default:
                    break;
            }


            return Vector2.zero;
        }

        /// <summary>
        /// 检测与矩形的碰撞
        /// </summary>
        /// <param name="rectangle2D"></param>
        /// <returns>返回法线向量</returns>
        private Vector2 CheckWithRectangle(Rectangle2D rectangle2D)
        {
            // 矩形中心到右上角
            var rightUp = rectangle2D.GetPoint(false, false);
            var oToRightUp = rightUp - rectangle2D.center;

            // 圆心投影到第一象限
            var circleCenter = center - rectangle2D.center;
            var oe = new Vector2(Mathf.Abs(circleCenter.x), Mathf.Abs(circleCenter.y));

            // AE
            var ae = oe - oToRightUp;
            ae = new Vector2(Mathf.Max(0, ae.x), Mathf.Max(ae.y, 0));

            if (ae.magnitude > radius)
                return Vector2.zero;

            var aeNoemal = ae.normalized;
            var isXFlip = circleCenter.x < 0 ? -1 : 1;
            var isYFlip = circleCenter.y < 0 ? -1 : 1;

            return (new Vector2(isXFlip * aeNoemal.x, isYFlip * aeNoemal.y)).normalized;
        }

        /// <summary>
        /// 检测与圆形的碰撞
        /// </summary>
        /// <param name="circle2D"></param>
        /// <returns>返回法线向量</returns>
        private Vector2 CheckWithCircle(Circle2D circle2D)
        {
            // 圆心
            var otherCenter = circle2D.center;
            var thisCenter = this.center;
            var otherRadius = circle2D.radius;
            var thisRadius = this.radius;

            var distance = Vector2.Distance(otherCenter,thisCenter);
            if(distance > thisRadius + otherRadius || distance < Mathf.Abs(thisRadius - otherRadius))
                return Vector2.zero;
            if (Mathf.Max(thisRadius, otherRadius) >= distance) {
                // 内部碰撞
                return (otherCenter - thisCenter).normalized;
            }
            // 外部碰撞
            return (thisCenter - otherCenter).normalized;
        }
    }
}

using UnityEngine;

namespace XWDataStructure
{
    /// <summary>
    /// 线段
    /// </summary>
    public class Line2D : AActorShape
    {
        public Line2D(AnRanShapeConfig.ActorShapeType actorShapeType, Vector2 center) : base(actorShapeType, center)
        {
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
    }
}

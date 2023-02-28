using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using XWDataStructure;
namespace XWActor
{
    /// <summary>
    /// 碰撞相关属性(若后续需要一个数据中包含多个碰撞器，需要添加一个TriggerListProperty)
    /// </summary>
    [Serializable]
    public class TriggerProperty : AProperty
    {
        public AActorShape shape;

        public AnRanShapeConfig.ActorShapeType shapeType;
        [Tooltip("Circle:半径 Rect:width")]
        public float size1;
        [Tooltip("Rect:height")]
        [ShowIf("shapeType",AnRanShapeConfig.ActorShapeType.RECTANGLE)]
        public float size2;

        public TriggerProperty GetCopy()
        {
            AActorShape shape = null;
            switch (shapeType) {

                case AnRanShapeConfig.ActorShapeType.POINT:
                    break;
                case AnRanShapeConfig.ActorShapeType.LINE:
                    break;
                case AnRanShapeConfig.ActorShapeType.TRIANGLES:
                    break;
                case AnRanShapeConfig.ActorShapeType.RECTANGLE:
                    break;
                case AnRanShapeConfig.ActorShapeType.CIRCLE:
                    shape = new Circle2D(Vector2.zero, size1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new TriggerProperty()
            {
                shape = shape,
                size1 = size1,
                size2 = size2,
                shapeType = shapeType
            };
        }
        
        #if UNITY_EDITOR
        private IEnumerable<Type> GetShapes()
        {
            var q = typeof(AActorShape).Assembly.GetTypes()
                .Where(x => typeof(AActorShape).IsAssignableFrom(x))
                .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition);                 // Excludes classes not inheriting from BaseClass
            return q;
        }
        #endif
    }
}
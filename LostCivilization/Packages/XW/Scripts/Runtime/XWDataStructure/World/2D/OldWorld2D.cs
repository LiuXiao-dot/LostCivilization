/*using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace XWDataStructure
{
    /// <summary>
    /// 2D的碰撞世界
    /// </summary>
    [Obsolete]
    public class World2D
    {
        public static World2D Instance;
        
        public class TriggerEvent
        {
            public AModel from;
            public AModel to;
            public Vector2 normal;

            public TriggerEvent(AModel from,AModel to, Vector2 normal)
            {
                this.from = from;
                this.to = to;
                this.normal = normal;
            }
            public TriggerEvent()
            {
            }

            public override bool Equals(object obj)
            {
                if (obj is TriggerEvent other) {
                    return (from.Equals(other.from) && to.Equals(other.to)) || (from.Equals(other.to) && to.Equals(other.from));
                }
                return false;
            }

            public override int GetHashCode()
            {
                return from.GetHashCode() ^ to.GetHashCode();
            }
        }

        /// <summary>
        /// 2维碰撞检测的世界
        /// row:x+width * y 表示位置
        /// col:表示同一个位置的shape
        /// </summary>
        private List<AModel>[] actorShapes;
        private int width;
        private int height;
        private int length;

        public UnityAction<TriggerEvent> onTrigger;
        /// <summary>
        /// 每次检测会清空再重新添加
        /// </summary>
        private HashSet<TriggerEvent> _triggers;
        
        /// <summary>
        /// 碰撞检测的偏移量
        /// key:radius
        /// value:偏移
        /// </summary>
        private Dictionary<int, List<int>> checkOffsets = new Dictionary<int, List<int>>();

        private ObjectPools.ObjectPool<TriggerEvent> _objectPool;

        public World2D(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.length = 4 * width * height;
            actorShapes = new List<AModel>[length];
            _triggers = new HashSet<TriggerEvent>();
            _objectPool = new ObjectPools.ObjectPool<TriggerEvent>();
        }

        /// <summary>
        /// 检测所有碰撞器
        /// </summary>
        public void CheckTrigger()
        {
            int i, j, k;
            /*foreach (TriggerEvent temp in _triggers) {
                _objectPool.Release(temp);
            }#1#
            _triggers.Clear();
            for (i = 0; i < length; i++) {
                if (actorShapes[i] == null) continue;

                var shapes = actorShapes[i];
                var count = shapes.Count;
                for (j = 0; j < count; j++) {
                    var shape1 = shapes[j];
                    for (k = j + 1; k < count; k++) {
                        var otherShape = shapes[k];
                        if(shape1.Equals(otherShape)) 
                            continue;
                        var triggerEvent = new TriggerEvent(shape1,otherShape,Vector2.zero);
                        /*var triggerEvent = _objectPool.Get();
                        triggerEvent.from = shape1;
                        triggerEvent.to = otherShape;#1#
                        if(_triggers.Contains(triggerEvent))
                            continue;
                        var normalize = shape1.GetModel<TriggerProperty>().shape.GetNormalize(otherShape.GetModel<TriggerProperty>().shape);
                        if (normalize != Vector2.zero) {
                            triggerEvent.normal = normalize;
                            _triggers.Add(triggerEvent);
                            onTrigger.Invoke(triggerEvent);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 偏移检测 todo:更加精确的判定
        /// </summary>
        private List<int> CheckOffsets(float radius)
        {
            var tempRadius = Mathf.CeilToInt(radius+3);
            if (checkOffsets.ContainsKey(tempRadius)) return checkOffsets[tempRadius];

            var newOffsets = new List<int>();
            for (int i = -tempRadius; i <= tempRadius; i++) {
                for (int j = -tempRadius; j <= tempRadius; j++) {
                    if (i * i + j * j > tempRadius * tempRadius) continue;
                    newOffsets.Add(i + j * width * 2);
                }
            }
            checkOffsets.Add(tempRadius, newOffsets);
            return checkOffsets[tempRadius];
        }

        private int GetIndex(Vector2 center)
        {
            return ((int)center.x + width) + ((int)center.y + height) * width * 2;
        }

        /// <summary>
        /// 添加物体
        /// </summary>
        /// <param name="shape"></param>
        public void AddShape(AModel shape)
        {
            var index = GetIndex(shape.GetModel<TriggerProperty>().shape.center);
            var offsets = CheckOffsets(shape.GetModel<TriggerProperty>().size1);
            var offsetsCount = offsets.Count;
            for (int i = 0; i < offsetsCount; i++) {
                var tempIndex = index + offsets[i];
                if(tempIndex < 0 || tempIndex >= this.length) continue;
                if (actorShapes[tempIndex] == null) actorShapes[tempIndex] = new List<AModel>();
                actorShapes[tempIndex].Add(shape);
            }
        }

        /// <summary>
        /// 移除物体
        /// </summary>
        /// <param name="shape"></param>
        public void RemoveShape(AModel shape)
        {
            var index = GetIndex(shape.GetModel<TriggerProperty>().shape.center);
            var offsets = CheckOffsets(shape.GetModel<TriggerProperty>().size1);
            var offsetsCount = offsets.Count;
            for (int i = 0; i < offsetsCount; i++) {
                var tempIndex = index + offsets[i];
                if(tempIndex < 0 || tempIndex >= this.length) continue;
                actorShapes[tempIndex].Remove(shape);
            }
        }

        /// <summary>
        /// 世界中的物体只能使用该方法设置坐标
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="to"></param>
        public void MoveShape(AModel shape, Vector2 to)
        {
            var oldIndex = GetIndex(shape.GetModel<TriggerProperty>().shape.center);
            if (oldIndex < 0 || oldIndex >= length) return;
            if (actorShapes[oldIndex] == null || !actorShapes[oldIndex].Contains(shape)) {
                shape.GetModel<TriggerProperty>().shape.center = to;
                AddShape(shape);
                return;
            }
            var newIndex = GetIndex(to);
            if (oldIndex == newIndex){
                return;
            }
            if (newIndex - oldIndex > 1) {
                ;
            }
            RemoveShape(shape);
            shape.GetModel<TriggerProperty>().shape.center = to;
            AddShape(shape);
        }

        public void Clear()
        {
            for (int i = 0; i < length; i++) {
                if (actorShapes[i] == null) continue;
                actorShapes[i].Clear();
            }
            checkOffsets.Clear();
        }
    }
}*/
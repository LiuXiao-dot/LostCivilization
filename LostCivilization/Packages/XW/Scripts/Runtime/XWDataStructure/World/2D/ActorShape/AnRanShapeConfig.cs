namespace XWDataStructure
{
    /// <summary>
    /// 形状相关常量等配置
    /// </summary>
    public class AnRanShapeConfig
    {
        /// <summary>
        /// 形状
        /// </summary>
        public enum ActorShapeType : int
        {
            NONE = 0,
            POINT = 1, // 点
            LINE = 2, // 线段
            TRIANGLES = 3, // 三角形
            RECTANGLE = 4, // 矩形
            CIRCLE = 5, // 圆形
        }
        
        /// <summary>
        /// 碰撞类型
        /// </summary>
        public enum TriggerType
        {
            /// <summary>
            /// 不启用
            /// </summary>
            Disable,
            /// <summary>
            /// 触发(只有触发的一方可以触发，另一方为碰撞时不触发另一方)
            /// </summary>
            Trigger,
            /// <summary>
            /// 碰撞(只有两个都为碰撞才能发生碰撞)
            /// </summary>
            Collider,
        }
    }
}

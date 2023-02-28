using UnityEngine;
namespace XWGrid.Hexagon
{
    /// <summary>
    /// 顶点数据
    /// </summary>
    public class VertexData
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 position;
        /// <summary>
        /// 顶点标记值
        /// </summary>
        public int value;

        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }
    }
}
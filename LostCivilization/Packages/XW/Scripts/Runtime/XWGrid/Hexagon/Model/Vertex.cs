using Vector3 = UnityEngine.Vector3;
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

        public VertexData(Vector3 position, int value = 0)
        {
            this.position = position;
            this.value = value;
        }
        
        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }
    }
}
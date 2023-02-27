using UnityEngine;
namespace XWGrid.Hexagon
{
    /// <summary>
    /// 六边形顶点的工具方法
    /// </summary>
    public sealed class VertexHexUtils
    {
        /// <summary>
        /// 获取一条边的中心点
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMid(Vector3 a, Vector3 b)
        {
            return (a + b) / 2;
        }
        
        /// <summary>
        /// 获取三角形的中心点
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="list"></param>
        public static Vector3 GetMid(Vector3 a, Vector3 b, Vector3 c)
        {
            return (a + b + c) / 3;
        }
        
        /// <summary>
        /// 获取四边形的中心点
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="list"></param>
        public static Vector3 GetMid(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            return (a + b + c + d) / 4;
        }
    }
}
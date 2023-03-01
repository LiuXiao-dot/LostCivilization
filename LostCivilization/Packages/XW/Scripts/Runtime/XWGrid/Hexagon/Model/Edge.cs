using System;
using UnityEngine;
namespace XWGrid.Hexagon
{
    /// <summary>
    /// 边
    /// </summary>
    public struct Edge : IEquatable<Edge>
    {
        public Vector3 a;
        public Vector3 b;

        private Edge(Vector3 a, Vector3 b)
        {
            this.a = a;
            this.b = b;
        }

        public static Edge CreateEdge(Vector3 a, Vector3 b)
        {
            if (a.Equals(b)) return default(Edge);
            return new Edge(a, b);
        }

        /// <summary>
        /// 获取三角形的第三个顶点
        /// </summary>
        public void GetTriangleVertexs(out Vector3 vertexA, out Vector3 vertexB)
        {
            var dif = a - b; // 差值，三角形为将差值换个位置
            vertexA = new Vector3(a.x + dif.y, a.y + dif.z, a.z + dif.x);
            vertexB = new Vector3(a.x + dif.z, a.y + dif.x, a.z + dif.y);
        }

        public static bool operator ==(Edge a, Edge b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Edge a, Edge b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(a, b);
        }
        public bool Equals(Edge other)
        {
            return (this.a == other.a && this.b == other.b) || (this.a == other.b && this.b == other.a);
        }
        /// <summary>
        /// 会有GC
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is Edge other && Equals(other);
        }
    }
}
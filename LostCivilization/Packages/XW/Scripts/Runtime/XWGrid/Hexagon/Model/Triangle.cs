using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using XWDataStructure;
using XWUtility;
namespace XWGrid.Hexagon
{
    /// <summary>
    /// 三角形
    /// </summary>
    [Serializable]
    public struct Triangle : IEquatable<Triangle>
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        /// <summary>
        /// 单个环的三角形
        /// </summary>
        public static void TrianglesRing(int radius, List<Vector3> vertices, List<Edge> edges, List<Triangle> triangles)
        {
            var baseInnerIndex = radius == 1 ? 0: ((radius - 1) * (radius - 2) * 3 + 1);
            var baseOuterIndex = radius * (radius - 1) * 3 + 1;
            var innerCount = radius == 1 ? 1 : (radius * 6 - 6);
            var outerCount = radius * 6;
            for (int i = 0; i < GridConstant.POLIGON; i++) {
                for (int j = 0; j < radius; j++) {
                    // 创建两个顶点在外圈，一个顶点在内圈的三角形
                    var a = vertices[baseOuterIndex + i * radius + j];
                    var b = vertices[(i * radius + j + 1 ) % outerCount + baseOuterIndex];
                    var c = vertices[(i * (radius - 1) + j) % innerCount + baseInnerIndex];
                    var ab = Edge.CreateEdge(a, b);
                    var bc = Edge.CreateEdge(b, c);
                    var ca = Edge.CreateEdge(c, a);
                    var needAb = !edges.Contains(ab);
                    var needBc = !edges.Contains(bc);
                    var needCa = !edges.Contains(ca);
                    if (needAb)
                        edges.Add(ab);
                    if (needBc)
                        edges.Add(bc);
                    if (needCa)
                        edges.Add(ca);
                    // 创建一个顶点在外圈，两个顶点在内圈的三角形
                    if (j > 0) {
                        var d = vertices[i * (radius - 1) + j - 1 + baseInnerIndex];
                        triangles.Add(new Triangle(a, c, d));
                        var da = Edge.CreateEdge(d, a);
                        var cd = Edge.CreateEdge(c, d);
                        if (!edges.Contains(da))
                            edges.Add(da);
                        if (!edges.Contains(cd))
                            edges.Add(cd);
                    }
                    triangles.Add(new Triangle(a, b, c));
                }
            }
        }

        /// <summary>
        /// 全部环的三角形
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="vertices"></param>
        /// <param name="triangles"></param>
        public static void TrianglesRings(int radius, List<Vector3> vertices, List<Edge> edges, List<Triangle> triangles)
        {
            for (int i = 1; i <= radius; i++) {
                TrianglesRing(i, vertices, edges, triangles);
            }
        }

        /// <summary>
        /// 两个三角面是否相邻，判断两个点相同即可
        /// </summary>
        /// <returns></returns>
        public bool IsNeighbor(Triangle other)
        {
            var sameCount = 0;
            sameCount += a == other.a ? 1 : 0;
            sameCount += a == other.b ? 1 : 0;
            sameCount += a == other.c ? 1 : 0;
            sameCount += b == other.a ? 1 : 0;
            sameCount += b == other.b ? 1 : 0;
            sameCount += b == other.c ? 1 : 0;
            sameCount += c == other.a ? 1 : 0;
            sameCount += c == other.b ? 1 : 0;
            sameCount += c == other.c ? 1 : 0;
            return sameCount == 2;
        }

        /// <summary>
        /// 以确定有相邻边的可直接调用
        /// </summary>
        /// <returns></returns>
        public static Edge FindNeighborEdge(Triangle triangleA, Triangle triangleB, out Vector3 unCommonSelf, out Vector3 unCommonOther)
        {
            // 两个三角形，边顺序肯定相反,a相同时，只可能为ab == triangleB.ac || ac == triangleB.ab
            if (triangleA.a == triangleB.a) {
                if (triangleA.b == triangleB.c) {
                    unCommonSelf = triangleA.c;
                    unCommonOther = triangleB.b;
                    return Edge.CreateEdge(triangleA.a, triangleA.b);
                }
                unCommonSelf = triangleA.b;
                unCommonOther = triangleB.c;
                return Edge.CreateEdge(triangleA.c, triangleA.a); // 顺时针顺序创建
            }
            // a != triangleB.a
            //   b      b    a    b
            // a  c   a   c    c
            //      a   b
            //        c
            if (triangleA.b == triangleB.b) {
                if (triangleA.a == triangleB.c) {
                    unCommonSelf = triangleA.c;
                    unCommonOther = triangleB.a;
                    return Edge.CreateEdge(triangleA.a, triangleA.b);
                }
                unCommonSelf = triangleA.a;
                unCommonOther = triangleB.c;
                return Edge.CreateEdge(triangleA.c, triangleA.a);
            }

            if (triangleA.c == triangleB.c) {
                if (triangleA.b == triangleB.a) {
                    unCommonSelf = triangleA.a;
                    unCommonOther = triangleB.b;
                    return Edge.CreateEdge(triangleA.b, triangleA.c);
                }
                unCommonSelf = triangleA.b;
                unCommonOther = triangleB.a;
                return Edge.CreateEdge(triangleA.c, triangleA.a);
            }
            unCommonOther = default;
            unCommonSelf = default;
            XWLogger.Error("无公共顶点");
            return default;
        }

        /// <summary>
        /// 找出所有相邻三角形
        /// </summary>
        /// <returns></returns>
        public List<Triangle> FindAllNeighbors(List<Triangle> triangles)
        {
            var t = this;
            return triangles.FindAll(temp => t.IsNeighbor(temp));
        }

        public static bool HasNeighborTriangles(List<Edge> edges, List<Triangle> triangles)
        {
            foreach (var triangle in triangles) {
                return edges.Contains(Edge.CreateEdge(triangle.a, triangle.b)) ||
                    edges.Contains(Edge.CreateEdge(triangle.b, triangle.c)) ||
                    edges.Contains(Edge.CreateEdge(triangle.c, triangle.a));
            }
            return false;
        }

        /// <summary>
        /// 合并两个三角形为四边形
        /// </summary>
        public static void MergeNeighborTriangles(Triangle triangleA, Triangle triangleB, List<Quad> quads)
        {
            FindNeighborEdge(triangleA, triangleB, out var quadA, out var quadC);
            var quadB = triangleA.a == quadA ? triangleA.b : (triangleA.b == quadA ? triangleA.c : triangleA.a);
            var quadD = triangleB.a == quadC ? triangleB.b : (triangleB.b == quadC ? triangleB.c : triangleB.a);
            var quad = new Quad(quadA, quadB, quadC, quadD);
            quads.Add(quad);
        }

        public static void RandomMergeTriangles(SerializableRandom random, List<Edge> edges, List<Triangle> triangles, List<Quad> quads)
        {
            var randomIndex = random.Roll(0, edges.Count);
            if (edges.Count == 0) return;
            var commonEdge = edges[randomIndex]; // 公共边
            commonEdge.GetTriangleVertexs(out var vertexA, out var vertexB);
            var triangleA = new Triangle(commonEdge.a, commonEdge.b, vertexA);
            var triangleB = new Triangle(commonEdge.a, commonEdge.b, vertexB);
            var aIndex = triangles.IndexOf(triangleA);
            var bIndex = triangles.IndexOf(triangleB);
            bIndex = bIndex > aIndex ? bIndex - 1 : bIndex;
            edges.RemoveAt(randomIndex);
            if (aIndex >= 0 && bIndex >= 0) {
                triangleA = triangles[aIndex];
                triangles.RemoveAt(aIndex);
                triangleB = triangles[bIndex];
                triangles.RemoveAt(bIndex);
                MergeNeighborTriangles(triangleA, triangleB, quads);
            }
        }

        /// <summary>
        /// 细分为多个四边形
        /// </summary>
        public void Subdivide(List<Quad> subQuads)
        {
            var abMid = VertexHexUtils.GetMid(a, b);
            var bcMid = VertexHexUtils.GetMid(b, c);
            var caMid = VertexHexUtils.GetMid(c, a);
            var center = VertexHexUtils.GetMid(a, b, c);
            var subA = new Quad(a, abMid, center, caMid);
            var subB = new Quad(b, bcMid, center, abMid);
            var subC = new Quad(c, caMid, center, bcMid);
            subQuads.Add(subA);
            subQuads.Add(subB);
            subQuads.Add(subC);
        }

        private static int sameCount = 0;
        public int GetHashCode(Triangle obj)
        {
            return HashCode.Combine(obj.a, obj.b, obj.c);
        }
        public bool Equals(Triangle other)
        {
            sameCount = 0;
            sameCount += a == other.a ? 1 : 0;
            sameCount += a == other.b ? 1 : 0;
            sameCount += a == other.c ? 1 : 0;
            sameCount += b == other.a ? 1 : 0;
            sameCount += b == other.b ? 1 : 0;
            sameCount += b == other.c ? 1 : 0;
            sameCount += c == other.a ? 1 : 0;
            sameCount += c == other.b ? 1 : 0;
            sameCount += c == other.c ? 1 : 0;
            return sameCount == 3;
        }
    }
}
using System;
using System.Collections.Generic;
using XWDataStructure;
using XWUtility;
namespace XWGrid.Hexagon
{
    /// <summary>
    /// 三角形
    /// </summary>
    [Serializable]
    public struct Triangle
    {
        public VertexCenter a;
        public VertexCenter b;
        public VertexCenter c;

        public Triangle(VertexCenter a, VertexCenter b, VertexCenter c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        /// <summary>
        /// 单个环的三角形
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="vertices"></param>
        /// <param name="triangles"></param>
        public static void TrianglesRing(int radius, List<VertexCenter> vertices, HashSet<Edge> edges, List<Triangle> triangles)
        {
            var inner = VertexCenter.GrabRing(radius - 1, vertices);
            var outer = VertexCenter.GrabRing(radius, vertices);
            var innerCount = inner.Count;
            var outerCount = outer.Count;
            for (int i = 0; i < GridConstant.POLIGON; i++) {
                for (int j = 0; j < radius; j++) {
                    // 创建两个顶点在外圈，一个顶点在内圈的三角形
                    var a = outer[i * radius + j];
                    var b = outer[(i * radius + j + 1) % outerCount];
                    var c = inner[(i * (radius - 1) + j) % innerCount];
                    triangles.Add(new Triangle(a, b, c));
                    var ab = Edge.CreateEdge(a, b);
                    var bc = Edge.CreateEdge(b, c);
                    var ca = Edge.CreateEdge(c, a);
                    if (edges.Contains(ab))
                        edges.Add(ab);
                    if (edges.Contains(bc))
                        edges.Add(bc);
                    if (edges.Contains(ca))
                        edges.Add(ca);
                    // 创建一个顶点在外圈，两个顶点在内圈的三角形
                    if (j > 0) {
                        var d = inner[i * (radius - 1) + j - 1];
                        triangles.Add(new Triangle(a, c, d));
                        var da = Edge.CreateEdge(d, a);
                        var cd = Edge.CreateEdge(c, d);
                        if (edges.Contains(da))
                            edges.Add(da);
                        if (edges.Contains(cd))
                            edges.Add(cd);
                    }
                }
            }
        }

        /// <summary>
        /// 全部环的三角形
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="vertices"></param>
        /// <param name="triangles"></param>
        public static void TrianglesRings(int radius, List<VertexCenter> vertices, HashSet<Edge> edges, List<Triangle> triangles)
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
            /*#if UNITY_EDITOR
            if (sameCount == 3) {
                XWLogger.Error("存在重复的三角面");
            }
            #endif*/
            return sameCount == 2;
        }

        /// <summary>
        /// 以确定有相邻边的可直接调用
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Edge FindNeighborEdge(Triangle other, out VertexCenter unCommonSelf, out VertexCenter unCommonOther)
        {
            // 两个三角形，边顺序肯定相反,a相同时，只可能为ab == other.ac || ac == other.ab
            if (a == other.a) {
                if (b == other.c) {
                    unCommonSelf = c;
                    unCommonOther = other.b;
                    return Edge.CreateEdge(a, b);
                }
                unCommonSelf = b;
                unCommonOther = other.c;
                return Edge.CreateEdge(c, a); // 顺时针顺序创建
            }
            // a != other.a
            //   b      b    a    b
            // a  c   a   c    c
            //      a   b
            //        c
            if (b == other.b) {
                if (a == other.c) {
                    unCommonSelf = c;
                    unCommonOther = other.a;
                    return Edge.CreateEdge(a, b);
                }
                unCommonSelf = a;
                unCommonOther = other.c;
                return Edge.CreateEdge(c, a);
            }

            if (c == other.c) {
                if (b == other.a) {
                    unCommonSelf = a;
                    unCommonOther = other.b;
                    return Edge.CreateEdge(b, c);
                }
                unCommonSelf = b;
                unCommonOther = other.a;
                return Edge.CreateEdge(c, a);
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

        public static bool HasNeighborTriangles(List<Triangle> triangles)
        {
            foreach (var from in triangles) {
                foreach (var to in triangles) {
                    if (from.IsNeighbor(to)) return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// 合并两个三角形为四边形
        /// </summary>
        /// <param name="neighbor"></param>
        /// <param name="quads"></param>
        public void MergeNeighborTriangles(Triangle neighbor, HashSet<Edge> edges,List<Triangle> triangles,List<Quad> quads)
        {
            var commonEdge = FindNeighborEdge(neighbor, out var quadA, out var quadC);
            var quadB = this.a == quadA ? this.b : (this.b == quadA ? c : a);
            var quadD = neighbor.a == quadC ? neighbor.b : (neighbor.b == quadC ? neighbor.c : neighbor.a);
            var quad = new Quad(quadA, quadB, quadC, quadD);
            quads.Add(quad);
            edges.Remove(commonEdge);
            triangles.Remove(this);
            triangles.Remove(neighbor);
        }

        public static void RandomMergeTriangles(SerializableRandom random,HashSet<Edge> edges, List<Triangle> triangles, List<Quad> quads)
        {
            var randomIndex = random.Roll(0,triangles.Count);
            var neighbors = triangles[randomIndex].FindAllNeighbors(triangles);
            if(neighbors.Count == 0) return;
            var randomNeighborIndex = random.Roll(0,neighbors.Count);
            triangles[randomIndex].MergeNeighborTriangles(neighbors[randomNeighborIndex],edges,triangles,quads);
        }
    }
}
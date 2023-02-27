using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;
using XWDataStructure;
namespace XWGrid.Hexagon
{
    [Serializable]
    public struct Grid
    {
        public int radius;
        public int cellSize;
        /// <summary>
        /// 六边形顶点
        /// </summary>
        public List<Vector3> hexs;
        /// <summary>
        /// 三角形
        /// </summary>
        public List<Triangle> triangles;
        /// <summary>
        /// 所有边
        /// </summary>
        public List<Edge> edges;
        /// <summary>
        /// 所有四边形
        /// </summary>
        public List<Quad> quads;

        /// <summary>
        /// 网格细分后的四边形
        /// </summary>
        public List<Quad> subQuads;

        /// <summary>
        /// 平滑次数
        /// </summary>
        public int smoothTime;

        private static float sqrt3 = Mathf.Sqrt(3);

        static readonly ProfilerMarker gridCreate = new ProfilerMarker("gridCreate");

        public Grid(int radius, int cellSize, int smoothTime = 5,int seed = 0)
        {
            using (gridCreate.Auto()) {
                this.radius = radius;
                this.cellSize = cellSize;
                hexs = new List<Vector3>(radius * (radius + 1) * 3 + 1);
                triangles = new List<Triangle>(6 * radius * radius);
                edges = new List<Edge>(9 * radius * radius + 3 * radius);
                quads = new List<Quad>(3 * radius * radius);
                var random = new SerializableRandom(seed);
                this.smoothTime = smoothTime;

                CoodinateUtils.Rings(radius, hexs);
                Triangle.TrianglesRings(radius, hexs, edges, triangles);

                var tempEdges = this.edges;
                for (int i = 0; i < 10000; i++) {
                    if (!Triangle.HasNeighborTriangles(tempEdges, triangles)) {
                        break;
                    }
                    Triangle.RandomMergeTriangles(random, tempEdges, triangles, quads);
                }

                subQuads = new List<Quad>(triangles.Count * 3 + quads.Count * 4);
                foreach (Triangle triangle in triangles) {
                    triangle.Subdivide(subQuads);
                }
                foreach (var quad in quads) {
                    quad.Subdivide(subQuads);
                }
                var count = subQuads.Count;

                var difDc = new Dictionary<Vector3, Vector3>(100);
                
                for (int i = 0; i < count; i++) {
                    subQuads[i] = subQuads[i].ChangeToWorld(cellSize);
                    for (int j = 0; j < smoothTime; j++) {
                        subQuads[i].Scroll(difDc);
                    }
                }
                for (int i = 0; i < count; i++) {
                    var a = subQuads[i].a + difDc[ subQuads[i].a];
                    var b = subQuads[i].b + difDc[ subQuads[i].b];
                    var c = subQuads[i].c + difDc[ subQuads[i].c];
                    var d = subQuads[i].d + difDc[ subQuads[i].d];
                    subQuads[i] = new Quad(a,b,c,d);
                }
            }
            Profiler.enabled = false;
        }

        /// <summary>
        /// 顶点朝上计算坐标
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public Vector3 GetPointyCoordinateWorldPosition(Vector3 vector3)
        {
            return new Vector3(cellSize * vector3.x * sqrt3, 0, (-vector3.y - vector3.x / 2f) * 2 * cellSize);
        }

        /// <summary>
        /// 边朝上计算坐标
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public Vector3 GetFlatCoordinateWorldPosition(Vector3 vector3)
        {
            return new Vector3(-(vector3.x * sqrt3 + vector3.y * sqrt3 / 2), 0, vector3.y * 3f / 2) * cellSize;
        }
        
        public static Vector3 GetFlatCoordinateWorldPosition(Vector3 vector3,int cellSize)
        {
            return new Vector3(-(vector3.x * sqrt3 + vector3.y * sqrt3 / 2), 0, vector3.y * 3f / 2) * cellSize;
        }
    }
}
using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;
using XWDataStructure;
using XWGrid.MarchingCube;
namespace XWGrid.Hexagon
{
    [Serializable]
    public struct Grid
    {
        /// <summary>
        /// Grid的半径
        /// </summary>
        public int radius;
        /// <summary>
        /// Grid的高度
        /// </summary>
        public int height;
        /// <summary>
        /// 格子大小
        /// </summary>
        public int cellSize;
        /// <summary>
        /// 格子高度
        /// </summary>
        public int cellHeight;
        /// <summary>
        /// 平滑次数
        /// </summary>
        public int smoothTime;
        public Dictionary<Vector3, VertexData> vertexDatas;
        public List<Cube> cubes;

        private static float sqrt3 = Mathf.Sqrt(3);

        static readonly ProfilerMarker gridCreate = new ProfilerMarker("gridCreate");

        public Grid(int radius, int height, int cellSize, int cellHeight, int smoothTime = 5, int seed = 0)
        {
            using (gridCreate.Auto()) {
                this.radius = radius;
                this.height = height;
                this.cellSize = cellSize;
                this.cellHeight = cellHeight;
                var hexs = new List<Vector3>(radius * (radius + 1) * 3 + 1);
                var triangles = new List<Triangle>(6 * radius * radius);
                var edges = new List<Edge>(9 * radius * radius + 3 * radius);
                var quads = new List<Quad>(3 * radius * radius);
                var random = new SerializableRandom(seed);
                this.smoothTime = smoothTime;

                CoodinateUtils.Rings(radius, hexs);
                Triangle.TrianglesRings(radius, hexs, edges, triangles);

                for (int i = 0; i < 10000; i++) {
                    if (!Triangle.HasNeighborTriangles(edges, triangles)) {
                        break;
                    }
                    Triangle.RandomMergeTriangles(random, edges, triangles, quads);
                }

                var subQuads = new List<Quad>(triangles.Count * 3 + quads.Count * 4);
                foreach (Triangle triangle in triangles) {
                    triangle.Subdivide(subQuads);
                }
                foreach (var quad in quads) {
                    quad.Subdivide(subQuads);
                }

                var count = subQuads.Count;
                this.vertexDatas = new Dictionary<Vector3, VertexData>();
                var tempVertexs = new Dictionary<Vector3, VertexData>(100);
                for (int i = 0; i < count; i++) {
                    subQuads[i] = subQuads[i].ChangeToWorld(cellSize);
                    if (smoothTime == 0) {
                        subQuads[i].UnScroll(tempVertexs);
                    } else {
                        for (int j = 0; j < smoothTime; j++) {
                            subQuads[i].Scroll(tempVertexs);
                        }
                    }
                }
                cubes = new List<Cube>(count * this.height);
                for (int i = 0; i < count; i++) {
                    var a = subQuads[i].a + tempVertexs[subQuads[i].a].position;
                    var b = subQuads[i].b + tempVertexs[subQuads[i].b].position;
                    var c = subQuads[i].c + tempVertexs[subQuads[i].c].position;
                    var d = subQuads[i].d + tempVertexs[subQuads[i].d].position;
                    subQuads[i] = new Quad(a, b, c, d);
                    // 变为3d网格
                    for (int j = 0; j < height; j++) {
                        a = new Vector3(a.x, j * cellHeight, a.z);
                        b = new Vector3(b.x, j * cellHeight, b.z);
                        c = new Vector3(c.x, j * cellHeight, c.z);
                        d = new Vector3(d.x, j * cellHeight, d.z);
                        if (!this.vertexDatas.ContainsKey(a)) {
                            this.vertexDatas.Add(a, new VertexData(a));
                        }
                        if (!this.vertexDatas.ContainsKey(b)) {
                            this.vertexDatas.Add(b, new VertexData(b));
                        }
                        if (!this.vertexDatas.ContainsKey(c)) {
                            this.vertexDatas.Add(c, new VertexData(c));
                        }
                        if (!this.vertexDatas.ContainsKey(d)) {
                            this.vertexDatas.Add(d, new VertexData(d));
                        }
                        if (j > 0) {
                            var k = j - 1;
                            var bottoma = this.vertexDatas[new Vector3(a.x, k * cellHeight, a.z)];
                            var bottomb = this.vertexDatas[new Vector3(b.x, k * cellHeight, b.z)];
                            var bottomc = this.vertexDatas[new Vector3(c.x, k * cellHeight, c.z)];
                            var bottomd = this.vertexDatas[new Vector3(d.x, k * cellHeight, d.z)];
                            var topa = this.vertexDatas[new Vector3(a.x, j * cellHeight, a.z)];
                            var topb = this.vertexDatas[new Vector3(b.x, j * cellHeight, b.z)];
                            var topc = this.vertexDatas[new Vector3(c.x, j * cellHeight, c.z)];
                            var topd = this.vertexDatas[new Vector3(d.x, j * cellHeight, d.z)];
                            cubes.Add(new Cube(bottoma,bottomb,bottomc,bottomd,
                                topa,topb,topc,topd));
                        }
                    }
                }
                tempVertexs.Clear();

            }
            Profiler.enabled = false;
        }

        /// <summary>
        /// 顶点朝上计算坐标
        /// </summary>
        /// <param name="vector3">逻辑坐标</param>
        /// <returns>世界坐标</returns>
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

        public static Vector3 GetFlatCoordinateWorldPosition(Vector3 vector3, int cellSize)
        {
            return new Vector3(-(vector3.x * sqrt3 + vector3.y * sqrt3 / 2), 0, vector3.y * 3f / 2) * cellSize;
        }
    }
}
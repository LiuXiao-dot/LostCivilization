using System;
using System.Collections.Generic;
using UnityEngine;
using XWDataStructure;
using XWUtility;
namespace XWGrid.Hexagon
{
    [Serializable]
    public struct Grid
    {
        public int radius;
        public int cellSize;
        public List<VertexCenter> centers;
        public List<Triangle> triangles;
        public HashSet<Edge> edges;
        public List<Quad> quads;
        private static float sqrt3 = Mathf.Sqrt(3);

        public Grid(int radius, int cellSize, int seed = 0)
        {
            this.radius = radius;
            this.cellSize = cellSize;
            centers = new List<VertexCenter>();
            VertexCenter.Hex(centers, radius);
            triangles = new List<Triangle>();
            edges = new HashSet<Edge>();
            quads = new List<Quad>();
            Triangle.TrianglesRings(radius,centers,edges, triangles);
            var random = new SerializableRandom(seed);

            for (int i = 0; i < 10000; i++) {
                if (!Triangle.HasNeighborTriangles(triangles)) {
                    XWLogger.Log($"循环次数{i}");
                    break;
                }
                Triangle.RandomMergeTriangles(random,edges,triangles,quads);
            }
        }

        /// <summary>
        /// 顶点朝上计算坐标
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public Vector3 GetPointyCoordinateWorldPosition(Coordinate coordinate)
        {
            return new Vector3(cellSize * coordinate.q * sqrt3, 0, (-coordinate.r - coordinate.q / 2f) * 2 * cellSize);
        }
        
        /// <summary>
        /// 边朝上计算坐标
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public Vector3 GetFlatCoordinateWorldPosition(Coordinate coordinate)
        {
            return new Vector3(-(coordinate.q * sqrt3 + coordinate.r * sqrt3 / 2), 0, coordinate.r * 3f / 2) * cellSize;
        }
    }
}
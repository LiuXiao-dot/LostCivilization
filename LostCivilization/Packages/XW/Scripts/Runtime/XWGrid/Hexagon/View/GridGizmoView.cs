using Sirenix.OdinInspector;
using UnityEngine;
using XWGrid.MarchingCube;
namespace XWGrid.Hexagon
{
    ///网格的Gizmo展示
    public class GridGizmoView : MonoBehaviour
    {
        public Vector2Int gridSize;
        public Vector2Int cellSize;
        public Grid grid;
        private int frame;
        public int smoothTime = 5;
        public int seed = 0;
        [Button]
        private void Awake()
        {
            grid = new Grid(gridSize.x, gridSize.y, cellSize.x, cellSize.y, smoothTime, seed);
        }

        private void OnDrawGizmos()
        {
            if (grid.Equals(default) || grid.vertexDatas == null || grid.cubes == null) return;

            foreach (Cube cube in grid.cubes) {
                Gizmos.DrawLine(cube.vertexDatas[0].position,cube.vertexDatas[1].position);
                Gizmos.DrawLine(cube.vertexDatas[1].position,cube.vertexDatas[2].position);
                Gizmos.DrawLine(cube.vertexDatas[2].position,cube.vertexDatas[3].position);
                Gizmos.DrawLine(cube.vertexDatas[3].position,cube.vertexDatas[0].position);
                Gizmos.DrawLine(cube.vertexDatas[0].position,cube.vertexDatas[4].position);
                Gizmos.DrawLine(cube.vertexDatas[1].position,cube.vertexDatas[5].position);
                Gizmos.DrawLine(cube.vertexDatas[2].position,cube.vertexDatas[6].position);
                Gizmos.DrawLine(cube.vertexDatas[3].position,cube.vertexDatas[7].position);
                Gizmos.DrawLine(cube.vertexDatas[4].position,cube.vertexDatas[5].position);
                Gizmos.DrawLine(cube.vertexDatas[5].position,cube.vertexDatas[6].position);
                Gizmos.DrawLine(cube.vertexDatas[6].position,cube.vertexDatas[7].position);
                Gizmos.DrawLine(cube.vertexDatas[7].position,cube.vertexDatas[4].position);
            }
            
            var vertexs = grid.vertexDatas;
            foreach (var vertex in vertexs) {
                Gizmos.color = vertex.Value.value == 0 ? Color.gray: Color.red;
                Gizmos.DrawSphere(vertex.Key,0.1f);
            }
        }
    }
}
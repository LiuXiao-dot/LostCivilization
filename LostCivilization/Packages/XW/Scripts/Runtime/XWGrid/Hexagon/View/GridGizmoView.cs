using Sirenix.OdinInspector;
using UnityEngine;
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

        private void OnDrawGizmosSelected()
        {
            if (grid.Equals(default) || grid.vertexDatas == null) return;

            foreach (var vertexData in grid.vertexDatas) {
                if(vertexData.Value.value == 1)
                    Gizmos.color = Color.green;
                else {
                    Gizmos.color = Color.gray;
                }
                Gizmos.DrawSphere(vertexData.Value.position,0.1f);
            }
        }
    }
}
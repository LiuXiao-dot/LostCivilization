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
            if (grid.Equals(default) || grid.subQuads == null) return;

            var count = grid.subQuads.Count;
            for (int i = 0; i < count; i++) {
                var quad = grid.subQuads[i];
                Gizmos.color = Color.green;
                var a = quad.a;
                var b = quad.b;
                var c = quad.c;
                var d = quad.d;
                Gizmos.DrawLine(a, b);
                Gizmos.DrawLine(b, c);
                Gizmos.DrawLine(c, d);
                Gizmos.DrawLine(d, a);
            }
        }
    }
}
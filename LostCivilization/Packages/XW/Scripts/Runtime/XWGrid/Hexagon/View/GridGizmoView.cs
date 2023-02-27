using Sirenix.OdinInspector;
using UnityEngine;
namespace XWGrid.Hexagon
{
    ///网格的Gizmo展示
    public class GridGizmoView : MonoBehaviour
    {
        public int radius;
        public int cellSize;
        public Grid grid;
        private int frame;
        public int smoothTime = 5;
        public int seed = 0;
        [Button]
        private void Awake()
        {
            grid = new Grid(radius, cellSize,smoothTime, seed);
        }

        private void OnDrawGizmosSelected()
        {
            if(grid.Equals(default) || grid.hexs == null || grid.quads == null) return;
            
            var count = grid.subQuads.Count;
            for (int i = 0; i < count; i++) {
                var quad = grid.subQuads[i];
                Gizmos.color = Color.green;
                var a = quad.a;
                var b = quad.b;
                var c = quad.c;
                var d = quad.d;
                Gizmos.DrawLine(a,b);
                Gizmos.DrawLine(b,c);
                Gizmos.DrawLine(c,d);
                Gizmos.DrawLine(d,a);
            }
        }
    }
}
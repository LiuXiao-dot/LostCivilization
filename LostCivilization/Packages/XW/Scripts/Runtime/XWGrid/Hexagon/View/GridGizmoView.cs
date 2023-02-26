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
        private int index;
        private int frame;
        public int seed = 0;
        [Button]
        private void Awake()
        {
            grid = new Grid(radius, cellSize,seed);
        }

        private void OnDrawGizmosSelected()
        {
            if(grid.Equals(default) || grid.centers == null || grid.quads == null) return;
            frame++;
            if (frame % 60 == 0) {
                index++;
            }

            var count = grid.centers.Count;
            if (index >= count) index = 0;
            for (int i = 0; i < count; i++) {
                if (i > index) {
                    break;
                }
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(grid.GetFlatCoordinateWorldPosition( grid.centers[i].position),0.3f);
            }
            count = grid.triangles.Count;
            for (int i = 0; i < count; i++) {
                var triangle = grid.triangles[i];
                Gizmos.color = Color.red;
                var a = grid.GetFlatCoordinateWorldPosition(triangle.a.position);
                var b = grid.GetFlatCoordinateWorldPosition(triangle.b.position);
                var c = grid.GetFlatCoordinateWorldPosition(triangle.c.position);
                Gizmos.DrawLine(a,b);
                Gizmos.DrawLine(b,c);
                Gizmos.DrawLine(c,a);
            }

            count = grid.quads.Count;
            for (int i = 0; i < count; i++) {
                var quad = grid.quads[i];
                Gizmos.color = Color.green;
                var a = grid.GetFlatCoordinateWorldPosition(quad.a.position);
                var b = grid.GetFlatCoordinateWorldPosition(quad.b.position);
                var c = grid.GetFlatCoordinateWorldPosition(quad.c.position);
                var d = grid.GetFlatCoordinateWorldPosition(quad.d.position);
                Gizmos.DrawLine(a,b);
                Gizmos.DrawLine(b,c);
                Gizmos.DrawLine(c,d);
                Gizmos.DrawLine(d,a);
            }
        }
    }
}
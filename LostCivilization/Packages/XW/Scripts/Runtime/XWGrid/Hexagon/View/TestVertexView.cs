using UnityEngine;
namespace XWGrid.Hexagon
{
    /// <summary>
    /// 顶点测试使用
    /// </summary>
    public class TestVertexView : MonoBehaviour
    {
        public GridGizmoView gridGizmoView;
        public int size;
        public int value = 0;
        
        public void Update()
        {
            var grid = gridGizmoView.grid;
            var position = transform;
            var vertexs = grid.vertexDatas;
            foreach (var vertex in vertexs) {
                if (Vector3.Distance(vertex.Key, transform.position) < size) {
                    vertex.Value.SetValue(value);
                }
            }
        }
    }
}
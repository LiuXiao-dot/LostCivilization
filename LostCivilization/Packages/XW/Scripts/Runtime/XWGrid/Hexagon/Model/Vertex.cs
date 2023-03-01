using System;
using Vector3 = UnityEngine.Vector3;
namespace XWGrid.Hexagon
{
    /// <summary>
    /// 顶点数据
    /// </summary>
    public class VertexData
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 position;
        /// <summary>
        /// 顶点标记值
        /// </summary>
        public int value;

        public Action onValueChanged;
        
        public VertexData(Vector3 position, int value = 0)
        {
            this.position = position;
            this.value = value;
        }

        public void AddValueChangedListener(Action listener)
        {
            onValueChanged += listener;
        }
        
        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }

        public void SetValue(int value)
        {
            if(value == this.value) return;
            this.value = value;
            if(onValueChanged != null) onValueChanged.Invoke();
        }
    }
}
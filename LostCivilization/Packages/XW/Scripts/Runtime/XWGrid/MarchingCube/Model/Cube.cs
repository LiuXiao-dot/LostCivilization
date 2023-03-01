using System;
using UnityEngine;
using XWGrid.Hexagon;
namespace XWGrid.MarchingCube
{
    /// <summary>
    /// MarchingCube中的cube数据
    /// </summary>
    [Serializable]
    public class Cube
    {
        [HideInInspector]public VertexData[] vertexDatas;
        public int value;

        public Cube(params VertexData[] vertexDatas)
        {
            this.vertexDatas = vertexDatas;
            foreach (VertexData vertexData in vertexDatas) {
                vertexData.AddValueChangedListener(Refresh);
            }
            Refresh();
        }

        public void Refresh()
        {
            value = 0;
            for (int i = 0; i < 8; i++) {
                value = (value << 1) + vertexDatas[i].value;
            }
        }
    }
}
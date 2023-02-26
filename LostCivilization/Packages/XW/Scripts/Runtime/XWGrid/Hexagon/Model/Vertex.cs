using System;
using System.Collections.Generic;
namespace XWGrid.Hexagon
{
    /// <summary>
    /// 顶点
    /// </summary>
    public struct Vertex
    {

    }

    [Serializable]
    public struct VertexCenter
    {
        public readonly Coordinate position;
        
        public VertexCenter(Coordinate position)
        {
            this.position = position;
        }

        public static void Hex(List<VertexCenter> vertices, int radius)
        {
            var coordinates = new List<Coordinate>();
            Coordinate.Rings(radius, coordinates);
            var length = coordinates.Count;
            for (int i = 0; i < length; i++) {
                vertices.Add(new VertexCenter(coordinates[i]));
            }
        }

        public static List<VertexCenter> GrabRing(int radius, List<VertexCenter> vertiices)
        {
            if (radius == 0)
                return vertiices.GetRange(0, 1);
            return vertiices.GetRange(radius * (radius - 1) * 3 + 1, radius * 6);
        }

        public override bool Equals(object obj)
        {
            if (obj is VertexCenter vertex) {
                return vertex.position == position;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
        
        public static bool operator ==(VertexCenter a,VertexCenter b)
        {
            return a.Equals(b);
        }
        
        public static bool operator !=(VertexCenter a,VertexCenter b)
        {
            return !a.Equals(b);
        }

    }
}
using UnityEngine;
namespace XWGrid.Hexagon
{
    public sealed class GridConstant
    {
        public const int POLIGON = 6;
        
        public static Vector3[] directions = new[]
        {
            new Vector3(0, 1, -1), new Vector3(-1, 1, 0), new Vector3(-1, 0, 1), new Vector3(0, -1, 1), new Vector3(1, -1, 0), new Vector3(1, 0, -1),
        };
    }
}
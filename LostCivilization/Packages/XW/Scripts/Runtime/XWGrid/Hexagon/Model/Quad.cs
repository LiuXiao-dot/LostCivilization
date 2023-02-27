using System;
using System.Collections.Generic;
using System.Numerics;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
namespace XWGrid.Hexagon
{
    [Serializable]
    public struct Quad
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;
        public Vector3 d;

        public Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        /// <summary>
        /// 细分为多个四边形
        /// </summary>
        public void Subdivide(List<Quad> subQuads)
        {
            var abMid = VertexHexUtils.GetMid(a, b);
            var daMid = VertexHexUtils.GetMid(a, d);
            var bcMid = VertexHexUtils.GetMid(b, c);
            var cdMid = VertexHexUtils.GetMid(c, d);
            var center = VertexHexUtils.GetMid(a, b, c, d);
            var subA = new Quad(a, abMid, center, daMid);
            var subB = new Quad(b, bcMid, center, abMid);
            var subC = new Quad(c, cdMid, center, bcMid);
            var subD = new Quad(d, daMid, center, cdMid);
            subQuads.Add(subA);
            subQuads.Add(subB);
            subQuads.Add(subC);
            subQuads.Add(subD);
        }

        public Quad ChangeToWorld(int cellSize)
        {
            this.a = Grid.GetFlatCoordinateWorldPosition(a,cellSize);
            this.b = Grid.GetFlatCoordinateWorldPosition(b,cellSize);
            this.c = Grid.GetFlatCoordinateWorldPosition(c,cellSize);
            this.d = Grid.GetFlatCoordinateWorldPosition(d,cellSize);
            return new Quad(a,b,c,d);
        }
        
        public void Scroll(Dictionary<Vector3,Vector3> difDic)
        {
            Vector3 center = (a + b + c + d) / 4;
            Vector3 verctorA = (a 
                + Quaternion.AngleAxis(-90,Vector3.up)*(b-center) + center
                + Quaternion.AngleAxis(-180,Vector3.up)*(c-center) + center
            + Quaternion.AngleAxis(-270,Vector3.up)*(d-center) + center)/4;
            var verctorB = Quaternion.AngleAxis(90, Vector3.up) * (verctorA - center) + center;
            var verctorC = Quaternion.AngleAxis(180, Vector3.up) * (verctorA - center) + center;
            var verctorD = Quaternion.AngleAxis(270, Vector3.up) * (verctorA - center) + center;

            difDic.TryAdd(a,Vector3.zero);
            difDic.TryAdd(b,Vector3.zero);
            difDic.TryAdd(c,Vector3.zero);
            difDic.TryAdd(d,Vector3.zero);
            difDic[a] += (verctorA - a) * 0.1f;
            difDic[b] += (verctorB - b) * 0.1f;
            difDic[c] += (verctorC - c) * 0.1f;
            difDic[d] += (verctorD - d) * 0.1f;
        }
    }
}
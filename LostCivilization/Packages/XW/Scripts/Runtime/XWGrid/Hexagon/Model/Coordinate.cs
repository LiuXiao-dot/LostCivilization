using System;
using System.Collections.Generic;
namespace XWGrid.Hexagon
{
    /// <summary>
    /// 坐标
    /// 六边形网格，包含三个轴向
    /// </summary>
    [Serializable]
    public struct Coordinate
    {
        public int q;
        public int r;
        public int s;

        public Coordinate(int q, int r, int s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        public static Coordinate zero = new Coordinate(0, 0, 0);
        private static Coordinate[] directions = new[]
        {
            new Coordinate(0, 1, -1), new Coordinate(-1, 1, 0), new Coordinate(-1, 0, 1), new Coordinate(0, -1, 1), new Coordinate(1, -1, 0), new Coordinate(1, 0, -1),
        };

        /// <summary>
        /// 方向获取
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Coordinate Direction(int direction)
        {
            return Coordinate.directions[direction];
        }

        public Coordinate Neighbor(int direction)
        {
            return directions[direction] + this;
        }

        /// <summary>
        /// 无GC的六边形环
        /// 非编辑器模式下不负责检测result是否为空,默认必需非空才能传入
        /// </summary>
        public static void RingNoAlloc(int radius, List<Coordinate> result)
        {
            #if UNITY_EDITOR
            if (result == null) throw new Exception();
            if (radius < 0) throw new Exception();
            #endif
            //result.Clear();

            if (radius == 0) {
                result.Add(Coordinate.zero);
                return;
            }
            var coord = directions[4] * radius;
            for (int i = 0; i < GridConstant.POLIGON; i++) {
                for (int j = 0; j < radius; j++) {
                    result.Add(coord);
                    coord = coord.Neighbor(i);
                }
            }
        }

        /// <summary>
        /// 构建所有环
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="result"></param>
        public static void Rings(int radius, List<Coordinate> result)
        {
            for (int i = 0; i <= radius; i++) {
                RingNoAlloc(i, result);
            }
        }

        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="muled"></param>
        /// <param name="mul"></param>
        /// <returns></returns>
        public static Coordinate operator *(Coordinate muled, int mul)
        {
            return new Coordinate(muled.q * mul, muled.r * mul, muled.s * mul);
        }

        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="add"></param>
        /// <param name="added"></param>
        /// <returns></returns>
        public static Coordinate operator +(Coordinate added, Coordinate add)
        {
            return new Coordinate(add.q + added.q, add.r + added.r, add.r + added.s);
        }

        public static bool operator ==(Coordinate a,Coordinate b)
        {
            return a.Equals(b);
        }
        
        public static bool operator !=(Coordinate a,Coordinate b)
        {
            return !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Coordinate coordinate) {
                return coordinate.q == q && coordinate.r == r && coordinate.s == s;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return q.GetHashCode() + r.GetHashCode() + s.GetHashCode();
        }
    }
}
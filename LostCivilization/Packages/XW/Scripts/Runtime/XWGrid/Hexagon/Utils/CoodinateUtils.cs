using System;
using System.Collections.Generic;
using UnityEngine;
namespace XWGrid.Hexagon
{
    /// <summary>
    /// 工具方法
    /// </summary>
    public sealed class CoodinateUtils
    {
        /// <summary>
        /// 方向获取
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 Direction(int direction)
        {
            return GridConstant.directions[direction];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Vector3 Neighbor(Vector3 self, int direction)
        {
            return self + GridConstant.directions[direction];
        }

        /// <summary>
        /// 无GC的六边形环
        /// 非编辑器模式下不负责检测result是否为空,默认必需非空才能传入
        /// </summary>
        public static void RingNoAlloc(int radius, List<Vector3> result)
        {
            #if UNITY_EDITOR
            if (result == null) throw new Exception();
            if (radius < 0) throw new Exception();
            #endif

            if (radius == 0) {
                result.Add(Vector3.zero);
                return;
            }
            var coord = GridConstant.directions[4] * radius;
            for (int i = 0; i < GridConstant.POLIGON; i++) {
                for (int j = 0; j < radius; j++) {
                    result.Add(coord);
                    coord = Neighbor(coord, i);
                }
            }
        }

        /// <summary>
        /// 构建所有环
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="result"></param>
        public static void Rings(int radius, List<Vector3> result)
        {
            for (int i = 0; i <= radius; i++) {
                RingNoAlloc(i, result);
            }
        }
    }
}
using UnityEngine;
using XWDataStructure;
namespace LostCivilization.World
{
    /// <summary>
    /// 标准关卡控制器
    /// </summary>
    public class StandardWaveController : AStandardController
    {
        /// <summary>
        /// 2秒 实际值为degree*INTERVAL,最大500
        /// </summary>
        private const int INTERVAL = 100;
        /// <summary>
        /// 关卡切换时间间隔(在上一波最后一次生成后开始计算)
        /// </summary>
        private int _interval;
        private int realInterval;
        public StandardWaveController(StandardWorldContext context) : base(context)
        {
            RefreshInterval();
        }

        private void RefreshInterval()
        {
            realInterval = Mathf.Min(_context.degree, 5) * INTERVAL;
        }

        public override void Act()
        {
            if (_context.degree == _context.enemySpawnTime) {
                _interval++;
                if (_interval == realInterval) {
                    _context.wave++;
                    _context.degree = Calculate(_context.wave);
                    //_context.enemySpawnTime = 0;
                    _context.isWaveChanged = true;
                }

            } else {
                _context.isWaveChanged = false;
                _interval = 0;
            }
        }


        #region 难度计算
        /// <summary>
        /// 特点:
        /// 1.离散
        /// 2.长期平稳增长
        /// 3.偶尔有突变的峰值
        /// 4.偶尔有下降
        /// </summary>
        /// <returns></returns>
        private int Calculate(int wave)
        {
            var tempa = 1;
            var tempb = 9;
            var tempc = 1;
            var value = wave; // 递增
            value = Mathf.FloorToInt(Mathf.Sqrt(wave * tempa)); // 减缓wave较大时的增长趋势
            // 前期峰谷少,后期多,前期峰谷小,后期高
            //tempb = (1 / tempb) + Mathf.Sqrt(tempb);
            tempb = tempb / (int)Mathf.Sqrt(value) + 1;
            tempc = (1 + tempc) * tempc;
            var rectWave = (wave % tempb) == 0 ? ((wave / tempb)%2 == 0 ? 1 : -1) * tempc + value: value; // 方波,添加波峰和波谷
            
            return (int)rectWave + value;
        }
        
        #endregion
    }
}
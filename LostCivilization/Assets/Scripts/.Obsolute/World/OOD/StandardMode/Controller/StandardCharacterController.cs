using UnityEngine;
using XWDataStructure;
using XWResource;
namespace LostCivilization.World
{
    /// <summary>
    /// 敌人生成器:根据当前世界数据生成敌人
    /// 生成规则:
    /// 1.根据波数增加敌人出现的频率
    /// 2.根据波数增加敌人出现的强度
    /// </summary>
    public class StandardCharacterController : AStandardController, ICaculater<StandardWorldContext, int>
    {
        /// <summary>
        /// 敌人出现频率,每执行_checkInterval次逻辑判断一次是否需要生成敌人,按一次调用0.02s计算
        /// </summary>
        private int _checkInterval;

        private int _checkTime = 0;

        public StandardCharacterController(StandardWorldContext context) : base(context)
        {
        }

        /// <summary>
        /// 检测是否该生成敌人
        /// </summary>
        public override void Act()
        {
            // 角色加入/移除战斗
            for (int i = 0; i < 2; i++) {
                var deathGroup = _context.deathCharacters[i];
                if (deathGroup.Count > 0) {
                    var length = deathGroup.Count;
                    for (int j = length - 1; j >= 0; j--) {
                        _context.curCharacters[i].Remove(deathGroup[j]);
                        deathGroup.RemoveAt(j);
                    }
                }

                var tempGroup = _context.newCharacters[i];
                if (tempGroup.Count > 0) {
                    _context.curCharacters[i].AddRange(tempGroup);
                    tempGroup.Clear();
                }
            }

            if (_context.isWaveChanged) {
                // 波数切换
                _checkInterval = 100;
                _context.enemySpawnTime = 0;
            }
            _checkTime++;
            if (_checkTime > _checkInterval) {
                _checkTime = 0;
                var count = Calculate(_context);
                _checkInterval = count * 20;
                _context.enemySpawnTime += count;
                //CreateRandomGroup(count);
            }
            
            // temp->new
            for (int i = 0; i < 2; i++) {
                var tempGroup = _context.tempNewCharacters[i];
                if (tempGroup.Count > 0) {
                    _context.newCharacters[i].AddRange(tempGroup);
                    tempGroup.Clear();
                }
            }
        }

        /// <summary>
        /// 创建随机角色组
        /// </summary>
        private void CreateRandomGroup(int value)
        {
            for (int i = 0; i < value; i++) {
                _context.newCharacters[1].Add(StandardFactory.Instance.CreateGroup($"GROUP_0", CaculatePosition(), 1));
            }
        }

        /// <summary>
        /// 根据上下文信息计算需要生成的敌人数量
        /// todo:修改为返回多个敌人的完整数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Calculate(StandardWorldContext input)
        {
            if (input.degree > input.enemySpawnTime) {
                return _context.random.Roll(1, Mathf.Min(5, input.degree - input.enemySpawnTime));
            }
            // 等待进入下一波才能再计算生成
            return 0;
        }

        /// <summary>
        /// 计算坐标
        /// </summary>
        /// <returns></returns>
        private Vector3 CaculatePosition()
        {
            var radiu = _context.random.Roll(0, 360) * Mathf.PI / 180; // 角度
            return new Vector3(Mathf.Cos(radiu) * 52, 0, Mathf.Sin(radiu) * 52);
        }
    }
}
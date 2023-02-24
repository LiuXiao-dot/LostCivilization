using System;
namespace XWDataStructure
{
    /// <summary>
    /// 战斗相关属性
    /// </summary>
    [Serializable]
    public class BattleBaseProperty : AProperty
    {
        /// <summary>
        /// 生命值
        /// </summary>
        public int health = 1;
        /// <summary>
        /// 攻击力
        /// </summary>
        public int atk = 2;
        /// <summary>
        /// 防御力
        /// </summary>
        public int def = 1;
        /// <summary>
        /// 攻击速度
        /// </summary>
        public int atkSpeed = 1;

        /// <summary>
        /// 复制数据
        /// </summary>
        /// <returns></returns>
        public BattleBaseProperty GetCopy()
        {
            return new BattleBaseProperty()
            {
                health = health,
                atk = atk,
                def = def,
                atkSpeed = atkSpeed
            };
        }
    }
}
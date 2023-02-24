using System.Collections.Generic;
using XWDataStructure;

namespace LostCivilization.World
{
    /// <summary>
    /// 世界运行上下文(每次Update都会有数据被修改)
    /// state:
    /// 3:游戏结束
    /// </summary>
    public class StandardWorldContext : AWorldContext
    {
        /// <summary>
        /// 当前关卡难度
        /// </summary>
        public int degree;

        /// <summary>
        /// 波数是否发生了改变
        /// </summary>
        public bool isWaveChanged;
        
        /// <summary>
        /// 当前波数
        /// </summary>
        public int wave;

        /// <summary>
        /// 当前波已经生成敌人的次数
        /// </summary>
        public int enemySpawnTime;

        /// <summary>
        /// 外界因素添加角色，角色需要先走一次循环再加入到newCharacters中，再生成
        /// </summary>
        public readonly List<StandardGroup>[] tempNewCharacters;
        
        /// <summary>
        /// 新生成的角色(在下一次循环才会参与主逻辑)
        /// </summary>
        public readonly List<StandardGroup>[] newCharacters;

        /// <summary>
        /// 当前世界中参与计算的角色
        /// </summary>
        public readonly List<StandardGroup>[] curCharacters;

        /// <summary>
        /// 死亡的角色(不再参与主逻辑)
        /// </summary>
        public readonly List<StandardGroup>[] deathCharacters;

        /// <summary>
        /// 己方主堡
        /// </summary>
        public readonly StandardBuilding castle;
        
        /// <summary>
        /// 资源数量
        /// </summary>
        public int resource;
        
        /// <summary>
        /// 当前分数
        /// </summary>
        public int score;

        /// <summary>
        /// 随机数
        /// </summary>
        public SerializableRandom random;

        public StandardWorldContext(StandardWorldModel model)
        {
            tempNewCharacters = new List<StandardGroup>[2];
            newCharacters = new List<StandardGroup>[2];
            curCharacters = new List<StandardGroup>[2];
            deathCharacters = new List<StandardGroup>[2];
            for (int i = 0; i < 2; i++) {
                tempNewCharacters[i] = new List<StandardGroup>(); 
                newCharacters[i] = new List<StandardGroup>();
                curCharacters[i] = new List<StandardGroup>();
                deathCharacters[i] = new List<StandardGroup>();
            }
            random = new SerializableRandom(model.seed);
        }
    }
}
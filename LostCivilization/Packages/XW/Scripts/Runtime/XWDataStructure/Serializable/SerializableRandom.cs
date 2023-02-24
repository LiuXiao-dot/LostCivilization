using System;
using UnityEngine;

namespace XWDataStructure
{
    /// <summary>
    /// 可序列化的随机数(默认范围：0~10000)
    /// </summary>
    [Serializable]
    public class SerializableRandom : ISerializationCallbackReceiver
    {
        private System.Random _random;

        /// <summary>
        /// 当前种子(初始种子需要自行保存，不属于随机数应该保存的数据)
        /// </summary>
        [SerializeField] private int seed;

        public SerializableRandom()
        {
            _random = new System.Random();
            seed = _random.Next();
        }

        public SerializableRandom(int seed)
        {
            this.seed = seed;
            _random = new System.Random(seed);
        }

        /// <summary>
        /// 随机一次
        /// </summary>
        /// <returns></returns>
        public int Roll(int min = 0, int max = 10000)
        {
            seed = _random.Next(min, max);
            return seed;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _random = new System.Random(this.seed);
        }
    }
}
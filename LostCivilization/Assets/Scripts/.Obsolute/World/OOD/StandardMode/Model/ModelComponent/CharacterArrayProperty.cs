using UnityEngine;
using XWDataStructure;
namespace LostCivilization.World
{
    /// <summary>
    /// 角色组
    /// </summary>
    public class CharacterArrayProperty
    {
        public int side;
        /// <summary>
        /// 一组角色（战斗中只有全部角色死亡才会删除组数据，单个角色死修改状态，防止复活等效果）
        /// </summary>
        public StandardCharacter[] characters;

        /// <summary>
        /// 组控制模式
        /// exp:
        /// 0:统一行动
        /// 1:自由行动
        /// </summary>
        public int mode;

        private int capacity;

        public CharacterArrayProperty(StandardCharacterSO characterSo, int side, int capacity, Vector3 position,int mode = 0)
        {
            this.capacity = capacity;
            this.mode = mode;
            this.side = side;
            characters = new StandardCharacter[capacity];
            for (int i = 0; i < capacity; i++) {
                characters[i] = new StandardCharacter(characterSo, side, position + offsets[i % 6] * (i / 6 + 1),this);
            }
        }
        
        public void SetPosition(Vector3 position)
        {
            for (int i = 0; i < capacity; i++) {
                characters[i].SetPosition(position + offsets[i % 6] * (i / 6 + 1));
            }
        }
        
        /// <summary>
        /// 子对象在组中的偏移量
        /// </summary>
        private static Vector3[] offsets;

        static CharacterArrayProperty()
        {
            CheckOffsets();
        }
        
        private static void CheckOffsets()
        {
            if (offsets != null) return;
            offsets = new Vector3[]
            {
                new Vector3(1, 0, 0), new Vector3(0.5f, 0, 0.7f), new Vector3(-0.5f, 0, 0.7f), new Vector3(-1, 0, 0), new Vector3(-0.5f, 0, -0.7f), new Vector3(0.5f, 0, -0.7f),
            };
        }

        public void SetRotation(Quaternion rotation)
        {
            for (int i = 0; i < capacity; i++) {
                characters[i].SetRotation(rotation);
            }
        }

        /// <summary>
        /// 获取整组的状态
        /// 0:有存活
        /// 1:全部死亡
        /// </summary>
        /// <returns></returns>
        public int GetState()
        {
            var death = (int)CharacterState.Death;
            for (int i = 0; i < capacity; i++) {
                if (characters[i].GetModel<StateProperty>().state != death) {
                    return 0;
                }
            }
            return 1;
        }
    }
}
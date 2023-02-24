using UnityEngine;
namespace LostCivilization.World
{
    /// <summary>
    /// 标准模式的工厂
    /// 1.创建角色
    /// </summary>
    public class StandardFactory
    {
        /// <summary>
        /// 工厂单例(可销毁)
        /// </summary>
        public static StandardFactory Instance = new StandardFactory();
        /// <summary>
        /// 创建角色组
        /// </summary>
        /// <param name="name"></param>
        /// <param name="position"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public StandardGroup CreateGroup(string name,Vector3 position, int side)
        {
            var groupSo = StandardConfigSO.Instance.groups[name];
            var group = new StandardGroup(groupSo, new Character(){name = name},position,side);
            return group;
        }
    }
}
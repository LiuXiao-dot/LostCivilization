using Sirenix.OdinInspector;
using UnityEngine;
using XWDataStructure;
namespace LostCivilization.World
{
    /// <summary>
    /// 角色组配置数据
    /// </summary>
    public class StandardGroupSO : ScriptableObject
    {
        [LabelText("角色")]
        public StandardCharacterSO characterSo;
        [LabelText("角色数量")]
        public int capacity;
        [LabelText("移动范围")]
        public TriggerProperty triggerProperty;
        [LabelText("角色组类型")]
        [Tooltip("决定角色的移动方式")]
        public StandardGroupType groupType;
    }

    /// <summary>
    /// 角色组类型
    /// </summary>
    public enum StandardGroupType
    {
        /// <summary>
        /// 圆形区域移动
        /// </summary>
        Circle,
        /// <summary>
        /// 线段间移动
        /// </summary>
        Line,
    }
}
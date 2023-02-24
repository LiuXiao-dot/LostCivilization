using Sirenix.OdinInspector;
using UnityEngine;
using XWDataStructure;
namespace LostCivilization.World
{
    /// <summary>
    /// 角色配置
    /// </summary>
    public class StandardCharacterSO : ScriptableObject
    {
        [LabelText("碰撞器")]
        public TriggerProperty triggerProperty;
        [LabelText("战斗属性")]
        public BattleBaseProperty battleBaseProperty;
        [LabelText("移动属性")]
        public MoveProperty moveProperty;
    }
}
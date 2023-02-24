using System;
using UnityEngine;
using XWDataStructure;

namespace LostCivilization.World
{
    /// <summary>
    /// 世界中的角色数据
    /// </summary>
    public class StandardCharacter : StandardActor
    {
        /// <summary>
        /// 角色配置
        /// </summary>
        public StandardCharacterSO config;

        #region NonSerialize
        [NonSerialized] public StandardCharacterView view;
        public CharacterArrayProperty parent;
        #endregion

        public StandardCharacter(StandardCharacterSO config, int side, Vector3 position) : base(position, Quaternion.identity)
        {
            this.config = config;
            InjectModel<CampProperty>(new CampProperty(side, side));
            InjectModel<BattleBaseProperty>(config.battleBaseProperty.GetCopy());
            InjectModel<TriggerProperty>(config.triggerProperty.GetCopy());
            InjectModel<MoveProperty>(config.moveProperty.GetCopy());
            // CharacterState
            InjectModel<StateProperty>(new StateProperty());

            this.SetPosition(position);
        }

        public StandardCharacter(StandardCharacterSO config, int side, Vector3 position, CharacterArrayProperty parent) : this(config, side, position)
        {
            this.parent = parent;
        }

        public override void SetPosition(Vector3 position)
        {
            base.SetPosition(position);
            World2D.Instance.MoveShape(this, new Vector2(position.x, position.z));
        }

        internal string GetPath()
        {
            return $"{config.name}.prefab";
        }
    }
}
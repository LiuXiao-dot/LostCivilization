using UnityEngine;
using XWDataStructure;
namespace LostCivilization.World
{
    /// <summary>
    /// 角色组(己方或敌方都为一组角色同时生成)
    /// 1.包含一组角色
    /// 2.可以移动位置
    /// 3.限制组内角色的移动
    /// 4.组类型决定角色移动方式
    /// 5.组内角色全部死亡时组会被销毁/置为死亡状态(可复活)
    /// 6.todo:三个相同组可以合并/两个组合并随机选一个组保留,将另一个组的角色放入保留的组中,并有概率强化保留的组(如:增加移动范围)
    /// 7.todo:组提供范围buff/组受buff影响
    /// </summary>
    public class StandardGroup : StandardActor
    {
        /// <summary>
        /// 角色元数据
        /// </summary>
        public StandardGroupSO config;

        /// <summary>
        /// 角色的养成信息
        /// </summary>
        public Character source;

        public StandardGroup(StandardGroupSO config, Character source, Vector3 position, int side = 1) : base(position, Quaternion.identity)
        {
            this.config = config;
            this.source = source;
            InjectModel<CampProperty>(new CampProperty(side, side));
            InjectModel<TriggerProperty>(config.triggerProperty.GetCopy());
            // 0:正常 1：销毁
            InjectModel<StateProperty>(new StateProperty());
            InjectModel<CharacterArrayProperty>(new CharacterArrayProperty(config.characterSo,side,config.capacity,position));
            SetPosition(position);
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="position"></param>
        public override void SetPosition(Vector3 position)
        {
            base.SetPosition(position);
            World2D.Instance.MoveShape(this,new Vector2(position.x, position.z));
            GetModel<CharacterArrayProperty>().SetPosition(position);
        }

        public override void SetRotation(Quaternion rotation)
        {
            base.SetRotation(rotation);
            GetModel<CharacterArrayProperty>().SetRotation(rotation);
        }

        /// <summary>
        /// 刷新小组状态
        /// </summary>
        public int RefreshGroupState()
        {
            var t = GetModel<StateProperty>();
            t.SetState(GetModel<CharacterArrayProperty>().GetState());
            return t.state;
        }
    }
}
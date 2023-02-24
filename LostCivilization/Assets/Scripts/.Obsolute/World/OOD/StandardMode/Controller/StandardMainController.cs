using UnityEngine;
using XWDataStructure;
namespace LostCivilization.World
{
    /// <summary>
    /// 主逻辑控制
    /// </summary>
    public class StandardMainController : AStandardController
    {
        public StandardMainController(StandardWorldContext context) : base(context)
        {
            World2D.Instance.onTrigger -= OnTrigger;
            World2D.Instance.onTrigger += OnTrigger;
        }
        
        private void OnTrigger(World2D.TriggerEvent triggerEvent)
        //private void OnTrigger(AModel from, AModel to, Vector2 normalize)
        {
            var from = triggerEvent.from;
            var to = triggerEvent.to;
            var normalize = triggerEvent.normal;
            if (from.Contains<BattleBaseProperty>() && to.Contains<BattleBaseProperty>()) {
                // 战斗单位 通过group判断是否攻击
                var fromCamp = from.GetModel<CampProperty>();
                var toCamp = to.GetModel<CampProperty>();
                if (fromCamp.campGroup != toCamp.campGroup) {
                    var fromBattle = from.GetModel<BattleBaseProperty>();
                    var toBattle = from.GetModel<BattleBaseProperty>();
                    if (from.GetModel<StateProperty>().state == (int)CharacterState.Death) return;
                    if (to.GetModel<StateProperty>().state == (int)CharacterState.Death) return;
                    fromBattle.health = toBattle.atk > fromBattle.def ? fromBattle.health - toBattle.atk + fromBattle.def: fromBattle.health;
                    toBattle.health = fromBattle.atk > toBattle.def ? toBattle.health - fromBattle.atk + toBattle.def: toBattle.health;
                    if (fromBattle.health <= 0) {
                        fromBattle.health = 0;
                        from.GetModel<StateProperty>().SetState((int)CharacterState.Death);
                    }
                    if (toBattle.health <= 0) {
                        toBattle.health = 0;
                        to.GetModel<StateProperty>().SetState((int)CharacterState.Death);
                    }
                }
            }
            /*if (CheckSelfGroup(from, to, -1 * normalize)) {
                CheckSelfGroup(to, from, normalize);
            }*/
            
        }

        private bool CheckSelfGroup(AModel from,AModel to,Vector2 normalize)
        {
            if (from.TryGetModel<CharacterArrayProperty>(out var chaArray) && to is StandardCharacter cha && chaArray.Equals(cha.parent)) {
                switch (chaArray.side) {
                    case 0:
                        // 己方碰到边缘反弹
                        /*
                        // 用rotation判断运动方向(反弹方向增加一定的偏移，防止只在一条路径上移动)
                        if (normalize != Vector2.zero) {
                            // 碰撞了
                            cha.SetRotation(Quaternion.LookRotation(new Vector3(normalize.x, 0, normalize.y)));
                            //cha.SetRotation(cha.GetModel<BaseProperty>().rotation * Quaternion.AngleAxis(180,Vector3.up));
                        }
                        if (cha.GetModel<BaseProperty>().rotation == Quaternion.identity) {
                            cha.SetRotation(Quaternion.LookRotation(Vector3.forward));
                        }
                        */
                        
                        break;
                    default:
                        break;
                }
                return false;
            }
            return true;
        }


        /// <summary>
        /// 在此进行角色移动，碰撞检测，其他角色行为的判断
        /// </summary>
        public override void Act()
        {
            Move();
            CheckGroup();
        }
        /// <summary>
        /// 移动
        /// </summary>
        private void Move()
        {
            var groups = _context.curCharacters;

            var selfs = groups[0];
            var length = selfs.Count;
            for (int i = 0; i < length; i++) {
                MoveSelf(selfs[i]);
            }

            var enemys = groups[1];
            length = enemys.Count;
            for (int i = 0; i < length; i++) {
                MoveEnemy(enemys[i]);
            }
            
            World2D.Instance.CheckTrigger();
        }

        /// <summary>
        /// 己方移动逻辑
        /// </summary>
        private void MoveSelf(StandardGroup group)
        {
            var groupShape = group.GetModel<TriggerProperty>().shape;
            var chas = group.GetModel<CharacterArrayProperty>().characters;
            var length = group.config.capacity;
            for (int i = 0; i < length; i++) {
                // 用rotation判断运动方向(反弹方向增加一定的偏移，防止只在一条路径上移动)
                /*var cha = chas[i];
                var normalize = cha.GetModel<TriggerProperty>().shape.GetNormalize(groupShape);
                if (normalize != Vector2.zero) {
                    // 碰撞了
                    cha.SetRotation(new Vector3(normalize.x, 0, normalize.y));
                }
                if (cha.GetModel<ActorBaseProperty>().rotation == Vector3.zero) {
                    cha.SetRotation(Vector3.forward);
                }
                // 移动
                cha.SetPosition(cha.GetModel<ActorBaseProperty>().position + cha.GetModel<ActorBaseProperty>().rotation * cha.GetModel<MoveProperty>().speed * 0.02f);*/
            }
        }

        /// <summary>
        /// 敌方移动逻辑
        /// </summary>
        private void MoveEnemy(StandardGroup group)
        {
            switch (group.GetModel<CharacterArrayProperty>().mode) {
                case 0:
                    group.SetPosition(Vector3.MoveTowards(group.GetModel<ActorBaseProperty>().position, Vector3.zero, 0.02f * group.GetModel<CharacterArrayProperty>().characters[0].GetModel<MoveProperty>().speed));
                    /*if(group.GetModel<ActorBaseProperty>().position != Vector3.zero)
                        group.SetRotation(group.GetModel<ActorBaseProperty>().rotation);*/
                    break;
                case 1:
                    // 各自移动
                    break;
                default:
                    break;
            }
            // 移动完后与己方角色判断是否发生碰撞
            /*var enemys = group.GetModel<CharacterArrayProperty>().characters;
            var length = group.config.capacity;
            for (int i = 0; i < length; i++) {
                var enemy = enemys[i];
                if(enemy.GetModel<StateProperty>().state == (int)CharacterState.Death) continue; //todo:可战斗判断
                var enemyShape = enemy.GetModel<TriggerProperty>().shape;
                var selfs = _context.curCharacters[0];
                var selfLength = selfs.Count;
                for (int j = 0; j < selfLength; j++) {
                    var selfGroup = selfs[j];
                    var chaLength = selfGroup.config.capacity;
                    var selfChas = selfGroup.GetModel<CharacterArrayProperty>().characters;
                    for (int k = 0; k < chaLength; k++) {
                        var selfCha = selfChas[k];
                        if(selfCha.GetModel<StateProperty>().state == (int)CharacterState.Death) continue;
                        if(!selfCha.GetModel<TriggerProperty>().shape.CheckCollider(enemyShape)) continue;
                        var selfBP = selfCha.GetModel<BattleProperty>();
                        var enemyBP = enemy.GetModel<BattleProperty>();

                        selfBP.health -= Mathf.Max(0,enemyBP.atk - selfBP.def);
                        enemyBP.health -= Mathf.Max(0,selfBP.atk - enemyBP.def);
                        if (selfBP.health <= 0) {
                            selfBP.health = 0;
                            selfCha.GetModel<StateProperty>().SetState((int)CharacterState.Death);
                        }
                        if (enemyBP.health <= 0) {
                            enemyBP.health = 0;
                            enemy.GetModel<StateProperty>().SetState((int)CharacterState.Death);
                        }
                    }
                }
            }*/
        }

        /// <summary>
        /// 检测组状态
        /// </summary>
        private void CheckGroup()
        {
            var groups = _context.curCharacters;

            for (int i = 0; i < 2; i++) {
                var groupsList = groups[i];
                var length = groupsList.Count;
                for (int j = length - 1; j >= 0; j--) {
                    if (groupsList[j].RefreshGroupState() == 1) {
                        _context.deathCharacters[i].Add(groupsList[j]);
                        //selfs.RemoveAt(j);
                    }
                }
            }
        }
    }
}
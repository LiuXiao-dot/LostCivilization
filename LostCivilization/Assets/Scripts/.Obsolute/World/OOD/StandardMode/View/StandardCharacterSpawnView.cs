using UnityEngine;
using XWResource;
using XWUtility;

namespace LostCivilization.World
{
    /// <summary>
    /// 角色生成
    /// </summary>
    public class StandardCharacterSpawnView : MonoBehaviour
    {
        #if UNITY_EDITOR
        private int nameIndex = 1;
        #endif
        private int _sizeCount = 2;
        private bool _isDestroyed;
        //private List<List<StandardCharacterView>> _views = new List<List<StandardCharacterView>>();
        private void Awake()
        {
            /*for (int i = 0; i < _sizeCount; i++) {
                _views.Add(new List<StandardCharacterView>());
            }*/
        }

        /// <summary>
        /// 生成：先有数据后生成GameObject
        /// 销毁：先修改数据为待销毁，再销毁GameObject，最后删除数据
        /// </summary>
        /// <param name="context"></param>
        public void Check(StandardWorldContext context)
        {
            var newCharacters = context.newCharacters;
            var length0 = newCharacters.Length;
            var length1 = 0;
            for (int i = 0; i < length0; i++) {
                var tempCs = newCharacters[i];
                length1 = tempCs.Count;
                for (int j = 0; j < length1; j++) {
                    SpawnGroup(tempCs[j]);
                }
            }

            var deathCharacters = context.deathCharacters;
            length0 = deathCharacters.Length;
            for (int i = 0; i < length0; i++) {
                var tempCs = deathCharacters[i];
                length1 = tempCs.Count;
                for (int j = 0; j < length1; j++) {
                    DestroyGroup(tempCs[j]);
                }
            }
        }

        /// <summary>
        /// 召唤一组角色
        /// </summary>
        public void SpawnGroup(StandardGroup group)
        {
            var cha = group.GetModel<CharacterArrayProperty>().characters;
            var length = cha.Length;
            #if UNITY_EDITOR
            _name = nameIndex.ToString();
            nameIndex++;
  #endif
            for (int i = 0; i < length; i++) {
                SpawnCharacter(cha[i]);
            }
        }
        private string _name;

        /// <summary>
        /// 生成角色
        /// </summary>
        public void SpawnCharacter(StandardCharacter character)
        {
            var path = character.GetPath();
            AddressablesPoolManager.InstantiatePoolGameObject(path, transform, go =>
            {
                if (character == null) {
                    AddressablesPoolManager.DestroyGameObject(go);
                    return;
                }
                go.name = _name;
                var view = go.GetOrAddComponent<StandardCharacterView>();
                view.Refresh(character);
                character.view = view;
            });
        }

        /// <summary>
        /// 同步UI状态，view和model数量和顺序要对应，在此之前需要执行完生成和销毁逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="action"></param>
        public void Sync(StandardWorldContext context)
        {
            var characters = context.curCharacters;
            for (int i = 0; i < _sizeCount; i++) {
                var groups = characters[i];
                var groupLength = groups.Count;
                for (int j = 0; j < groupLength; j++) {
                    var chas = groups[j];
                    var chasLenght = chas.config.capacity;
                    for (int k = 0; k < chasLenght; k++) {
                        var ch = chas.GetModel<CharacterArrayProperty>().characters[k];
                        if(ch.view != null)
                            ch.view.Refresh(ch);
                    }
                }
            }
        }

        /// <summary>
        /// 销毁组
        /// </summary>
        public void DestroyGroup(StandardGroup group)
        {
            var cha = group.GetModel<CharacterArrayProperty>().characters;
            var length = cha.Length;
            for (int i = 0; i < length; i++) {
                DestroyCharacter(cha[i]);
            }
        }

        /// <summary>
        /// 销毁单个角色
        /// </summary>
        public void DestroyCharacter(StandardCharacter character)
        {
            var view = character.view;
            if (view != null) {
                AddressablesPoolManager.DestroyGameObject(view.gameObject);
            }
            character.view = null;
            character = null;
        }
    }
}
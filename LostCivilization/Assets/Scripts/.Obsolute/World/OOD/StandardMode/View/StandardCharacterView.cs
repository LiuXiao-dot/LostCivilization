using UnityEngine;
using XWDataStructure;
namespace LostCivilization.World
{
    public class StandardCharacterView : MonoBehaviour
    {
        private Transform _trans;
        private void Awake()
        {
            this._trans = transform;
        }
        public void Refresh(StandardCharacter character)
        {
            _trans.position = character.GetModel<ActorBaseProperty>().position;
            _trans.rotation = character.GetModel<ActorBaseProperty>().rotation;
            gameObject.SetActive(character.GetModel<StateProperty>().state == 0);
        }
    }
}
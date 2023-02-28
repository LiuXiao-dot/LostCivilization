using Sirenix.OdinInspector;
using UnityEngine;
using XWDataStructure;
namespace XWActor
{
    /// <summary>
    /// Actor的配置数据
    /// </summary>
    public class ActorConfig : ScriptableObject
    {
        [LabelText("角色类型")]
        [ReadOnly]
        public string actorType;

        public SerializableDictionary<string,Actor> actors;
    }
}
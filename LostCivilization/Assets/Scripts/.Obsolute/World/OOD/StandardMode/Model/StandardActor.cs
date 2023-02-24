using UnityEngine;
using XWDataStructure;
namespace LostCivilization.World
{
    /// <summary>
    /// 标准世界中的元素的公共父类
    /// </summary>
    public class StandardActor : AModel
    {
        public StandardActor( Vector3 position, Quaternion quaternion)
        {
            InjectModel<ActorBaseProperty>(new ActorBaseProperty()
            {
                position = position,
                rotation = quaternion
            });
        }

        public virtual void SetPosition(Vector3 position)
        {
            GetModel<ActorBaseProperty>().SetPosition(position);
        }
        
        public virtual void SetRotation(Quaternion rotation)
        {
            GetModel<ActorBaseProperty>().SetRotation(rotation);
        }
    }
}
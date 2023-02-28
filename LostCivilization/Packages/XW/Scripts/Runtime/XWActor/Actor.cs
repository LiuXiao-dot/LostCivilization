using System;
using XWDataStructure;
namespace XWActor
{
    /// <summary>
    /// Actor:Property的数据容器
    /// </summary>
    [Serializable]
    public class Actor : AModel
    {
        public override void InjectModel(Type t, object newModel)
        {
            if (models.ContainsKey(t))
            {
                #if UNITY_EDITOR
                UnityEngine.Debug.LogWarning($"存在相同类型的数据（为防止数据混乱，各个模块的数据需要使用一个总的类包含模块中的所有数据，只存在一个）{newModel.GetType()}");
                #endif
                return;
            }
            models.Add(t, newModel);
            if (newModel is AProperty { parent: null } property) property.parent = this;
        }
    }
}
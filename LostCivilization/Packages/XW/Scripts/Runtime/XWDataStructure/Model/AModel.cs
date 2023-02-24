using System;
namespace XWDataStructure
{
    /// <summary>
    /// 数据抽象类
    /// </summary>
    public abstract class AModel
    {
        public ObjectDictionary models = new ObjectDictionary();

        /// <summary>
        /// 注入数据
        /// </summary>
        public void InjectModel(object newModel)
        {
            InjectModel(newModel.GetType(),newModel);
        }

        public virtual void InjectModel(Type t,object newModel)
        {
            if (models.ContainsKey(t))
            {
                #if UNITY_EDITOR
                UnityEngine.Debug.LogWarning($"存在相同类型的数据（为防止数据混乱，各个模块的数据需要使用一个总的类包含模块中的所有数据，只存在一个）{newModel.GetType()}");
                #endif
                return;
            }
            models.Add(t, newModel);
        }

        public void InjectModel<T>(object newModel)
        {
            InjectModel(typeof(T),newModel);
        }
        
        public void InjectModel<T>()
        {
            InjectModel(Activator.CreateInstance(typeof(T)));
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public T GetModel<T>()
        {
            return (T)GetModel(typeof(T));
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public object GetModel(Type t)
        {
            return models[t];
        }

        public bool TryGetModel(Type key,out object t)
        {
            return models.TryGetValue(key,out t);
        }
        
        public bool TryGetModel<T>(out T t)
        {
            var result = TryGetModel(typeof(T),out object o);
            t = (T)o;
            return result;
        }

        public bool Contains(Type key)
        {
            return models.ContainsKey(key);
        }

        public bool Contains<T>()
        {
            return Contains(typeof(T));
        }
    }
}
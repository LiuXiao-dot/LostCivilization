using UnityEngine;

namespace XWDataStructure
{
    /// <summary>
    /// 单例ScriptableObject
    /// </summary>
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T>
    {
        public static T Instance
        {
            get {
                return _instance;
            }
        }

        private static T _instance;

        protected virtual void OnEnable()
        {
            if (_instance != null && this != _instance)
            {
                //Debug.LogError($"存在多个{typeof(T)}实例");
                return;
            }

            _instance = (T)this;
        }
    }
}
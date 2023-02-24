using UnityEngine;

namespace XWUtility
{
    public static class GameObjectExtension
    {
        /// <summary>
        /// 获取或添加Component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.TryGetComponent(out T a) ? a : gameObject.AddComponent<T>();
        }
    }
}

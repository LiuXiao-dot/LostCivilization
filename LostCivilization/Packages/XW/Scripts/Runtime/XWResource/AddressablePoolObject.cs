/// <summary>
/// @author TTM
/// @Modified 2022年03月18日 星期五 17:15
/// </summary>

using System;
using UnityEngine;

namespace XWResource
{
    /// <summary>
    /// AddressablePoolObject:
    /// 描述:用于AddressablePool的对象需要挂在该脚本
    /// </summary>
    [DisallowMultipleComponent]
    public class AddressablePoolObject:MonoBehaviour
    {
        private AddressablePool _pool;
        internal void GetObject(AddressablePool pool)
        {
            _pool = pool;
            gameObject.SetActive(true);
            OnGetObject();
        }

        /// <summary>
        /// 从池中取出，初始化设置
        /// </summary>
        public virtual void OnGetObject()
        {
            
        }
        
        internal void ResetObject()
        {
            gameObject.SetActive(false);
            gameObject.transform.localPosition = Vector3.zero;
            OnResetObject();
        }
        
        /// <summary>
        /// 回收到池中时，重置状态
        /// </summary>
        public virtual void OnResetObject()
        {
            
        }

        public AddressablePool GetPool()
        {
            return _pool;
        }

        public void Destroy()
        {
            _pool.Release(gameObject);
        }
    }
}
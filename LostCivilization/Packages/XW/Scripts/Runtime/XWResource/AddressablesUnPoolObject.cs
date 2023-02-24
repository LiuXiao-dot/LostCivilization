/// <summary>
/// @author TTM
/// @Modified 2022年03月18日 星期五 18:07
/// </summary>

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace XWResource
{
    /// <summary>
    /// AddressablesUnPoolObject:
    /// 描述：非池对象销毁，会自动释放引用
    /// </summary>
    [DisallowMultipleComponent]
    public class AddressablesUnPoolObject:MonoBehaviour
    {
        public AsyncOperationHandle<GameObject> handle;
        private void OnDestroy()
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace XWResource
{
    /// <summary>
    /// Addressables相关工具方法
    /// </summary>
    public sealed class AddressablesUtils
    {
        public static AddressablesUtils Instance = new AddressablesUtils();

        /// <summary>
        /// 请求加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="address">资源Addressables地址</param>
        /// <param name="callback">加载完成回调</param>
        public void RequestLoad<T>(string address,Action<AsyncOperationHandle<T>> callback)
        {
            Addressables.LoadAssetAsync<T>(address).Completed += callback;
        }

        /// <summary>
        /// 实例化Prefab
        /// </summary>
        /// <param name="address">Prefab的Addressables地址</param>
        /// <param name="callback">加载+实例化完成的回调</param>
        public void RequestInstantiate(string address, Action<AsyncOperationHandle<GameObject>> callback)
        {
            Addressables.InstantiateAsync(address).Completed += callback;
        }
    }
}

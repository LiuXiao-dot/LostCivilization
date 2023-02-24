using System;
using UnityEngine.AddressableAssets;
using XWDataStructure;

namespace XWResource
{
    public class AddressablesPreloader : IPreloader
    {
        public void Load(Action<float> onProgress)
        {
            var handle = Addressables.InitializeAsync(); // 提前初始化Addressables，防止未初始化导致的错误
            handle.Completed += operationHandle =>
            {
                onProgress.Invoke(1);
            };
        }
    }
}
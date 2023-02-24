using System;

namespace XWDataStructure
{
    /// <summary>
    /// 预加载接口
    /// </summary>
    public interface IPreloader
    {
        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="onProgress">加载进度发生变化</param>
        void Load(Action<float> onProgress);
    }
}
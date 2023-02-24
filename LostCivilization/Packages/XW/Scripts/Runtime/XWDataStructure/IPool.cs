namespace XWDataStructure
{
    /// <summary>
    /// 池数据的接口
    /// </summary>
    public interface IPool<T>
    {
        /// <summary>
        /// 从池中获取元素
        /// </summary>
        /// <returns></returns>
        T Get();
        
        /// <summary>
        /// 释放元素
        /// </summary>
        /// <param name="element"></param>
        void Release(T element);
    }
}
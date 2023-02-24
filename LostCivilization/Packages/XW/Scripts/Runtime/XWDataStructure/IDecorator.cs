namespace XWDataStructure
{
    /// <summary>
    /// 装饰器
    /// </summary>
    public interface IDecorator<T>
    {
        /// <summary>
        /// 装饰source并返回装饰后的T
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        void Decorate(T source);
    }
}
namespace XWDataStructure
{
    /// <summary>
    /// 工厂模式的工厂
    /// </summary>
    public interface IFactory<T>
    {
        T Create();
    }
}
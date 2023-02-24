namespace XWDataStructure
{
    /// <summary>
    /// IOC组件（继承IComponent）的容器
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// 注入一个组件，并替换掉同key值的组件或添加一个新的组件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="component"></param>
        void Injection(string key,IComponent component);

        IComponent GetComponent(string key);
    }

    /// <summary>
    /// IOC组件
    /// </summary>
    public interface IComponent
    {
    }

    /// <summary>
    /// IOC组件间自动引用
    /// </summary>
    public interface IAutowired
    {
        /// <summary>
        /// 装配全部container中的组件
        /// </summary>
        /// <param name="container"></param>
        void Wired(IContainer container);
    }
}
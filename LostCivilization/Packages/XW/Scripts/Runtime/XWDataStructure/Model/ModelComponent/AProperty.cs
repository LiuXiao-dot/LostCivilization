using System;
namespace XWDataStructure
{
    /// <summary>
    /// 数据抽象类
    /// </summary>
    [Serializable]
    public abstract class AProperty
    {
        /// <summary>
        /// 依赖的父数据。
        /// 如果一个Property要被多个AModel公用，需要将该Property添加到一个共享数据中。
        /// </summary>
        public AModel parent;
    }
}
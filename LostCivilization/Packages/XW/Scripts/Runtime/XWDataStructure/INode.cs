using System.Collections.Generic;

namespace XWDataStructure
{
    /// <summary>
    /// 节点
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="node"></param>
        void AddChild(INode node);
        /// <summary>
        /// 移除子节点
        /// </summary>
        /// <param name="node"></param>
        void RemoveChild(INode node);
        /// <summary>
        /// 获取全部子节点
        /// </summary>
        /// <returns></returns>
        List<INode> GetChilds();
    }
}
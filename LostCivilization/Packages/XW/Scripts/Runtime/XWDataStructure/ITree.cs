using System;
using System.Collections.Generic;

namespace XWDataStructure
{
    /// <summary>
    /// 树
    /// </summary>
    public interface ITree
    {
        /// <summary>
        /// 获取根节点
        /// </summary>
        /// <returns></returns>
        INode GetRoot();

        /// <summary>
        /// 查找第一个满足filter节点
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        INode FindNode(Func<bool> filter);
        
        /// <summary>
        /// 查找全部满足filter的节点
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<INode> FindNodes(Func<bool> filter);
    }
}
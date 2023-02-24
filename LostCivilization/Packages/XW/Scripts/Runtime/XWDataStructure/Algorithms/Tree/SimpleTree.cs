using System;
using System.Collections.Generic;

namespace XWDataStructure
{
    /// <summary>
    /// 简单没有过多条件限制的树(可序列化)
    /// </summary>
    [Serializable]
    public abstract class SimpleTree<T>
    {
        /// <summary>
        /// 子树
        /// </summary>
        public List<SimpleTree<T>> childs;

        /// <summary>
        /// 该树的根节点的数据
        /// </summary>
        public T value;
        
        public SimpleTree(T value)
        {
            this.value = value;
            childs = new List<SimpleTree<T>>();
        }

        /// <summary>
        /// 添加子节点（会创建一个只包含自己的数据的子树）
        /// </summary>
        /// <param name="childValue"></param>
        public void AddChild(T childValue)
        {
            childs.Add((SimpleTree<T>)Activator.CreateInstance(GetType(),childValue));
        }

        public void RemoveChild(SimpleTree<T> child)
        {
            childs.Remove(child);
        }
        
        public void RemoveChild(T childValue)
        {
            var index = childs.FindIndex(temp => temp.value.Equals(childValue));
            if(index == -1) return;
            childs.RemoveAt(index);
        }

        /// <summary>
        /// 广度遍历
        /// </summary>
        /// <param name="action"></param>
        public void WidelyForeach(Action<T> action)
        {
            action(value);
            ForeachChilds(action);
        }

        public void ForeachChilds(Action<T> action)
        {
            foreach (var child in childs)
            {
                action(child.value);
            }
            foreach (var child in childs)
            {
                child.ForeachChilds(action);
            }
        }
    }
}
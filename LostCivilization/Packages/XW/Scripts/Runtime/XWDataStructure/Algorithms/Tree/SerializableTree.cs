using System;
using System.Collections.Generic;

namespace XWDataStructure
{
    /// <summary>
    /// 节点组
    /// </summary>
    [System.Serializable]
    public class SerializableTree
    {
        /// <summary>
        /// 列表中的id是（1，1）递增的
        /// </summary>
        public List<SerializableTreeNode> serializableTreeNodes;

        public SerializableTree()
        {
            serializableTreeNodes = new List<SerializableTreeNode> { CreateSourceNode() };
        }

        public SerializableTree(List<SerializableTreeNode> SerializableTreeNodes) : this()
        {
            this.serializableTreeNodes = new List<SerializableTreeNode>();
            this.serializableTreeNodes.AddRange(SerializableTreeNodes);
            SerializableTreeNodes.Clear();
        }

        #region operation

        /// <summary>
        /// 创造源节点
        /// </summary>
        /// <returns></returns>
        protected SerializableTreeNode CreateSourceNode()
        {
            return CreateNode(0, "SOURCE");
        }

        /// <summary>
        /// 继承后创建不一样的节点
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual SerializableTreeNode CreateNode(int id, string name)
        {
            return new SerializableTreeNode(id, name);
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="parentId">父节点ID</param>
        /// <param name="name">节点名</param>
        /// <param name="cost">节点消耗</param>
        /// <returns></returns>
        public void AddChildNode(int parentId, string name)
        {
            var parentNode = GetNode(parentId);
            var newNode = CreateNode(GetNextNodeID(), name);
            parentNode.AddChild(CreateNode(GetNextNodeID(), name));
            OnAddNode(newNode);
        }

        public void AddNode(int id, string name)
        {
            var parentNode = GetParentNode(id);
            var newNode = CreateNode(GetNextNodeID(), name);
            parentNode.InsertChild(id, newNode.id);
            OnAddNode(newNode);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveNode(int id)
        {
            var parent = GetParentNode(id);
            return parent.RemoveChild(id) && serializableTreeNodes.Remove(GetNode(id));
        }

        private void OnAddNode(SerializableTreeNode SerializableTreeNode)
        {
            serializableTreeNodes.Add(SerializableTreeNode);
            serializableTreeNodes.Sort((a, b) => a.id.CompareTo(b.id));
        }

        /// <summary>
        /// 更改父节点
        /// </summary>
        /// <param name="oldParentId"></param>
        /// <param name="newParentId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ChangeNodeParent(int oldParentId, int newParentId, int id)
        {
            return GetNode(oldParentId).RemoveChild(id) && GetNode(newParentId).AddChild(id);
        }

        private int GetNextNodeID()
        {
            var newId = 0;
            var count = serializableTreeNodes.Count;
            for (int i = 0; i < count; i++)
            {
                if (serializableTreeNodes[i].id == newId)
                {
                    newId++;
                    continue;
                }
            }

            return newId;
        }

        #endregion


        #region Utils

        /// <summary>
        /// 源节点id为0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SerializableTreeNode GetNode(int id)
        {
            foreach (var node in serializableTreeNodes)
            {
                if (node.id == id)
                    return node;
            }

            return null;
        }

        public SerializableTreeNode GetParentNode(int id)
        {
            foreach (var node in serializableTreeNodes)
            {
                if (node.childs.Exists(child => child == id))
                    return node;
            }

            return null;
        }

        /// <summary>
        /// 检测节点是否合法
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual bool CheckNode(SerializableTreeNode node)
        {
            return true;
        }

        #endregion
    }

    /// <summary>
    /// Unity的JsonUtility可序列化的树
    /// </summary>
    [Serializable]
    public class SerializableTreeNode
    {
        /// <summary>
        /// 节点唯一ID
        /// </summary>
        public int id;

        /// <summary>
        /// 节点名称
        /// </summary>
        public string name;

        /// <summary>
        /// 子节点ID
        /// </summary>
        public List<int> childs;

        /// <summary>
        /// 子节点数量
        /// </summary>
        public int childCount;

        public SerializableTreeNode(int id, string name)
        {
            this.id = id;
            this.name = name;
            this.childs = new List<int>();
            this.childCount = 0;
        }

        public bool AddChild(SerializableTreeNode newChild)
        {
            return AddChild(newChild.id);
        }

        public bool AddChild(int id)
        {
            if (childs.Contains(id))
                return false;
            childs.Add(id);
            childCount++;
            return true;
        }

        /// <summary>
        /// 插入节点
        /// </summary>
        public bool InsertChild(int oldId, int newId)
        {
            if (childs.Contains(id))
                return false;
            var oldIndex = childs.IndexOf(oldId);
            childs.Insert(oldIndex + 1, newId);
            childCount++;
            return true;
        }

        public bool RemoveChild(int id)
        {
            if (childs.Remove(id))
            {
                childCount--;
                return true;
            }

            return false;
        }

        public bool RemoveChild(SerializableTreeNode oldChild)
        {
            return RemoveChild(oldChild.id);
        }
    }
}
using System;
namespace XWDataStructure
{
    /// <summary>
    /// 阵营
    /// </summary>
    [Serializable]
    public class CampProperty : AProperty
    {
        /// <summary>
        /// 阵营的id
        /// </summary>
        public int campId;

        /// <summary>
        /// 阵营分组(多个id在同一个组中，视为盟军)
        /// </summary>
        public int campGroup;
        public CampProperty(int campId, int campGroup)
        {
            this.campId = campId;
            this.campGroup = campGroup;
        }

        /// <summary>
        /// 分到新的组中
        /// </summary>
        public void SetGroup(int campGroup)
        {
            this.campGroup = campGroup;
        }
    }
}
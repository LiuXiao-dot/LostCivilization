using XWMessageQueue;
namespace XWGrid.Terrain
{
    [MessageQueue(isMainThread = true)]
    public enum TerrainMessage
    {
        /// <summary>
        /// 根据坐标计算点再去修改value
        /// </summary>
        SetPositionValue,
        
        /// <summary>
        /// 更新点坐标
        /// </summary>
        SetVertexValue,
    }
}
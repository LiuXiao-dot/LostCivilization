namespace XWConverter
{
    /// <summary>
    /// 数据写出器
    /// </summary>
    public abstract class ADataWriter
    {
        public abstract void Write(AILData data, string path);
    }
}
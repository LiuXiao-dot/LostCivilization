namespace XWConverter
{
    /// <summary>
    /// 数据读取解析器
    /// </summary>
    public abstract class ADataReader
    {
        public abstract AILData Read(string path);
    }
}
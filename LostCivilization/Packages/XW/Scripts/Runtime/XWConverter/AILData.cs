namespace XWConverter
{
    /// <summary>
    /// 中间类型的数据，不同格式文件间转换都需要先从原格式转换为原格式的ILData，再转换为目标格式的ILData，再转换成目标格式.
    /// tip:不同格式可能需要定义自己的ILData
    /// </summary>
    public abstract class AILData
    {
    }
}
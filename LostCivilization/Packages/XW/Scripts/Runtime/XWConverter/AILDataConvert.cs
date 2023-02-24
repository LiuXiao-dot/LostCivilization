namespace XWConverter
{
    /// <summary>
    /// 中间数转换器
    /// </summary>
    public abstract class AILDataConvert<F, T>
    {
        public abstract T Convert(F from);
    }
}
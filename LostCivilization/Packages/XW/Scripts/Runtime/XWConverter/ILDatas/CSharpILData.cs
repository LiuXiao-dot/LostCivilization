namespace XWConverter
{
    /// <summary>
    /// C#文件 中间数据
    /// </summary>
    public class CSharpILData : AILData
    {
        /// <summary>
        /// 基础代码字符串
        /// </summary>
        public string csharpText;

        public override string ToString()
        {
            return csharpText;
        }
    }
}
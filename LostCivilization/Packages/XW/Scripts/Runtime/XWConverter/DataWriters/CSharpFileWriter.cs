using System.IO;
using System.Text;

namespace XWConverter
{
    /// <summary>
    /// C# class代码生成
    /// </summary>
    public class CSharpFileWriter : ADataWriter
    {
        public override void Write(AILData data, string path)
        {
            File.WriteAllText(path, data.ToString(),Encoding.UTF8);
        }
    }
}
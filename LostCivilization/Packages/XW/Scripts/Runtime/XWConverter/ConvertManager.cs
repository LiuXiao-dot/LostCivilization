using System.IO;

namespace XWConverter
{
    /// <summary>
    /// 数据转换器
    /// </summary>
    public class ConvertManager
    {
        /// <summary>
        /// IL中间数据转换
        /// </summary>
        /// <param name="from">源数据</param>
        /// <param name="to">目标数据</param>
        public void ConvertIL(AILData from, AILData to)
        {
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="from">源数据</param>
        /// <param name="to">目标数据</param>
        /// <typeparam name="F">源数据类型</typeparam>
        /// <typeparam name="T">目标数据类型</typeparam>
        public void Convert<F, T>(F from, T to)
        {
        }

        /// <summary>
        /// 根据路径加载文件，并转换为目标文件格式，并生成目标文件,并使用默认的转换器进行处理。使用该方法文件扩展名需要正确
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        public void Convert(string fromPath, string toPath)
        {
            var fromExtension = Path.GetExtension(fromPath);
            var toExtension = Path.GetExtension(toPath);
        }

        /// <summary>
        /// 使用专门的数据解析器进行文件转换
        /// </summary>
        /// <param name="fromPath"></param>
        /// <param name="toPath"></param>
        /// <param name="dataReader"></param>
        /// <param name="dataWriter"></param>
        /// <param name="convert"></param>
        /// <typeparam name="F"></typeparam>
        /// <typeparam name="T"></typeparam>
        public void Convert<F, T>(string fromPath, string toPath, ADataReader dataReader, ADataWriter dataWriter,
            AILDataConvert<F, T> convert) where F : AILData where T : AILData
        {
            var ilFrom = dataReader.Read(fromPath);
            var ilTo = convert.Convert((F)ilFrom);
            dataWriter.Write(ilTo, toPath);
        }
    }
}
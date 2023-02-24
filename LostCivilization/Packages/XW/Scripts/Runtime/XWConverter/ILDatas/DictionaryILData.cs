using System.Collections.Generic;
// ReSharper disable All

namespace XWConverter
{
    /// <summary>
    /// 键值对（字典）型的中间数据
    /// </summary>
    public class DictionaryILData : AILData
    {
        public Dictionary<string, List<string>> ilDatas = new Dictionary<string, List<string>>();

        public List<string> this[string key] => ilDatas[key];

        public void Add(string key, string value)
        {
            if (!ilDatas.TryGetValue(key, out List<string> valueList))
            {
                valueList = new List<string>();
                ilDatas.Add(key, valueList);
            }

            valueList.Add(value);
        }
    }
}
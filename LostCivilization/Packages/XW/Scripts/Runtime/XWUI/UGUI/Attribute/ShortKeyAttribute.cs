using System;
namespace XWUI
{
    /// <summary>
    /// key,需要全大写，根据key值自动生成UI代码
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ShortKeyAttribute : System.Attribute
    {
        public string key;

        public ShortKeyAttribute(string key = null)
        {
            this.key = key;
        }
    }
}
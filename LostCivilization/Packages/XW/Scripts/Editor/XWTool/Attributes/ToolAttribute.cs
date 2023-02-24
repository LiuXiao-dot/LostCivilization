using System;

namespace XWTool
{
    /// <summary>
    /// 创建一个工具所需要的数据
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ToolAttribute : Attribute
    {
        public string path;
        public ToolIconEnum icon;

        public ToolAttribute(string path,ToolIconEnum icon = ToolIconEnum.NONE)
        {
            this.path = path;
            this.icon = icon;
        }
    }
}
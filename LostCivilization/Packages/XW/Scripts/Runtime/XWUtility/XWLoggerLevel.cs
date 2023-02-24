
namespace XWUtility
{
    using System;
    /// <summary>
    /// debug等级
    /// </summary>
    [Flags]
    public enum XWLoggerLevel
    {
        DEBUG = 1,
        WARNING = 2,
        ERROR = 4,
        OUTPUT = 8
    }
}
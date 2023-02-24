namespace XWUtilityEditor
{
    using System;

    /// <summary>
    /// 统计方法/属性的耗时与内存消耗
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class TimeAttribute : Attribute
    {
    }
}
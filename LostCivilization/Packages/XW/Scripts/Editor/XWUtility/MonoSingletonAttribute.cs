namespace XWUtilityEditor
{
    using System;

    /// <summary>
    /// Monobehaviour的单例
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MonoSingletonAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="needField">是否需要声明一个字段</param>
        public MonoSingletonAttribute(bool needField = false)
        {
        }
    }
}
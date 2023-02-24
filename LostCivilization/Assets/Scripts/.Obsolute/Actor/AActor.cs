namespace LostCivilization
{
    /// <summary>
    /// 所有Actor元素的父类
    /// </summary>
    public abstract class AActor
    {
        public string name;

        /// <summary>
        /// 用于加载资源
        /// </summary>
        /// <returns></returns>
        public abstract string GetPath();
    }
}
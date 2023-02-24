namespace LostCivilization.World
{
    /// <summary>
    /// 所有状态机需要继承该接口。（状态由T本身提供，State中只包含逻辑，不存储T的状态数据）
    /// </summary>
    public interface IState<T>
    {
        /// <summary>
        /// 更新t的数据与状态
        /// </summary>
        /// <param name="t"></param>
        void Update(T t);
    }
}
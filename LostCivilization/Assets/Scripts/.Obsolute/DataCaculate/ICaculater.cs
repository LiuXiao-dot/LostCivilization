namespace LostCivilization
{
    /// <summary>
    /// 计算器
    /// </summary>
    public interface ICaculater<I,O>
    {
        O Calculate(I input);
    }
}
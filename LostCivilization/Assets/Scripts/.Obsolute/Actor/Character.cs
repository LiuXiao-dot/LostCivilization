namespace LostCivilization
{
    /// <summary>
    /// 角色数据
    /// </summary>
    public class Character : AActor
    {
        public override string GetPath()
        {
            return $"World/Charactor/Prefabs/{name}.prefab";
        }
    }
}
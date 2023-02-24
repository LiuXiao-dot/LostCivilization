#if XW_ENABLE_DOTS
using UnityEngine.Jobs;

namespace LostCivilization.World
{
    /// <summary>
    /// 移动动画
    /// 调用方式:moveJob.Schedule(TransformAccessArray)
    /// </summary>
    public class MoveJob : IJobParallelForTransform
    {
        public void Execute(int index, TransformAccess transform)
        {

        }
    }
}
#endif
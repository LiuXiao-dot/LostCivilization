using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Profiling;
namespace LostCivilization.Test
{
    /// <summary>
    /// 类型获取计算
    /// result:无GC
    /// </summary>
    public class TypeReflectionTest : MonoBehaviour
    {
        [Button]
        private void Reflection()
        {
            Profiler.BeginSample("Reflection0");
            var type0 = typeof(int);
            type0 = typeof(long);
            Profiler.EndSample();
            var a = 1;
            Profiler.BeginSample("Reflection1");
            for (int i = 0; i < 10; i++) {
                type0 = a.GetType();
            }
            Profiler.EndSample();
            Profiler.enabled = false;
        }
    }
}
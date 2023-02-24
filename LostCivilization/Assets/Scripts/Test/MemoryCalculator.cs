/*using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Profiling;
namespace LostCivilization.Test
{
    public class MemoryCalculator :MonoBehaviour
    {
        [Button]
        public void TestStringCombine()
        {
            Profiler.BeginSample("MemoryCalculator0");
            var s1 = "s1";
            Profiler.EndSample();
            Profiler.BeginSample("MemoryCalculator1");
            var result = $"{s1}_s2";
            Profiler.EndSample();
            Profiler.BeginSample("MemoryCalculator2");
            var s2 = "s2";
            Profiler.EndSample();
            Profiler.BeginSample("MemoryCalculator3");
            result = $"{s1}_{s2}";
            Profiler.EndSample();
            
            Profiler.BeginSample("MemoryCalculator4");
            result = s1 + s2;
            Profiler.EndSample();
            
            Profiler.BeginSample("MemoryCalculator3_1");
            for (int i = 0; i < 100; i++) {
                result = $"{s1}_{s2}";
            }
            Profiler.EndSample();
            
            Profiler.BeginSample("MemoryCalculator4_1");
            for (int i = 0; i < 100; i++) {
                result = s1 + s2;
            }
            Profiler.EndSample();
            Profiler.enabled = false;
        }

        private enum TestEnum
        {
            A,
            B,
            C,
            D,
            E
        }
        
        [Button]
        public void TestEnumToInt()
        {
            Profiler.BeginSample("MemoryCalculator0");
            var s1 = (int)TestEnum.A;
            var s2 = TestEnum.B + 1;
            Profiler.EndSample();
            Profiler.enabled = false;
        }
    }
}*/
using System;

namespace XWDataStructure
{
    /// <summary>
    /// 数组扩展
    /// </summary>
    public static class ArrayExtension
    {
        public static bool IsEmptyOrNull(this Array array)
        {
            return array == null || array.Length == 0;
        }
    }
}
namespace XWDataStructure
{
    /// <summary>
    /// 整型
    /// todo:内存加密
    /// </summary>
    public struct FixedInt
    {
        private int value;
        
        public FixedInt(int value = 0)
        {
            this.value = value;
        }

        public static implicit operator int(FixedInt fixedInt)
        {
            return fixedInt.value;
        }
        
        public static implicit operator FixedInt(int intValue)
        {
            return new FixedInt(intValue);
        }
        
        public static FixedInt operator +(FixedInt fixedInt0,FixedInt fixedInt1)
        {
            return new FixedInt(fixedInt0.value + fixedInt1.value);
        }
        
        public static FixedInt operator -(FixedInt fixedInt0,FixedInt fixedInt1)
        {
            return new FixedInt(fixedInt0.value - fixedInt1.value);
        }
        
        public static FixedInt operator *(FixedInt fixedInt0,FixedInt fixedInt1)
        {
            return new FixedInt(fixedInt0.value * fixedInt1.value);
        }
        
        public static FixedInt operator /(FixedInt fixedInt0,FixedInt fixedInt1)
        {
            return new FixedInt(fixedInt0.value / fixedInt1.value);
        }
        
        public static FixedInt operator %(FixedInt fixedInt0,FixedInt fixedInt1)
        {
            return new FixedInt(fixedInt0.value % fixedInt1.value);
        }
        
        public static FixedInt operator ++(FixedInt fixedInt0)
        {
            return fixedInt0.value++;
        }
        
        public static FixedInt operator --(FixedInt fixedInt0)
        {
            return fixedInt0.value--;
        }
    }
}
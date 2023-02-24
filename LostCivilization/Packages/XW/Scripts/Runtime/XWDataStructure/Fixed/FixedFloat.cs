namespace XWDataStructure
{
    /// <summary>
    /// 浮点数:只保留三位小数
    /// todo:内存加密
    /// </summary>
    public struct FixedFloat
    {
        private int value;
        private const int RATIO = 1000;
        public FixedFloat(float value = 0)
        {
            this.value = (int)(value * RATIO);
        }

        public FixedFloat(int value)
        {
            this.value = value * RATIO;
        }

        public static implicit operator float(FixedFloat FixedFloat)
        {
            return (float)FixedFloat.value / RATIO;
        }

        public static implicit operator FixedFloat(float floatValue)
        {
            return new FixedFloat(floatValue);
        }

        public static FixedFloat operator +(FixedFloat FixedFloat0, FixedFloat FixedFloat1)
        {
            return new FixedFloat(FixedFloat0.value + FixedFloat1.value);
        }

        public static FixedFloat operator -(FixedFloat FixedFloat0, FixedFloat FixedFloat1)
        {
            return new FixedFloat(FixedFloat0.value - FixedFloat1.value);
        }

        public static FixedFloat operator *(FixedFloat FixedFloat0, FixedFloat FixedFloat1)
        {
            return new FixedFloat(FixedFloat0.value * FixedFloat1.value / 1000);
        }

        public static FixedFloat operator /(FixedFloat FixedFloat0, FixedFloat FixedFloat1)
        {
            return new FixedFloat(FixedFloat0.value / FixedFloat1.value);
        }
    }
}
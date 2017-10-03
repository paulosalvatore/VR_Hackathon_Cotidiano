using System;
using System.Collections;

namespace Kit.Extend
{
    /// <summary>the better random.</summary>
    /// <see cref="http://stackoverflow.com/questions/3365337/best-way-to-generate-a-random-float-in-c-sharp"/>
    public static class RandomExtend
    {
        public static double NextDouble()
        {
            Random random = new Random();
            double mantissa = (random.NextDouble() * 2f) - 1f;
            double expoenet = Math.Pow(2f, random.Next(-126, 128));
            return mantissa * expoenet;
        }
        public static double NextDouble(double minValue, double maxValue)
        {
            return RandomExtend.NextDouble() * (maxValue - minValue) + minValue;
        }
        public static float NextFloat()
        {
            return (float)RandomExtend.NextDouble();
        }
        public static float NextFloat(float minValue, float maxValue)
        {
            return RandomExtend.NextFloat() * (maxValue - minValue) + minValue;
        }
    }
}

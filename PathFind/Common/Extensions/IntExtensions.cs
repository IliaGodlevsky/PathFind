﻿using System.Linq;

namespace Common.Extensions
{
    public static class IntExtensions
    {
        public static int Xor(this int value, int value2)
        {
            return value ^ value2;
        }

        public static long Pow(this int value, int power)
        {
            return Enumerable
                  .Repeat(value, power)
                  .Aggregate(1, (a, b) => a * b);
        }

        public static int Multiply(this int value, int value2)
        {
            return value * value2;
        }
    }
}
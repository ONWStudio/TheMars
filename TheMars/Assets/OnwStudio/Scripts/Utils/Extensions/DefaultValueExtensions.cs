using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Helper;

namespace Onw.Extensions
{
    public static class DefaultValueExtensions
    {
        public static int Min(this int value, int min)
        {
            return DefaultValueHelper.Min(value, min);
        }

        public static int Max(this int value, int max)
        {
            return DefaultValueHelper.Max(value, max);
        }

        public static uint Min(this uint value, uint min)
        {
            return DefaultValueHelper.Min(value, min);
        }

        public static uint Max(this uint value, uint max)
        {
            return DefaultValueHelper.Max(value, max);
        }

        public static byte Min(this byte value, byte min)
        {
            return DefaultValueHelper.Min(value, min);
        }

        public static byte Max(this byte value, byte max)
        {
            return DefaultValueHelper.Max(value, max);
        }

        public static sbyte Min(this sbyte value, sbyte min)
        {
            return DefaultValueHelper.Min(value, min);
        }

        public static sbyte Max(this sbyte value, sbyte max)
        {
            return DefaultValueHelper.Max(value, max);
        }

        public static short Min(this short value, short min)
        {
            return DefaultValueHelper.Min(value, min);
        }

        public static short Max(this short value, short max)
        {
            return DefaultValueHelper.Max(value, max);
        }

        public static ushort Min(this ushort value, ushort min)
        {
            return DefaultValueHelper.Min(value, min);
        }

        public static ushort Max(this ushort value, ushort max)
        {
            return DefaultValueHelper.Max(value, max);
        }
        
        public static long Min(this long value, long min)
        {
            return DefaultValueHelper.Min(value, min);
        }

        public static long Max(this long value, long max)
        {
            return DefaultValueHelper.Max(value, max);
        }

        public static ulong Min(this ulong value, ulong min)
        {
            return DefaultValueHelper.Min(value, min);
        }

        public static ulong Max(this ulong value, ulong max)
        {
            return DefaultValueHelper.Max(value, max);
        }

        public static float Min(this float value, float min)
        {
            return DefaultValueHelper.Min(value, min);
        }

        public static float Max(this float value, float max)
        {
            return DefaultValueHelper.Max(value, max);
        }

        public static double Min(this double value, double min)
        {
            return DefaultValueHelper.Min(value, min);
        }

        public static double Max(this double value, double max)
        {
            return DefaultValueHelper.Max(value, max);
        }

        public static decimal Min(this decimal value, decimal min)
        {
            return DefaultValueHelper.Min(value, min);
        }

        public static decimal Max(this decimal value, decimal max)
        {
            return DefaultValueHelper.Max(value, max);
        }
    }
}


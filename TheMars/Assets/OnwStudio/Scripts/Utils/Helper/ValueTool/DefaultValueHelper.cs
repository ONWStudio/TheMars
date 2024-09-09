
namespace Onw.Helper
{
    public static class DefaultValueHelper
    {
        public static int GetBitCount(byte value)
        {
            value = (byte)((value & 0x55) + ((value >> 1) & 0x55));
            value = (byte)((value & 0x33) + ((value >> 2) & 0x33));
            value = (byte)((value & 0x0F) + ((value >> 4) & 0x0F));

            return value;
        }
        
        public static int Min(int value, int min)
        {
            return value < min ? min : value;
        }

        public static int Max(int value, int max)
        {
            return value > max ? max : value;
        }

        public static uint Min(uint value, uint min)
        {
            return value < min ? min : value;
        }

        public static uint Max(uint value, uint max)
        {
            return value > max ? max : value;
        }

        public static byte Min(byte value, byte min)
        {
            return value < min ? min : value;
        }

        public static byte Max(byte value, byte max)
        {
            return value > max ? max : value;
        }

        public static sbyte Min(sbyte value, sbyte min)
        {
            return value < min ? min : value;
        }

        public static sbyte Max(sbyte value, sbyte max)
        {
            return value > max ? max : value;
        }

        public static short Min(short value, short min)
        {
            return value < min ? min : value;
        }

        public static short Max(short value, short max)
        {
            return value > max ? max : value;
        }

        public static ushort Min(ushort value, ushort min)
        {
            return value < min ? min : value;
        }

        public static ushort Max(ushort value, ushort max)
        {
            return value > max ? max : value;
        }
        
        public static long Min(long value, long min)
        {
            return value < min ? min : value;
        }

        public static long Max(long value, long max)
        {
            return value > max ? max : value;
        }

        public static ulong Min(ulong value, ulong min)
        {
            return value < min ? min : value;
        }

        public static ulong Max(ulong value, ulong max)
        {
            return value > max ? max : value;
        }

        public static float Min(float value, float min)
        {
            return value < min ? min : value;
        }

        public static float Max(float value, float max)
        {
            return value > max ? max : value;
        }

        public static double Min(double value, double min)
        {
            return value < min ? min : value;
        }

        public static double Max(double value, double max)
        {
            return value > max ? max : value;
        }

        public static decimal Min(decimal value, decimal min)
        {
            return value > min ? min : value;
        }

        public static decimal Max(decimal value, decimal max)
        {
            return value < max ? max : value;
        }
    }
}

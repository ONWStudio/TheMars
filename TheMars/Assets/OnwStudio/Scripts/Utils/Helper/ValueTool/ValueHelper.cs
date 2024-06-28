
namespace Onw.Helpers
{
    public static class ValueHelper
    {
        public static int GetBitCount(byte value)
        {
            value = (byte)((value & 0x55) + ((value >> 1) & 0x55));
            value = (byte)((value & 0x33) + ((value >> 2) & 0x33));
            value = (byte)((value & 0x0F) + ((value >> 4) & 0x0F));

            return value;
        }
    }
}

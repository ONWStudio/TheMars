using System.Collections;
using System.Collections.Generic;
using System;

namespace Onw.Helpers
{
    public static class ParseHelper
    {
        public static bool TryParse<T>(string value, out T result) where T : IComparable, IEquatable<T>
        {
            T defaultValue = default;
            bool isOk = false;

            result = (T)(object)(defaultValue switch
            {
                byte => (isOk = byte.TryParse(value, out byte byteValue)) ? byteValue : default,
                sbyte => (isOk = sbyte.TryParse(value, out sbyte sbyteValue)) ? sbyteValue : default,
                char => (isOk = char.TryParse(value, out char charValue)) ? charValue : default,
                short => (isOk = short.TryParse(value, out short shortValue)) ? shortValue : default,
                ushort => (isOk = ushort.TryParse(value, out ushort ushortValue)) ? ushortValue : default,
                int => (isOk = int.TryParse(value, out int intValue)) ? intValue : default,
                uint => (isOk = uint.TryParse(value, out uint uintValue)) ? uintValue : default,
                float => (isOk = float.TryParse(value, out float floatValue)) ? floatValue : default,
                long => (isOk = long.TryParse(value, out long longValue)) ? longValue : default,
                ulong => (isOk = ulong.TryParse(value, out ulong ulongValue)) ? ulongValue : default,
                double => (isOk = double.TryParse(value, out double doubleValue)) ? doubleValue : default,
                decimal => (isOk = decimal.TryParse(value, out decimal decimalValue)) ? decimalValue : default,
                _ => defaultValue
            });

            return isOk;
        }

        public static T GetTypeToMaxValue<T>(T value) where T : IComparable, IEquatable<T> => (T)(object)(value switch
        {
            byte => byte.MaxValue,
            sbyte => sbyte.MaxValue,
            char => char.MaxValue,
            short => short.MaxValue,
            ushort => ushort.MaxValue,
            int => int.MaxValue,
            uint => uint.MaxValue,
            float => float.MaxValue,
            long => long.MaxValue,
            ulong => ulong.MaxValue,
            double => double.MaxValue,
            decimal => decimal.MaxValue,
            _ => default
        });

        public static T GetTypeToMinValue<T>(T value) where T : IComparable, IEquatable<T> => (T)(object)(value switch
        {
            byte => byte.MinValue,
            sbyte => sbyte.MinValue,
            char => char.MinValue,
            short => short.MinValue,
            ushort => ushort.MinValue,
            int => int.MinValue,
            uint => uint.MinValue,
            float => float.MinValue,
            long => long.MinValue,
            ulong => ulong.MinValue,
            double => double.MinValue,
            decimal => decimal.MinValue,
            _ => default
        });
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiConverter.Common.Utils;

public static class EnumUtils
{
    public static IEnumerable<TEnum> GetValues<TEnum>()
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
    }
}

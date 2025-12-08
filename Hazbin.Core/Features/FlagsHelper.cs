namespace Hazbin.Core.Features;

public static class FlagsHelper
{
    public static bool HaveFlag<T>(T flags, T other) where T : Enum
    {
        ulong flagsValue = Convert.ToUInt64(flags);
        ulong otherValue = Convert.ToUInt64(other);

        return (flagsValue & otherValue) == otherValue;
    }
}
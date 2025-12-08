namespace Hazbin.Core.Features;

public class SnowflakesHelper {
    public static TimeSpan GetTimeSpanFromSnowflake(long snowflake) {
        long timestampMs = snowflake >> 22;

        return TimeSpan.FromMilliseconds(timestampMs);
    }
}
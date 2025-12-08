namespace Hazbin.Core.Features;

public static class RegexHelper {
    public const string Pv4Pattern = @"\b(25[0-5]|2[0-4]\d|[01]?\d\d?)\.(25[0-5]|2[0-4]\d|[01]?\d\d?)\.(25[0-5]|2[0-4]\d|[01]?\d\d?)\.(25[0-5]|2[0-4]\d|[01]?\d\d?)\b";
    public const string UserIdPattern = @"^\d+@[a-z]+$";
}
using System.Text.RegularExpressions;

namespace Hazbin.Core.Features;

public static class HtmlHelper {
    public static string StripHtml(this string raw) => Regex.Replace(raw, "<.*?>", string.Empty);

    public static string Bold(this string raw) => $"<b>{raw}</b>";

    public static string Size(this string raw, int size = 27) => $"<size={size}>{raw}</size>";
}
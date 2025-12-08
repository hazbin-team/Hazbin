using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Hazbin.Core.Extensions;

public static class StringExtensions {
    public static string ReplaceBrackets(this string input) {
        return string.IsNullOrEmpty(input) 
            ? input 
            : input.Replace('[', '(').Replace(']', ')');
    }

    public static string ToCamelCase(this string input) {
        if (string.IsNullOrEmpty(input))
            return input;

        List<string> words = SplitIntoWords(input);
        if (words.Count == 0)
            return string.Empty;

        StringBuilder sb = new StringBuilder(words[0].ToLowerInvariant());
        for (int i = 1; i < words.Count; i++)
        {
            sb.Append(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(words[i]));
        }

        return sb.ToString();
    }

    public static string ToUnderScore(this string input) {
        List<string> words = SplitIntoWords(input);
        return words.Count == 0 ? string.Empty : string.Join("_", words);
    }
    
    private static List<string> SplitIntoWords(this string input) {
        if (string.IsNullOrEmpty(input))
            return new List<string>();

        List<string> words = new List<string>();
        IEnumerable<string> parts = Regex.Split(input, "[^a-zA-Z0-9]+")
            .Where(part => !string.IsNullOrEmpty(part));

        foreach (string? part in parts)
        {
            IEnumerable<string> subWords = Regex.Split(part, "(?<=[a-z0-9])(?=[A-Z])")
                .Where(sub => !string.IsNullOrEmpty(sub))
                .Select(sub => sub.ToLowerInvariant());

            words.AddRange(subWords);
        }

        return words;
    }
    
    public static string InvertCase(this string input) {
        if (string.IsNullOrEmpty(input))
            return input;
    
        StringBuilder sb = new StringBuilder(input.Length);
        foreach (char c in input)
        {
            sb.Append(char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c));
        }
    
        return sb.ToString();
    }
    
    public static string ToPascalCase(this string input) {
        if (string.IsNullOrEmpty(input))
            return input;
    
        List<string> words = SplitIntoWords(input);
        if (words.Count == 0)
            return string.Empty;
    
        StringBuilder sb = new StringBuilder();
        foreach (string? word in words) {
            sb.Append(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(word));
        }
    
        return sb.ToString();
    }
    
    public static string RemoveSpaces(this string input) {
        return string.IsNullOrEmpty(input) ? input : input.Replace(" ", string.Empty);
    }
    
    public static string GetRawUserId(this string userId) {
        int index = userId.IndexOf('@');
        return index == -1 ? userId : userId.Substring(0, index);
    }
}
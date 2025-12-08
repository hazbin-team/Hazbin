namespace Hazbin.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class HarmonyPatchCategory(string category) : Attribute {
    public string Category { get; } = category;
}
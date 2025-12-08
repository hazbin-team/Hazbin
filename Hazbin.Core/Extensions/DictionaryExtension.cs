namespace Hazbin.Core.Extensions;

public static class DictionaryExtension {
    public static TValue? Get<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue? defaultT = default) {
        if (dic.TryGetValue(key, out TValue val))
            return val;
        
        return defaultT;
    }
}
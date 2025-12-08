namespace Hazbin.Core.Extensions;

public static class CollectionExtensions {
    public static T RandomItem<T>(this IEnumerable<T> source) {
        switch (source) {
            case null:
                throw new ArgumentNullException(nameof(source));
            
            case T[] { Length: 0 }:
                throw new InvalidOperationException("Collection is empty");
            
            case T[] array:
                return array[UnityEngine.Random.Range(0, array.Length)];
            
            case List<T> { Count: 0 }:
                throw new InvalidOperationException("Collection is empty");
            
            case List<T> list:
                return list[UnityEngine.Random.Range(0, list.Count)];
        }

        IEnumerable<T> enumerable = source.ToList();
        int count = enumerable.Count();
        
        if (count == 0)
            throw new InvalidOperationException("Collection is empty");
        
        return enumerable.ElementAt(UnityEngine.Random.Range(0, count));
    }
}
using UnityEngine;

namespace Hazbin.Core.Extensions;

public static class RandomExtensions {
    public static T RandomWeight<T>(this Dictionary<T, int> dic, T defaultVal = default!) {
        int sum = dic.Values.Sum();
        int chance = RandomGenerator.GetInt32(1, sum + 1);
        T returnT = defaultVal;
        foreach (KeyValuePair<T, int> kv in dic) {
            if (chance <= kv.Value)
            {
                returnT = kv.Key;
                break;
            }
            chance -= kv.Value;
        }
        return returnT;
    }
    
    public static T RandomWeight<T>(this Dictionary<T, int> dic, Func<KeyValuePair<T, int>, bool> predicate, T defaultVal = default!) {
        return dic.Where(predicate).ToDictionary(x => x.Key, x => x.Value).RandomWeight(defaultVal);
    }

    public static Vector3 RandomVector3(float min, float max) {
        return new(RandomGenerator.GetFloat(min, max), RandomGenerator.GetFloat(min, max), RandomGenerator.GetFloat(min, max));
    }
}
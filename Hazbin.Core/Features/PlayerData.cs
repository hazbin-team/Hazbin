namespace Hazbin.Core.Features;

public class PlayerData {
    private static HashSet<PlayerData> _data = new();
    
    public static void Add(string id, string key, List<object> data) => _data.Add(new PlayerData { UserId = id, Data = new Dictionary<string, List<object>> {{ key, data }}});
        
    public static bool Contains(string id) => _data.Contains(Get(id)!);
    public static PlayerData? Get(string id) => _data.FirstOrDefault(x => x.UserId == id);
        
    public static bool TryGet(string id, out PlayerData? data) {
        data = Get(id);

        return data != null;
    }
    public static void Remove(string id) => _data.Remove(_data.ToList().Find(x => x.UserId == id));

    public string UserId { get; set; } = "";
    public Dictionary<string, List<object>> Data { get; set; } = new();
}
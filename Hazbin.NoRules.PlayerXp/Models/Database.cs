using Hazbin.Core.Enums;
using MEC;
using Newtonsoft.Json;

namespace Hazbin.NoRules.PlayerXp.Models;

internal class Database : IDisposable
{
    private readonly string playersFile;
    private Dictionary<string, float> _players = new();
    private HashSet<Level>? _levels;
    private bool _dataChanged;
    private readonly object _lock = new();

    internal Database(string path)
    {
        string levelsFile1 = Path.Combine(path, "levels.json");
        this.playersFile = Path.Combine(path, "players_xp.data");

        if (!File.Exists(levelsFile1))
        {
            this._levels =
            [
                new() { Text = "example", Color = CustomInfoColor.Aqua, Xp = 0 },
                new() { Text = "example", Color = CustomInfoColor.Aqua, Xp = 1 }
            ];

            using FileStream fs = new(levelsFile1, FileMode.Create);
            using StreamWriter sw = new(fs);
            sw.WriteLine(JsonConvert.SerializeObject(this._levels, Formatting.Indented));
        }
        else
        {
            this._levels = JsonConvert.DeserializeObject<HashSet<Level>>(File.ReadAllText(levelsFile1));
        }
        
        if (File.Exists(this.playersFile))
        {
            using FileStream fs = new(this.playersFile, FileMode.Open);
            using StreamReader sr = new(fs);
            string json = sr.ReadToEnd();
            HashSet<PlayerXp>? playerList = JsonConvert.DeserializeObject<HashSet<PlayerXp>>(json);
            if (playerList != null) this._players = playerList.ToDictionary(p => p.UserID, p => p.Experience);
        }

        Timing.RunCoroutine(this.SaveCoroutine(), "SaveCoroutine");
    }

    private IEnumerator<float> SaveCoroutine()
    {
        for (;;)
        {
            yield return Timing.WaitForSeconds(10);
            
            this.Save();
        }
    }

    private void Save()
    {
        List<PlayerXp> playerList;
        lock (this._lock)
        {
            if (!this._dataChanged) return;
            playerList = this._players.Select(kvp => new PlayerXp { UserID = kvp.Key, Experience = kvp.Value }).ToList();
            this._dataChanged = false;
        }

        using FileStream fs = new(this.playersFile, FileMode.Create);
        using StreamWriter sw = new(fs);
        sw.WriteLine(JsonConvert.SerializeObject(playerList, Formatting.Indented));
    }
    
    public void Dispose()
    {
        Timing.KillCoroutines("SaveCoroutine");
        
        if (this._dataChanged)
        {
            this.Save();
        }
        
        this._levels!.Clear();
        this._players.Clear();
    }

    internal bool Contains(string userid)
    {
        lock (this._lock)
        {
            return this._players.ContainsKey(userid);
        }
    }

    internal PlayerXp? Find(string userid) => this._players.TryGetValue(userid, out float xp) ? new PlayerXp {UserID = userid, Experience = xp} : null;

    internal void Insert(string userid)
    {
        lock (this._lock)
        {
            if (!this._players.ContainsKey(userid))
            {
                this._players[userid] = 0;
                this._dataChanged = true;
            }
        }
    }

    internal void GiveXp(string userid, float exp)
    {
        lock (this._lock)
        {
            if (!this._players.ContainsKey(userid))
            {
                this._players[userid] = 0;
            }
            this._players[userid] += exp;
            this._dataChanged = true;
        }
    }

    internal void SetXp(string userid, float exp)
    {
        lock (this._lock)
        {
            this._players[userid] = exp;
            this._dataChanged = true;
        }
    }

    internal float GetXp(string userid)
    {
        lock (this._lock)
        {
            return this._players.TryGetValue(userid, out float xp) ? xp : 0;
        }
    }

    internal Level? GetLevel(string userid)
    {
        lock (this._lock)
        {
            if (this._players.TryGetValue(userid, out float xp))
            {
                return this._levels!.OrderByDescending(l => l.Xp).FirstOrDefault(level => xp >= level.Xp);
            }
            return null;
        }
    }
}
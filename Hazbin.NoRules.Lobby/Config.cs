using LabApi.Loader.Features.Paths;

namespace Hazbin.NoRules.Lobby;

public class Config {
    public string SoundsPath { get; set; } = $"{PathManager.LabApi.FullName}/Audio/";
}
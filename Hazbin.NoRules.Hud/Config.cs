namespace Hazbin.NoRules.Hud;

public class Config {
    public float UpdateInfoInterval { get; set; } = 8.0f;
    
    public string ServerName { get; set; } = "<color=#FF3B3B>N</color><color=#FD3E35>o</color><color=#FB412F>R</color><color=#F94429>u</color><color=#F74723>l</color><color=#F54A1D>e</color><color=#F34D17>s</color>";

    public List<string> SpectatorInfo { get; set; } = [
        "В нашем дискорде проходит набор в администрацию!",
        "У нас нету классик сервера",
        "Умерев в начале наунда вы можете воскреснуть \nпрописал .res в консоль [~]",
        "SCP-035 спавнится в камере SCP-096",
        "Гранатомёт весит напротив Микро-Хида",
        "Из SCP-330 можно вытенуть розовую конфету",
        "На старом спавне SCP-173 есть казик",
        "Суицид не выход!"
    ];
}
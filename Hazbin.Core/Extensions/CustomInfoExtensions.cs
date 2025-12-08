using Hazbin.Core.Enums;

namespace Hazbin.Core.Extensions;

public static class CustomInfoExtensions {
    public static string GetHexColor(this CustomInfoColor color) => color switch {
        CustomInfoColor.Pink => "#FF96DE",
        CustomInfoColor.Red => "#C50000",
        CustomInfoColor.Brown => "#944710",
        CustomInfoColor.Silver => "#A0A0A0",
        CustomInfoColor.LightGreen => "#32CD32",
        CustomInfoColor.Crimson => "#DC143C",
        CustomInfoColor.Cyan => "#00B7EB",
        CustomInfoColor.Aqua => "#00FFFF",
        CustomInfoColor.DeepPink => "#FF1493",
        CustomInfoColor.Tomato => "#FF6448",
        CustomInfoColor.Yellow => "#FAFF86",
        CustomInfoColor.Magenta => "#FF0090",
        CustomInfoColor.BlueGreen => "#4DFFB8",
        CustomInfoColor.Orange => "#FF9966",
        CustomInfoColor.Lime => "#BFFF00",
        CustomInfoColor.Green => "#228B22",
        CustomInfoColor.Emerald => "#50C878",
        CustomInfoColor.Carmine => "#960018",
        CustomInfoColor.Nickel => "#727472",
        CustomInfoColor.Mint => "#98FB98",
        CustomInfoColor.ArmyGreen => "#4B5320",
        CustomInfoColor.Pumpkin => "#EE7600",
        CustomInfoColor.Black => "#000000",
        CustomInfoColor.White => "#FFFFFF",
        _ => ""
    };
}
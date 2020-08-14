using System.Collections.Generic;

/// <summary>
/// Class with all word-kyes
/// </summary>
[System.Serializable]
public class LocalizationDictionary
{
    public static Dictionary<string, string> localizedMainMenu = new Dictionary<string, string>();
    public static Dictionary<string, string> localizedGame = new Dictionary<string, string>();
}

[System.Serializable]
public class LocalizationData
{
    public LocalizationItem[] items;
}

[System.Serializable]
public class LocalizationItem
{
    public string key;
    public string value;
}
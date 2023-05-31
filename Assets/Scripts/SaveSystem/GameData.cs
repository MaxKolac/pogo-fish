using System;

/// <summary>
/// A "package" class containing data that should remain persistent between multiple runs of the application.
/// </summary>
[Serializable]
public class GameData
{
    public int coinsAmount;
    public int highscore;

    public int skin_default;
    public int skin_blue;
    public int skin_green;

    public int upgradeLvl_springBoost;
    public int upgradeLvl_magnet;

    /// <summary>
    /// Creates a new GameData instance with default values.
    /// </summary>
    public GameData()
    {
        coinsAmount = 0;
        highscore = 0;

        skin_default = (int)SkinStatus.Equipped;
        skin_blue = (int)SkinStatus.Locked;
        skin_green = (int)SkinStatus.Locked;

        upgradeLvl_springBoost = 0;
        upgradeLvl_magnet = 0;
    }

    public override string ToString()
    {
        string s = "";
        s += $"coinsAmount = {coinsAmount}";
        s += $" | highscore = {highscore}";
        s += $" | skin_default = {skin_default}";
        s += $" | skin_blue = {skin_blue}";
        s += $" | skin_green = {skin_green}";
        s += $" | upgradeLvl_springBoost = {upgradeLvl_springBoost}";
        s += $" | upgradeLvl_magnet = {upgradeLvl_magnet}";
        return s;
    }
}
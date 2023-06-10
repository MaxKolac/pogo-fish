using System;

/// <summary>
/// A "package" class containing data that should remain persistent between multiple runs of the application.
/// </summary>
[Serializable]
public class GameData
{
    public float volume;

    public int coinsAmount;
    public int highscore;

    public SkinStatus skin_default;
    public SkinStatus skin_pirate;
    public SkinStatus skin_diver;

    public int upgradeLvl_springBoost;
    public int upgradeLvl_magnet;
    public int upgradeLvl_scoreMultiplier;

    /// <summary>
    /// Creates a new GameData instance with default values.
    /// </summary>
    public GameData()
    {
        volume = 1.0f;

        coinsAmount = 0;
        highscore = 0;

        skin_default = SkinStatus.Equipped;
        skin_pirate = SkinStatus.Locked;
        skin_diver = SkinStatus.Locked;

        upgradeLvl_springBoost = 0;
        upgradeLvl_magnet = 0;
        upgradeLvl_scoreMultiplier = 0;
    }

    public override string ToString()
    {
        string s = "";
        s += $"coinsAmount = {coinsAmount}";
        s += $" | highscore = {highscore}";
        s += $" | skin_default = {skin_default}";
        s += $" | skin_pirate = {skin_pirate}";
        s += $" | skin_diver = {skin_diver}";
        s += $" | upgradeLvl_springBoost = {upgradeLvl_springBoost}";
        s += $" | upgradeLvl_magnet = {upgradeLvl_magnet}";
        s += $" | upgradeLvl_scoreMultiplier = {upgradeLvl_scoreMultiplier}";
        return s;
    }
}
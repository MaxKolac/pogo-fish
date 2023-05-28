using System;

/// <summary>
/// A "package" class containing data that should remain persistent between multiple runs of the application.
/// </summary>
[Serializable]
public class GameData
{
    public int coinsAmount;
    public int highscore;

    public int upgradeLvl_springBoost = 0;

    /// <summary>
    /// Creates a new GameData instance with default values.
    /// </summary>
    public GameData()
    {
        coinsAmount = 0;
        highscore = 0;
        upgradeLvl_springBoost = 0;
    }

    public override string ToString()
    {
        string s = "";
        s += $"coinsAmount = {coinsAmount}";
        s += $" | highscore = {highscore}";
        s += $" | upgradeLvl_springBoost = {upgradeLvl_springBoost}";
        return s;
    }
}
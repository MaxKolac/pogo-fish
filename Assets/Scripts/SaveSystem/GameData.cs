﻿using System;

/// <summary>
/// A "package" class containing data that should remain persistent between multiple runs of the application.
/// </summary>
[Serializable]
public class GameData
{
    public int coinsAmount;
    public int highscore;

    public SkinStatus skin_default;
    public SkinStatus skin_blue;
    public SkinStatus skin_green;

    public int upgradeLvl_springBoost;
    public int upgradeLvl_magnet;
    public int upgradeLvl_scoreMultiplier;

    /// <summary>
    /// Creates a new GameData instance with default values.
    /// </summary>
    public GameData()
    {
        coinsAmount = 0;
        highscore = 0;

        skin_default = SkinStatus.Equipped;
        skin_blue = SkinStatus.Locked;
        skin_green = SkinStatus.Locked;

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
        s += $" | skin_blue = {skin_blue}";
        s += $" | skin_green = {skin_green}";
        s += $" | upgradeLvl_springBoost = {upgradeLvl_springBoost}";
        s += $" | upgradeLvl_magnet = {upgradeLvl_magnet}";
        s += $" | upgradeLvl_scoreMultiplier = {upgradeLvl_scoreMultiplier}";
        return s;
    }
}
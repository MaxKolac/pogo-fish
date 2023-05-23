using System;

/// <summary>
/// A "package" class containing data that should remain persistent between multiple runs of the application.
/// </summary>
[Serializable]
public class GameData
{
    public int deathCount;

    /// <summary>
    /// Creates a new GameData instance with default values.
    /// </summary>
    public GameData()
    {
        deathCount = 0;
    }
}
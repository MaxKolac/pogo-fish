using System;
using UnityEngine;

/// <summary>
/// Interface implemented by classes which hold variables that should be stored in the Save system.
/// </summary>
public interface IDataPersistence
{
    /// <summary>
    /// Implementing class can use this method to reference and read variables from a GameData object.
    /// </summary>
    void LoadData(GameData data);
    /// <summary>
    /// Implementing class can use this method to reference and write variables inside a GameData object. 
    /// </summary>
    void SaveData(ref GameData data);
}
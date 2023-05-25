using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//Shoutout to Trever Mock @ https://www.youtube.com/@TreverMock
//https://www.youtube.com/watch?v=ijVA5Z-Mbh8
public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false;
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption = false;

    public static DataPersistenceManager Instance { get; private set; }

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one DataPersistenceManager in the scene! Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded called. GameData loaded.");
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded called. GameData saved.");
        SaveGame();
    }

    private void OnApplicationQuit() => SaveGame();

    /// <summary>
    /// Initializes manager's GameData field with a new GameData object with default values.
    /// </summary>
    public void NewGame() => gameData = new GameData();

    /// <summary>
    /// <para>
    /// This method attempts to load a savefile through FileDataHandler.Load() method.
    /// If succesful, it iterates over all classes that implement IDataPersistence interface and calls their LoadData() methods.
    /// </para>
    /// <para>
    /// If manager's GameData is null, the NewGame() method needs to be called first for any data to be preserved after the application's closed. The debugging "Initialize Data If Null" toggle forces the manager to call its NewGame() method.
    /// </para>
    /// </summary>
    public void LoadGame()
    {
        gameData = dataHandler.Load();
        if (gameData == null && initializeDataIfNull)
        {
            NewGame();
        }
        if (gameData == null)
        {
            Debug.Log("No game data was found to load. A new game needs to be created before data can be loaded.");
            return;
        }
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        Debug.Log($"Loaded: coinsAmount as {gameData.coinsAmount}");
        Debug.Log($"Loaded: highscore as {gameData.highscore}");
    }

    /// <summary>
    /// <para>
    /// This method iterates over all classes that implement IDataPersistence interface and calls their SaveData() methods. Then, it passes the resulting GameData object to FileDataHandler.
    /// </para>
    /// <para>
    /// If manager's GameData is null, the NewGame() method needs to be called first for any data to be preserved after the application's closed.
    /// </para>
    /// </summary>
    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.Log("No game data to save. A new game needs to be created before data can be saved.");
            return;
        }
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        Debug.Log($"Saved: deathCount as {gameData.coinsAmount}");
        Debug.Log($"Saved: highscore as {gameData.highscore}");
        dataHandler.Save(gameData);
    }

    /// <returns>True, if gameData isn't null. Otherwise, false.</returns>
    public bool HasGameData()
    {
        return gameData != null;
    }

    /// <summary>
    /// Using LINQ, method searches for all classes that implement IDataPersistence interface and returns them as a List.
    /// </summary>
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
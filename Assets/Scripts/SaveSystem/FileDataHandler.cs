using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Class designed for serialization and deserialization of local JSON files into C# objects.
/// </summary>
public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private const string encryptionCodeWord = "bazinga";

    /// <summary>
    /// Creates a new instance of a FileDataHandler with the specified parameters, which together will create a full path to the save file.
    /// </summary>
    /// <param name="dataDirPath">Directory to put the save file in.</param>
    /// <param name="dataFileName">Name of the savefile. Extension is optional.</param>
    /// <param name="useEncryption">If true, the resulting JSON text will be encrypted before being saved as file.</param>
    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    /// <summary>
    /// Attempts to load and deserialize the local JSON file under the previously provided path.
    /// </summary>
    /// <returns>If succesful, a C# GameData object containing data saved in the deserialized file. If not, null is returned instead.</returns>
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) 
                {
                    using StreamReader reader = new StreamReader(stream);
                    dataToLoad = reader.ReadToEnd();
                }
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    /// <summary>
    /// Serializes the provided GameData object into JSON format and saves it as a file under previously provided path.
    /// </summary>
    /// <param name="data">GameData object to serialize.</param>
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using StreamWriter writer = new StreamWriter(stream);
                writer.Write(dataToStore);
            }
        } 
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data in file: " + fullPath + "\n" + e);
        }
    }

    /// <summary>
    /// Simple implementation of XOR encryption by Trever Mock.
    /// </summary>
    /// <param name="data">String variable to encrypt/decrypt.</param>
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}

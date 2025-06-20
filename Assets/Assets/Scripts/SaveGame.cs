using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public static SaveGame Instance { get; private set; }

    public string saveName = "savedGame";
    public string directoryName = "Saves";
    public SaveGameData saveGameData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load(); // Load the save data when the game starts
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        if(!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(directoryName + "/" + saveName + ".bin");
        formatter.Serialize(file, saveGameData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(directoryName + "/" + saveName + ".bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(directoryName + "/" + saveName + ".bin", FileMode.Open);
            if (file.Length == 0)
            {
                Debug.LogWarning("Save file is empty, creating new save data.");
                file.Close();
            }
            else
            {
                saveGameData = (SaveGameData)formatter.Deserialize(file);
                file.Close();
                Debug.Log("Game data loaded successfully.");
                return;
            }
        }
        saveGameData = new SaveGameData();
        saveGameData.deaths = 0;
        saveGameData.level = 0;
        saveGameData.starsPerLevel = new List<int>();
    }
}

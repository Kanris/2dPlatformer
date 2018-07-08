using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveLoadManager {
    
    public static void SavePlayer(string scene) {
        
        using (var stream = new FileStream( GetPathToSaveFile() , FileMode.Create ))
        {
            var binaryFormatter = new BinaryFormatter();
            var dataToSave = new GameData(scene);
            binaryFormatter.Serialize(stream, dataToSave);
        }

    }

    public static GameData LoadPlayer()
    {
        var path = GetPathToSaveFile();
        GameData gameData = null;

        if (!IsFileExists( path ))
        {
            Debug.LogError("SaveLoadManager: Can't load save file");
        }
        else
        {
            using ( var stream = new FileStream(  path, FileMode.Open ) )
            {
                var binaryFormatter = new BinaryFormatter();
                gameData = binaryFormatter.Deserialize(stream) as GameData;
            }
        }

        return gameData;
    }

    public static void DeleteGameData()
    {
        var path = GetPathToSaveFile();

        if (!IsFileExists(path))
        {
            Debug.LogError("SaveLoadManager: Can't delete save file");
        }
        else
        {
            File.Delete(path);
        }
    }

    public static bool IsFileExists(string path)
    {
        return File.Exists(path);
    }

    public static bool IsFileExists()
    {
        var path = GetPathToSaveFile();
        return IsFileExists(path);
    }

    private static string GetPathToSaveFile()
    {
        var path = Application.persistentDataPath + "/player.sav";

        return path;
    }

}

[System.Serializable]
public class GameData
{
    public int Coins;
    public float AdditionalDamage;
    public float DamageResistance;
    public int ResurectionStones;
    public string[] Equipment;

    public string level;

    public GameData() : this (SceneManager.GetActiveScene().name)
    { }

    public GameData(string level)
    {
        Coins = PlayerStats.Coins;
        AdditionalDamage = PlayerStats.AdditionalDamage;
        DamageResistance = PlayerStats.DamageResistance;
        ResurectionStones = PlayerStats.ResurectionStones;
        Equipment = PlayerStats.Equipment;

        this.level = level;
    }
}

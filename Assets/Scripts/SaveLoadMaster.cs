using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadMaster : MonoBehaviour {

    #region Singleton

    public static SaveLoadMaster Instance;

    public void Awake()
    {
        if (Instance != null)
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    #endregion Singleton

    public void SaveGame(string scene)
    {
        SaveLoadManager.SavePlayer(scene);
    }

    public void LoadGame()
    {
        if (!IsSaveFileExists())
            Debug.LogError("SaveLoadMaster: File doesn't exist");
        else
        {
            var gameData = SaveLoadManager.LoadPlayer();

            PlayerStats.Coins = gameData.Coins;
            PlayerStats.AdditionalDamage = gameData.AdditionalDamage;
            PlayerStats.DamageResistance = gameData.DamageResistance;
            PlayerStats.ResurectionStones = gameData.ResurectionStones;
            PlayerStats.Equipment = gameData.Equipment;
            PlayerStats.WeaponSlots = gameData.WeaponSlots;

            LoadScene(gameData.level);
        }
    }

    private void LoadScene(string scene)
    {
        var loadScene = GameObject.FindWithTag("SceneLoader") as GameObject;

        if (loadScene != null)
        {
            var loadSceneInstance = loadScene.GetComponent<LoadScene>();
            loadSceneInstance.Load(scene);
        }
    }

    public bool IsSaveFileExists()
    {
        return SaveLoadManager.IsFileExists();
    }

}

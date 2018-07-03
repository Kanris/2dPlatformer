﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    public Transform playerPrefab;
    private Transform spawnPoint;
    public float spawnDelay = 3.7f;

    public int LifeCount = 2;
    private Transform LifeGUI;
    private Transform[] lifesLeft;

    private AudioManager audioManager;
    public string spawnSoundName;
    public string LevelMusic;
    public string LevelName;

    private GameObject enemiesSpawnPrefab;

    private AnnouncerManager announcer;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        }

        InstantiateManagers();

        InitializeAnnouncer();

        InitalizeAudioManager();

        InitalizeEnemySpawn();

        InitalizeSpawnPoint();

        InitializeLifeGUI();
    }

    private void InstantiateManagers()
    {
        InstantiateManager("Managers/AnnouncerManager");

        InstantiateManager("Managers/AudioManager");

        InstantiateManager("Managers/LoadSceneManager");

        InstantiateManager("Managers/PauseMenuManager");

        InstantiateManager("Managers/ResurectionUI");

        InstantiateManager("Managers/ToolsUI");

        InstantiateManager("Managers/EventSystem");

        InstantiateManager("Managers/A_Path");
    }

    private void InstantiateManager(string resourcePath)
    {
        var manager = Resources.Load(resourcePath) as GameObject;

        Instantiate(manager);
    }

    private void InitializeAnnouncer()
    {
        announcer = AnnouncerManager.instance;

        if (announcer == null)
            Debug.LogError("GameMaster: Can't find AnnouncerManager on scene");
    }

    private void InitalizeSpawnPoint()
    {
        spawnPoint = GameObject.FindWithTag("Respawn").gameObject.transform;

        if (spawnPoint == null)
        {
            Debug.LogError("GameMaster: Can't find spawn point on scene");
        }
    }

    private void InitalizeAudioManager()
    {
        audioManager = AudioManager.instance;

        if (audioManager == null)
            Debug.LogError("GameMaster: Can't found AudioManager");
        else
        {
            AudioManager.ChangeBackgroundMusic(LevelMusic);
        }
    }

    private void InitalizeEnemySpawn()
    {
        enemiesSpawnPrefab = GameObject.FindWithTag("EnemiesSpawn");

        if (enemiesSpawnPrefab != null)
            enemiesSpawnPrefab.SetActive(false);

        if (!string.IsNullOrEmpty(LevelName))
        {
            if (enemiesSpawnPrefab != null)
                StartCoroutine(announcer.DisplayAnnouncerMessage(LevelName, 2f, () => enemiesSpawnPrefab.SetActive(true)));
            else
                StartCoroutine(announcer.DisplayAnnouncerMessage(LevelName, 2f));
        }
    }

    private void InitializeLifeGUI()
    {
        var lifeGUIObject = GameObject.FindWithTag("LifesUI");

        if (lifeGUIObject == null)
        {
            Debug.LogError("GameMaster: Can't find Lifes UI on scene");
        }
        else
        {
            LifeGUI = lifeGUIObject.transform;

            var lifeImage = Resources.Load("GUI/LifeImage") as GameObject;

            if (lifeImage != null)
            {
                lifesLeft = new Transform[LifeCount];

                for (int index = 0; index < LifeCount; index++)
                {
                    lifesLeft[index] = Instantiate(lifeImage.transform, LifeGUI.transform);
                    lifesLeft[index].position = new Vector3(lifesLeft[index].position.x, lifesLeft[index].position.y);
                }
            }
            else
            {
                Debug.LogError("GameMaster: Can't find LifeImage in Resources");
            }
        }
    }

    public IEnumerator RespawnPlayer()
    {
        audioManager.PlaySound(spawnSoundName);
        
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        var spawnPrefab = Resources.Load("Effects/SpawnParticles") as GameObject;
        var spawnEffect = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        Destroy(spawnEffect, 2.5f);
    }

    public void PlayerIsDead()
    {
        if (LifeCount > 0)
        {
            StartCoroutine(RespawnPlayer());
            LifeCount--;
            ChangeLifeGUI();

            string result = LifeCount > 1 ? "Lifes" : "Life";
            var announcerMessage = LifeCount + " " + result + " left";
            StartCoroutine(announcer.DisplayAnnouncerMessage(announcerMessage, 3f));
        }
        else
        {
            if (PlayerStats.ResurectionStones > 0)
            {
                ResurectionManager.instance.ShowHideResurectionUI();
            }
            else
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0.1f;
        gm.StartCoroutine(announcer.DisplayAnnouncerMessage("Game Over", 0.5f));
        gm.StartCoroutine(LoadScene("MainMenu", 0.5f));
    }

    public IEnumerator LoadScene(string scene, float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 1f;

        GameObject.FindWithTag("SceneLoader").GetComponent<LoadScene>().Load(scene);
    }

    private void ChangeLifeGUI()
    {
        Destroy(lifesLeft[LifeCount].gameObject);
    }

}

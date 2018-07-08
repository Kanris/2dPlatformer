using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    
    public string LevelMusic;

    public string scene = "Main";

    private AudioManager audioManager;

    private void Awake()
    {
        InstantiateManagers();
    }

    private void Start()
    {
        InitalizeLoadButton();
    }

    private void InitalizeLoadButton()
    {
        if (!SaveLoadManager.IsFileExists())
        {
            GameObject.FindWithTag("LoadButton").SetActive(false);
        }
    }

    private void InstantiateManagers()
    {
        InstantiateManager("Managers/AudioManager");

        InstantiateManager("Managers/LoadSceneManager");

        InstantiateManager("Managers/ToolsUI");

        InstantiateManager("Managers/EventSystem");
    }

    private void InstantiateManager(string resourcePath)
    {
        var manager = Resources.Load(resourcePath) as GameObject;
        
        Instantiate(manager);         
    }

    private void InitializeAudiomanager()
    {
        audioManager = AudioManager.instance;

        if (audioManager != null)
        {
            AudioManager.ChangeBackgroundMusic(LevelMusic);
        }
        else
            Debug.LogError("MainMenu: Can't find AudioManager on MainMenu scene");
    }

    public void PlayGame()
    {
        var loadScene = GameObject.FindWithTag("SceneLoader") as GameObject;

        if (loadScene != null)
        {
            var loadSceneInstance = loadScene.GetComponent<LoadScene>();
            loadSceneInstance.Load(scene);
        }
        else
            Debug.LogError("MainMenu: Can't find LoadScene on MainMenu scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
	
}

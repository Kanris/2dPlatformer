using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu pm;

    #region Singleton

    private void Awake()
    {
        if (pm != null)
        {
            if (pm != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            pm = this;
            DontDestroyOnLoad(this);
        }
    }

    #endregion 

    private GameObject pauseMenu;
    private bool isGamePaused = false;

    private void Start()
    {
        pauseMenu = transform.GetChild(0).gameObject;

        if (pauseMenu == null)
        {
            Debug.LogError("PauseMenu: Can't find PauseMenu in child.");
        }
    }

    public bool IsGamePause 
    {
        get
        {
            return isGamePaused;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOrResume();
        }
    }

    public void PauseOrResume()
    {
        isGamePaused = !isGamePaused;

        ShowHidePauseMenu();

        float pauseValue = isGamePaused ? 0f : 1f;

        Time.timeScale = pauseValue;
    }

    private void ShowHidePauseMenu()
    {
        if (pauseMenu != null)
        {
            pauseMenu.gameObject.SetActive(isGamePaused);
        }
        else
        {
            Debug.LogError("PauseMenu: Can't find pauseMenu Transform");
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        var loadScene = (FindObjectOfType(typeof(LoadScene)) as LoadScene);

        if (loadScene != null)
        {
            loadScene.Load("MainMenu");
        }
        else
        {
            Debug.LogError("PauseMenu: Can't find LoadScene in Scene.");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

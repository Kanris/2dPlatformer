using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu pm;

    #region Singleton

    private void Awake()
    {
        if (pm == null)
        {
            pm = this;
        }
    }

    #endregion 
    public Transform pauseMenu;
    private bool isGamePaused = false;

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
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject loadingScene;
    public Slider slider;

    public string LevelMusic;

    public string scene = "Main";

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;

        if (audioManager != null)
        {
            AudioManager.ChangeBackgroundMusic(LevelMusic);
        }
    }

    public void PlayGame()
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneAsync(string scene)
    {
        var operation = SceneManager.LoadSceneAsync(scene);

        loadingScene.SetActive(true);

        while (!operation.isDone)
        {
            var progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;

            yield return null;
        }
    }
	
}

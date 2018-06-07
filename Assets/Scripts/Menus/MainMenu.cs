using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject loadingScene;
    public Slider slider;

    public string scene = "Main";

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

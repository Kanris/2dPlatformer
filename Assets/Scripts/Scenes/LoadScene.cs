using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    private GameObject loadingScene;
    private Slider slider;

    private void Start()
    {
        loadingScene = transform.GetChild(0).gameObject;

        if (loadingScene == null)
        {
            Debug.LogError("LoadScene: Can't find LoadingScene in child.");
        }
    }

    public void Load(string scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    private IEnumerator LoadSceneAsync(string scene)
    {
        var operation = SceneManager.LoadSceneAsync(scene);

        InitializeLoadingScene();

        while (!operation.isDone)
        {
            var progress = Mathf.Clamp01(operation.progress / .9f);

            if (slider != null) slider.value = progress;

            yield return null;
        }
    }

    private void InitializeLoadingScene()
    {
        if (loadingScene != null)
        {
            loadingScene.SetActive(true);

            slider = loadingScene.GetComponentInChildren<Slider>();

            if (slider == null)
            {
                Debug.LogError("LoadScene: Can't find slider in LoadingScene.");
            }
        }
    }
}

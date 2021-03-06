﻿using System.Collections;
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
        else
        {
            loadingScene.SetActive(false);
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

        SaveGame(scene);
            
        while (!operation.isDone)
        {
            var progress = Mathf.Clamp01(operation.progress / .9f);

            if (slider != null) slider.value = progress;

            yield return null;
        }

        if (loadingScene != null)
            loadingScene.SetActive(false);
    }

    private void SaveGame(string scene)
    {
        SaveLoadMaster.Instance.SaveGame(scene);
    }

    private void InitializeLoadingScene()
    {
        if (loadingScene != null)
        {
            loadingScene.SetActive(true);

            slider = gameObject.transform.GetChild(0).GetComponentInChildren<Slider>();
                               
            if (slider == null)
            {
                Debug.LogError("LoadScene: Can't find slider in LoadingScene.");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    private int wavesCount = 0;
    private int wavesCompleted = 0;
    private float updateRate = 5f;

    [SerializeField]
    private Text TextLevelCompletion;

    public string NextScene;

    private void Start()
    {
        InitializeWavesCompleted();
        InitializeTextLevelCompletion();
    }

    private void InitializeWavesCompleted()
    {
        for (int index = 0; index < gameObject.transform.childCount - 1; index++)
        {
            var enemySpawn = gameObject.transform.GetChild(index).GetComponent<EnemySpawn>();
            wavesCount += enemySpawn.WaveCount;
        }

        wavesCompleted -= gameObject.transform.childCount - 1;
    }

    private void InitializeTextLevelCompletion()
    {
        if (TextLevelCompletion != null)
            TextLevelCompletion.text = "Level completion:0%";
        else
            Debug.LogError("LevelManager: Can't find level completion Text UI");
    }

    public void WaveCompleted()
    {
        wavesCompleted += 1;

        float waveCompletedPercent = Mathf.Round((float)wavesCompleted / (float)wavesCount * 100f);

        TextLevelCompletion.text = "Level completion:" + waveCompletedPercent + "%";

        if (waveCompletedPercent >= 100f)
        {
            LevelOver();
        }
            
    }

    private void LevelOver()
    {
        StartCoroutine(GameMaster.gm.LoadScene(NextScene, 0f));
    }



}

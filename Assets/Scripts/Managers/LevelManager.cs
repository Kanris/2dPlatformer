using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    private int wavesCount = 0;
    private int wavesCompleted = 0;
    private float updateRate = 5f;

    private bool isLevelOver = false;

    public Text TextLevelCompletion;

    public string NextScene;

    private void Start()
    {
        for (int index = 0; index < gameObject.transform.childCount - 1; index++)
        {
            var enemySpawn = gameObject.transform.GetChild(index).GetComponent<EnemySpawn>();
            wavesCount += enemySpawn.waves.Length;
        }

        wavesCompleted -= gameObject.transform.childCount - 1;

        TextLevelCompletion.text = "Level completion:0%";
    }

    private void Update()
    {
        if (Time.time >= updateRate)
        {
            updateRate = Time.time + 5f;
            IsLevelOver();
        }
    }

    public void WaveCompleted()
    {
        wavesCompleted += 1;

        float waveCompletedPercent = Mathf.Round((float)wavesCompleted / (float)wavesCount * 100f);

        TextLevelCompletion.text = "Level completion:" + waveCompletedPercent + "%";
    }

    private void IsLevelOver()
    {
        if (gameObject.transform.childCount <= 1)
        {
            isLevelOver = true;
            updateRate = 10f;
            StartCoroutine(GameMaster.gm.DisplayAnnouncerMessage("Level complete", updateRate));
            TextLevelCompletion.text = string.Empty;

            StartCoroutine(GameMaster.gm.LoadScene(NextScene, 0f));
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    private int wavesCount = 0;
    private int wavesCompleted = 0;
    private float updateRate = 2f;

    private bool isLevelOver = false;

    public Text TextLevelCompletion;

    private void Start()
    {
        for (int index = 0; index < gameObject.transform.childCount; index++)
        {
            var enemySpawn = gameObject.transform.GetChild(index).GetComponent<EnemySpawn>();
            wavesCount += enemySpawn.waves.Length;
        }

        wavesCompleted -= gameObject.transform.childCount;

        StartCoroutine(IsLevelOver());
    }

    public void WaveCompleted()
    {
        wavesCompleted += 1;

        float waveCompletedPercent = (float)wavesCompleted / (float)wavesCount * 100f;

        TextLevelCompletion.text = "Level completion:" + waveCompletedPercent + "%";
    }

    private IEnumerator IsLevelOver()
    {
        if (gameObject.transform.childCount == 0)
        {
            isLevelOver = true;
            updateRate = 10f;
            StartCoroutine(GameMaster.gm.DisplayAnnouncerMessage("Level complete", updateRate));
            TextLevelCompletion.text = string.Empty;
        }

        yield return new WaitForSeconds(updateRate);

        if (!isLevelOver)
            StartCoroutine(IsLevelOver());
        else
            Destroy(gameObject);
    }

}

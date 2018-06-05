using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    private int wavesCount = 0;
    private EnemySpawn[] enemySpawns;
    private float updateRate = 5f;

    private bool isLevelOver = false;

    private void Start()
    {
        enemySpawns = new EnemySpawn[gameObject.transform.childCount];

        for (int index = 0; index < gameObject.transform.childCount; index++)
        {
            enemySpawns[index] = gameObject.transform.GetChild(index).GetComponent<EnemySpawn>();
            wavesCount += enemySpawns[index].waves.Length;
        }

        StartCoroutine(IsLevelOver());
    }

    private IEnumerator IsLevelOver()
    {
        if (gameObject.transform.childCount == 0)
        {
            isLevelOver = true;
            updateRate = 10f;
            StartCoroutine(GameMaster.gm.DisplayAnnouncerMessage("Level complete", updateRate));

        }

        yield return new WaitForSeconds(updateRate);

        if (!isLevelOver)
            StartCoroutine(IsLevelOver());
        else
            Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    private int m_wavesCount = 0;
    private int m_waveCompleted = 0;
    private float m_updateRate = 5f;

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
            m_wavesCount += enemySpawn.WaveCount;
        }

        m_waveCompleted -= gameObject.transform.childCount - 1;
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
        m_waveCompleted += 1;

        float waveCompletedPercent = Mathf.Round((float)m_waveCompleted / (float)m_wavesCount * 100f);

        TextLevelCompletion.text = "Level completion:" + waveCompletedPercent + "%";

        if (waveCompletedPercent >= 100f)
        {
            LevelOver();
        }
            
    }

    private void LevelOver()
    {
        if (!string.IsNullOrEmpty(NextScene))
            StartCoroutine(GameMaster.gm.LoadScene(NextScene, 0f));
        else
            Debug.LogError("LevelManager: Can't find scene to load.");
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    [System.Serializable]
    public class Wave
    {
        public string Name;
        public EnemyType EnemyToSpawn;
        public int Count;

        public enum EnemyType { Kamikaze, Boss };
    }

    [SerializeField]
    private Wave[] waves;
    public int WaveCount { get { return waves.Length; } }

    private int m_currentWave = 0;
    public int CurrentWave { get { return m_currentWave; } }

    private float m_timeNextCheck = 2f;
    private float m_rateForNextWaveCheck = 2f;

    private LevelManager m_levelManager;

    private void Start()
    {
        m_levelManager = gameObject.transform.parent.GetComponent<LevelManager>();
    }

    private void Update()
    {
        if (m_timeNextCheck <= Time.time)
        {
            m_timeNextCheck = Time.time + m_rateForNextWaveCheck;

            if (gameObject.transform.childCount == 0)
            {
                if (m_currentWave < waves.Length)
                {
                    SpawnEnemies();
                    m_currentWave++;
                }
                else
                    Destroy(gameObject);

                m_levelManager.WaveCompleted();
            }
        }
    }

    private void SpawnEnemies()
    {
        var enemyToSpawn = GetEnemy( waves[m_currentWave].EnemyToSpawn );

        for (int index = 0; index < waves[m_currentWave].Count; index++)
        {
            var spawnPosition = GetRandomSpawnPoint();


            var enemy = Instantiate(enemyToSpawn, gameObject.transform);
            enemy.transform.position = spawnPosition;
        }
    }

    private Transform GetEnemy(Wave.EnemyType enemyType)
    {
        Transform returnEnemy = null;

        switch (enemyType)
        {
            case Wave.EnemyType.Kamikaze:
                returnEnemy = GetObjectFromResources("EnemyAlienShip").transform;
                break;

            case Wave.EnemyType.Boss:
                returnEnemy = GetObjectFromResources("BossAlienShip").transform;
                break;
        }

        return returnEnemy;
    }

    private GameObject GetObjectFromResources(string name)
    {
        var returnGameObject = Resources.Load("Enemies/" + name) as GameObject;

        return returnGameObject;
    }

    private Vector3 GetRandomSpawnPoint()
    {
        var offsetX = Random.Range(-1.0f, 1.0f);
        var offsetY = Random.Range(-1.0f, 1.0f);

        var parrentPosition = gameObject.transform.position;

        return new Vector3(parrentPosition.x + offsetX, parrentPosition.y + offsetY);
    }
}

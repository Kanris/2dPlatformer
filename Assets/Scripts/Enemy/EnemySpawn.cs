using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemyToSpawn;
        public int count;
    }

    public Wave[] waves;
    private int currentWave = 0;
    public int CurrentWave { get { return currentWave; } }
    private float rateForNextWaveCheck = 5f;

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        if (gameObject.transform.childCount == 0)
        {
            if (currentWave < waves.Length)
            {
                SpawnEnemies();
                currentWave++;
            }
            else
            {                
                Destroy(gameObject);
            }
        }

        yield return new WaitForSeconds(rateForNextWaveCheck);

        StartCoroutine(SpawnWave());
    }

    private void SpawnEnemies()
    {
        var enemyToSpawn = waves[currentWave].enemyToSpawn;

        for (int index = 0; index < waves[currentWave].count; index++)
        {
            var spawnPosition = GetRandomSpawnPoint();
            var enemy = Instantiate(enemyToSpawn, gameObject.transform);
            enemy.transform.position = spawnPosition;
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        var offsetX = Random.Range(-1.0f, 1.0f);
        var offsetY = Random.Range(-1.0f, 1.0f);

        var parrentPosition = gameObject.transform.position;

        return new Vector3(parrentPosition.x + offsetX, parrentPosition.y + offsetY);
    }
}

﻿using System.Collections;
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
    private float rateForNextWaveCheck = 2f;

    private LevelManager levelManager;

    private void Start()
    {
        levelManager = gameObject.transform.parent.GetComponent<LevelManager>();
    }

    private void Update()
    {
        if (rateForNextWaveCheck <= Time.time)
        {
            rateForNextWaveCheck = Time.time + 2f;   
            if (gameObject.transform.childCount == 0)
            {
                if (currentWave < waves.Length)
                {
                    SpawnEnemies();
                    currentWave++;
                    levelManager.WaveCompleted();
                }
                else
                {
                    levelManager.WaveCompleted();
                    Destroy(gameObject);
                }
            }
        }
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [Serializable] struct Wave
    {
        public float timeSincePreviousWave;
        public int enemyNumber;
        public EnemyStats enemyType;
    }

    [SerializeField] private Wave[] _waves;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private PlayerData _playerData;

    private float _timeOfLastWave;
    private int _nextWaveIndex;

    private void Start()
    {
        StartCoroutine(WaveSpawnCoroutine());
    }

    private IEnumerator WaveSpawnCoroutine()
    {
        while (_nextWaveIndex < _waves.Length)
        {
            SpawnWaveIfReady();
            yield return null;
        }
    }

    private void SpawnWaveIfReady()
    {
        var wave = _waves[_nextWaveIndex];

        if (wave.enemyNumber > _spawnPoints.Length)
        {
            print("More enemies than spawn points in wave " + _nextWaveIndex + ". Ignoring wave.");
            _nextWaveIndex++;
            return;
        }

        if (Time.time < wave.timeSincePreviousWave + _timeOfLastWave)
        {
            return;
        }

        var bestSpawnPoints = SelectBestSpawnPoints(wave);

        foreach (var point in bestSpawnPoints)
        {
            if (float.IsPositiveInfinity(WeightSpawnPoint(point, wave.enemyType)))
            {
                print("SpawnPoint " + point + " was counted as invalid and ignored. No enemy spawned.");
                continue;
            }
            
            EnemyPoolController.CurrentEnemyPoolController.SpawnEnemy(wave.enemyType, point, Quaternion.identity);
        }

        _nextWaveIndex++;
    }

    private Vector3[] SelectBestSpawnPoints(Wave wave)
    {
        //Selects the best spawn points based on the weighting in WeightSpawnPoint
        var bestPoints = new Vector3[wave.enemyNumber];
        var lowestWeights = new float[wave.enemyNumber];
        for (var index = 0; index < lowestWeights.Length; index++)
        {
            lowestWeights[index] = float.PositiveInfinity;
        }

        foreach (var currentPoint in _spawnPoints)
        {
            var currentWeight = WeightSpawnPoint(currentPoint.position, wave.enemyType);

            //Find the current worst point
            var worstIndex = 0;
            for (int i = 1; i < bestPoints.Length; i++)
            {
                if (WeightSpawnPoint(bestPoints[i], wave.enemyType) > WeightSpawnPoint(bestPoints[worstIndex], wave.enemyType))
                {
                    worstIndex = i;
                }
            }

            //Replace the worst of the best points if the current point is better
            if (currentWeight < WeightSpawnPoint(bestPoints[worstIndex], wave.enemyType))
            {
                lowestWeights[worstIndex] = currentWeight;
                bestPoints[worstIndex] = currentPoint.position;
            }
        }

        return bestPoints;
    }

    private float WeightSpawnPoint(Vector3 spawnPoint, EnemyStats enemyType)
    {
        //Returns low weight for good spawn points
        var distance = Vector3.Distance(spawnPoint, _playerData.position);
        
        if (distance <= enemyType.attackRange)
        {
            return float.PositiveInfinity;
        }

        return Mathf.Abs(2 * enemyType.attackRange - distance);
    }
}
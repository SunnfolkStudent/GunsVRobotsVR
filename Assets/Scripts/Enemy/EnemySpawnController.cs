using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [Serializable] struct Wave
    {
        public int enemyNumber;
        public GameObject enemyType;
        public Transform[] overrideSpawnPoints;
        public float preferredDistanceFromPlayer;
    }
    
    [SerializeField] private Wave[] _waves;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private bool spawnAutomatically = true;
    
    private int _nextWaveIndex;

    private void Start()
    {
        StartSpawningFromStart();
    }

    public void StartSpawningFromStart()
    {
        _nextWaveIndex = 0;
        
        StopAllCoroutines();
        
        if (spawnAutomatically)
        {
            StartCoroutine(WaveSpawnCoroutine());
        }
    }

    private IEnumerator WaveSpawnCoroutine()
    {
        while (_nextWaveIndex < _waves.Length)
        {
            yield return null;
            
            var wave = _waves[_nextWaveIndex];

            if (wave.enemyNumber > _spawnPoints.Length)
            {
                print("More enemies than spawn points in wave " + _nextWaveIndex + ". Ignoring wave.");
                _nextWaveIndex++;
                continue;
            }

            if (EnemyPoolController.CurrentEnemyPoolController.activeEnemies.Count > 2)
            {
                continue;
            }
            
            SpawnWave();
        }
    }

    public void SpawnWave()
    {
        if (_nextWaveIndex >= _waves.Length)
        {
            print("Unable to spawn new wave due to the list of being exhausted.");
            return;
        }
        
        var wave = _waves[_nextWaveIndex];

        if (wave.overrideSpawnPoints.Length == 0)
        {
            var bestSpawnPoints = SelectBestSpawnPoints(wave);

            foreach (var point in bestSpawnPoints)
            {
                if (float.IsPositiveInfinity(WeightSpawnPoint(point, wave.preferredDistanceFromPlayer)))
                {
                    print("SpawnPoint " + point + " was counted as invalid and ignored. No enemy spawned.");
                    continue;
                }
            
                EnemyPoolController.CurrentEnemyPoolController.SpawnEnemy(wave.enemyType, point, Quaternion.identity);
            }
        }
        else
        {
            foreach (var point in wave.overrideSpawnPoints)
            {
                if (float.IsPositiveInfinity(WeightSpawnPoint(point.position, wave.preferredDistanceFromPlayer)))
                {
                    print("SpawnPoint " + point.position + " was counted as invalid and ignored. No enemy spawned.");
                    continue;
                }
            
                EnemyPoolController.CurrentEnemyPoolController.SpawnEnemy(wave.enemyType, point.position, Quaternion.identity);
            }
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
            var currentWeight = WeightSpawnPoint(currentPoint.position, wave.preferredDistanceFromPlayer);

            //Find the current worst point
            var worstIndex = 0;
            for (int i = 1; i < bestPoints.Length; i++)
            {
                if (WeightSpawnPoint(bestPoints[i], wave.preferredDistanceFromPlayer) > WeightSpawnPoint(bestPoints[worstIndex], wave.preferredDistanceFromPlayer))
                {
                    worstIndex = i;
                }
            }

            //Replace the worst of the best points if the current point is better
            if (currentWeight < WeightSpawnPoint(bestPoints[worstIndex], wave.preferredDistanceFromPlayer))
            {
                lowestWeights[worstIndex] = currentWeight;
                bestPoints[worstIndex] = currentPoint.position;
            }
        }

        return bestPoints;
    }

    private float WeightSpawnPoint(Vector3 spawnPoint, float preferredDistance)
    {
        //Returns low weight for good spawn points
        var distance = Vector3.Distance(spawnPoint, _playerData.position);
        
        if (distance <= preferredDistance)
        {
            return float.PositiveInfinity;
        }

        return Mathf.Abs(preferredDistance - distance);
    }
}
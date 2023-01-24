using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [Serializable] struct Wave
    {
        public EnemySpawn[] enemies;
        /*public GameObject enemyType;
        public Transform[] overrideSpawnPoints;
        public float preferredDistanceFromPlayer;*/
    }
    
    [Serializable] struct EnemySpawn
    {
        public int enemyNumber;
        public GameObject enemyType;
        public Transform[] overrideSpawnPoints;
        public float preferredDistanceFromPlayer;
    }

    [SerializeField] private float _timeToWaitBeforeSpawningFirstEnemy;
    [SerializeField] private float _timeToWaitAfterLastEnemyKilled;
    [SerializeField] private Wave[] _waves;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private bool spawnAutomatically = true;

    [Header("Sentries")]
    public GameObject sentryPrefab;
    public Transform[] sentrySpawnPoints;
    public List<GameObject> activeSentries;
    
    private int _nextWaveIndex;

    private void Start()
    {
        StartSpawningFromStart();
    }

    public void StartSpawningFromStart()
    {
        StopAllCoroutines();
        
        if (activeSentries.Count != 0)
        {
            foreach (var sentry in activeSentries.ToList())
            {
                Destroy(sentry);
            }
            
            activeSentries.Clear();
        }

        foreach (var spawnPoint in sentrySpawnPoints)
        {
            activeSentries.Add(Instantiate(sentryPrefab, spawnPoint.position, spawnPoint.rotation));
        }

        _nextWaveIndex = 0;

        if (spawnAutomatically)
        {
            StartCoroutine(WaveSpawnCoroutine());
        }
    }

    private IEnumerator WaveSpawnCoroutine()
    {
        yield return new WaitForSeconds(_timeToWaitBeforeSpawningFirstEnemy);
        
        while (_nextWaveIndex < _waves.Length)
        {
            yield return null;
            
            var wave = _waves[_nextWaveIndex];

            var sum = 0;
            foreach (var enemySpawn in wave.enemies)
            {
                sum += enemySpawn.enemyNumber;
            }
            if (sum > _spawnPoints.Length)
            {
                print("More enemies than spawn points in wave " + _nextWaveIndex + ". Ignoring wave.");
                _nextWaveIndex++;
                continue;
            }

            if (EnemyPoolController.CurrentEnemyPoolController.activeEnemies.Count > 2)
            {
                continue;
            }
            
            yield return SpawnWave();
        }

        while (EnemyPoolController.CurrentEnemyPoolController.activeEnemies.Count > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(_timeToWaitAfterLastEnemyKilled);
        
        GameObject.Find("GameManager").GetComponent<GameManager>().SpawnNextLevelTrigger();
    }

    public IEnumerator SpawnWave()
    {
        if (_nextWaveIndex >= _waves.Length)
        {
            print("Unable to spawn new wave due to the list of waves being exhausted.");
            yield break;
        }
        
        var wave = _waves[_nextWaveIndex];

        foreach (var enemyType in wave.enemies)
        {
            if (enemyType.overrideSpawnPoints.Length == 0)
            {
                var bestSpawnPoints = SelectBestSpawnPoints(enemyType);

                foreach (var point in bestSpawnPoints)
                {
                    if (float.IsPositiveInfinity(WeightSpawnPoint(point, enemyType.preferredDistanceFromPlayer)))
                    {
                        print("SpawnPoint " + point + " was counted as invalid and ignored. No enemy spawned.");
                        continue;
                    }
            
                    EnemyPoolController.CurrentEnemyPoolController.SpawnEnemy(enemyType.enemyType, point, Quaternion.identity);
                }
            }
            else
            {
                foreach (var point in enemyType.overrideSpawnPoints)
                {
                    if (float.IsPositiveInfinity(WeightSpawnPoint(point.position, enemyType.preferredDistanceFromPlayer)))
                    {
                        print("SpawnPoint " + point.position + " was counted as invalid and ignored. No enemy spawned.");
                        continue;
                    }
            
                    EnemyPoolController.CurrentEnemyPoolController.SpawnEnemy(enemyType.enemyType, point.position, Quaternion.identity);
                }
            }

            yield return new WaitForSeconds(0.2f);
        }

        _nextWaveIndex++;
    }
    
    private Vector3[] SelectBestSpawnPoints(EnemySpawn enemyType)
    {
        //Selects the best spawn points based on the weighting in WeightSpawnPoint
        var bestPoints = new Vector3[enemyType.enemyNumber];
        var lowestWeights = new float[enemyType.enemyNumber];
        for (var index = 0; index < lowestWeights.Length; index++)
        {
            lowestWeights[index] = float.PositiveInfinity;
        }

        foreach (var currentPoint in _spawnPoints)
        {
            var currentWeight = WeightSpawnPoint(currentPoint.position, enemyType.preferredDistanceFromPlayer);

            //Find the current worst point
            var worstIndex = 0;
            for (int i = 1; i < bestPoints.Length; i++)
            {
                if (WeightSpawnPoint(bestPoints[i], enemyType.preferredDistanceFromPlayer) > WeightSpawnPoint(bestPoints[worstIndex], enemyType.preferredDistanceFromPlayer))
                {
                    worstIndex = i;
                }
            }

            //Replace the worst of the best points if the current point is better
            if (currentWeight < WeightSpawnPoint(bestPoints[worstIndex], enemyType.preferredDistanceFromPlayer))
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
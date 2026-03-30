using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave 
{
    public int totalSpawnEnemies;
    public int numberOfRandomSpawnPoint;
    public float delayStart;
    public float spawnInterval;
    public int numberOfPowerUp;
}

public class WaveSpawner : MonoBehaviour
{
    
    public Wave[] waves; 
    
    
    public Transform[] spawnPoints; 
    
    public GameObject enemyPrefab;
    public GameObject powerUpPrefab; 

    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        
        enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            Debug.Log($"เริ่ม Wave {currentWaveIndex + 1}");

            
            for (int i = 0; i < currentWave.numberOfPowerUp; i++)
            {
                SpawnRandomPowerUp();
            }

            
            List<Transform> selectedPoints = GetRandomSpawnPoints(currentWave.numberOfRandomSpawnPoint);

            
            yield return new WaitForSeconds(currentWave.delayStart);

            
            for (int i = 0; i < currentWave.totalSpawnEnemies; i++)
            {
                
                Transform sp = selectedPoints[Random.Range(0, selectedPoints.Count)];
                Instantiate(enemyPrefab, sp.position, Quaternion.identity);
                
                
                yield return new WaitForSeconds(currentWave.spawnInterval);
            }

            
            while (enemiesAlive > 0)
            {
                yield return null; 
            }

            
            currentWaveIndex++;
            Debug.Log("Wave has been cleared!");
        }

        Debug.Log("All waves have now concluded!");
    }

    
    List<Transform> GetRandomSpawnPoints(int count)
    {
        List<Transform> available = new List<Transform>(spawnPoints);
        List<Transform> selected = new List<Transform>();
        
        int loopCount = Mathf.Min(count, available.Count);
        for (int i = 0; i < loopCount; i++)
        {
            int rand = Random.Range(0, available.Count);
            selected.Add(available[rand]);
            available.RemoveAt(rand); 
        }
        return selected;
    }

    
    void SpawnRandomPowerUp()
    {
        
        Vector3 randomPos = new Vector3(Random.Range(-9f, 9f), 0.5f, Random.Range(-9f, 9f));
        Instantiate(powerUpPrefab, randomPos, Quaternion.identity);
    }
}
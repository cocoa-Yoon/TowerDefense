using System.Collections;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject monsterPrefab;
    public int count = 10;
    public float spawnInterval = 0.5f;
}

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    public Transform startPoint;

    [Header("Wave Settings")]
    public Wave[] waves;
    public float timeBetweenWaves = 3f;

    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (currentWaveIndex < waves.Length)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            currentWaveIndex++;

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("모든 웨이브 종료");
    }

    IEnumerator SpawnWave(Wave wave)
    {
        isSpawning = true;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnMonster(wave.monsterPrefab);
            yield return new WaitForSeconds(wave.spawnInterval);
        }

        isSpawning = false;
    }

    void SpawnMonster(GameObject monsterPrefab)
    {
        Instantiate(monsterPrefab, startPoint.position, Quaternion.identity);
    }
}
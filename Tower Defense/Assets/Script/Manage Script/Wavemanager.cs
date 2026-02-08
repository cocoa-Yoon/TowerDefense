using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Monster Prefabs")]
    public GameObject[] normalMonsters;
    public GameObject[] specialMonsters;

    [Header("Spawn")]
    public Transform spawnPoint;
    public float spawnInterval;

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public Button nextWaveButton;
    public GameObject disableOverlay; // ⭐ 추가

    int currentWave = 0;
    int aliveMonsterCount = 0;
    bool isSpawning = false;

    void Awake()
    {
        Instance = this;
        SetButtonState(true);
    }

    public void StartNextWave()
    {
        Debug.Log("StartNextWave called");
        if (isSpawning) return;

        currentWave++;
        waveText.text = $"Wave : {currentWave}";

        SetButtonState(false);
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        isSpawning = true;

        spawnInterval = Mathf.Max(0.3f, 2.0f - (currentWave * 0.1f));

        int normalCount = 3 + currentWave;
        int specialCount = 0;

        bool isSpecialWave =
            currentWave >= 10 &&
            (currentWave == 10 || (currentWave - 10) % 5 == 0);

        if (isSpecialWave)
        {
            normalCount /= 2;
            specialCount = (currentWave - 5) / 5;
        }

        int total = normalCount + specialCount;

        for (int i = 0; i < total; i++)
        {
            bool spawnSpecial =
                specialCount > 0 &&
                (normalCount == 0 || Random.value < 0.5f);

            if (spawnSpecial)
            {
                SpawnSpecial(specialMonsters);
                specialCount--;
            }
            else
            {
                SpawnNormal(normalMonsters);
                normalCount--;
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
        CheckWaveEnd();
    }

    void SpawnNormal(GameObject[] pool)
    {
        if (currentWave>=1&& currentWave<=5)
        {
            Instantiate(pool[0], spawnPoint.position, Quaternion.identity);
        }
        else if (currentWave>=6&& currentWave<=10)
        {
            int index = Random.Range(0, 2);
            Instantiate(pool[index], spawnPoint.position, Quaternion.identity);
        }
        else if (currentWave>=11&& currentWave<=15)
        {
            int index = Random.Range(0, 2);
            Instantiate(pool[index], spawnPoint.position, Quaternion.identity);
        }
        else if (currentWave>=16&& currentWave<=20)
        {
            int index = Random.Range(0, 3);
            Instantiate(pool[index], spawnPoint.position, Quaternion.identity);
        }
        else
        {
            int index = Random.Range(1, 3);
            Instantiate(pool[index], spawnPoint.position, Quaternion.identity);            
        }
        aliveMonsterCount++;
    }

    void SpawnSpecial(GameObject[] pool)
    {
        if (currentWave>=10&& currentWave<=20)
        {
            int index = Random.Range(0, 1);
            Instantiate(pool[index], spawnPoint.position, Quaternion.identity);
        }        
        else if(currentWave>=21&& currentWave<=30)
        {
            int index = Random.Range(0, 2);
            Instantiate(pool[index], spawnPoint.position, Quaternion.identity);
        }
        else
        {
            int index = Random.Range(0, 4);
            if (index == 2 || index == 3) 
            {
                index = 1;
            }
            Instantiate(pool[index], spawnPoint.position, Quaternion.identity);            
        }
        aliveMonsterCount++;
    }

    public void OnMonsterDead()
    {
        aliveMonsterCount--;
        CheckWaveEnd();
    }

    void CheckWaveEnd()
    {
        if (!isSpawning && aliveMonsterCount <= 0)
        {
            SetButtonState(true);
        }
    }

    void SetButtonState(bool canStart)
    {
        nextWaveButton.interactable = canStart;
        disableOverlay.SetActive(!canStart);
    }   
}
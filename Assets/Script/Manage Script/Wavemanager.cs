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

    [Header("Boss Prefabs")]
    public GameObject boss1Prefab;
    public GameObject boss2Prefab;
    public GameObject boss3Prefab;

    [Header("Spawn")]
    public Transform spawnPoint;
    public float spawnInterval;

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public Button nextWaveButton;
    public GameObject disableOverlay; // ⭐ 추가

    int currentWave = 0;
    public int aliveMonsterCount = 0;
    bool isSpawning = false;
    bool isLastWave = false;

    void Awake()
    {
        Instance = this;
        SetButtonState(true);
    }

    public void StartNextWave()
    {
        SoundManager.Instance.PlayUIClick(0);
        if (isSpawning) return;

        currentWave++;
        waveText.text = $"Wave : {currentWave}";

        
        if (currentWave == 20 || currentWave == 30 || currentWave == 40)
        {
            SoundManager.Instance.ChangeBGM(BGMType.Boss, 2f);
        }
        else
        {
            SoundManager.Instance.ChangeBGM(BGMType.Normal, 1.5f);
        }

        SetButtonState(false);

        if (IsBossWave())
        {
            SpawnBossWave();
        }
        else if (currentWave == 40)
        {
            SpawnLastWave();
        }
        else
        {
            StartCoroutine(SpawnWave());
        }
    }

    bool IsBossWave()
    {
        return currentWave == 20 ||
            currentWave == 30;
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

    void SpawnBossWave()
    {
        isSpawning = false;

        GameObject bossPrefab = null;

        if (currentWave == 20)
            bossPrefab = boss1Prefab;
        else if (currentWave == 30)
            bossPrefab = boss2Prefab;

        if (bossPrefab == null)
        {
            Debug.LogError("Boss prefab missing!");
            return;
        }

        SpawnMonster(bossPrefab, spawnPoint.position);

        CheckWaveEnd();
    }

    void SpawnLastWave()
    {
        StartCoroutine(SpawnLastWaveCoroutine());        
    }

    IEnumerator SpawnLastWaveCoroutine()
    {
        isSpawning = true;

        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, 4);
            if(index==0)
            {
                SpawnMonster(boss1Prefab, spawnPoint.position);
            }
            else
            {
                SpawnMonster(boss2Prefab, spawnPoint.position);
            }
            yield return new WaitForSeconds(1.6f);
        }

        yield return new WaitForSeconds(3f);

        SpawnMonster(boss3Prefab, spawnPoint.position);

        isSpawning = false;
        CheckWaveEnd();
    }


    void SpawnNormal(GameObject[] pool)
    {
        GameObject selectedPrefab = null;

        if (currentWave >= 1 && currentWave <= 5)
        {
            selectedPrefab = pool[0];
        }
        else if (currentWave >= 6 && currentWave <= 15)
        {
            int index = Random.Range(0, 2);
            selectedPrefab = pool[index];
        }
        else if (currentWave >= 16 && currentWave <= 20)
        {
            int index = Random.Range(0, 4);
            int finalIndex = Mathf.Clamp(index, 0, 1);
            selectedPrefab = pool[finalIndex];
        }
        else if (currentWave >= 21 && currentWave <= 25)
        {
            int index = Random.Range(0, 3);
            selectedPrefab = pool[index];
        }
        else if (currentWave >= 26 && currentWave <= 30)
        {
            int index = Random.Range(1, 3);
            selectedPrefab = pool[index];
        }
        else
        {
            int index = Random.Range(1, 5);
            int finalIndex = Mathf.Clamp(index, 1, 2);
            selectedPrefab = pool[finalIndex];
        }

        // ⭐ 직접 생성 대신 공용 함수 호출!
        if (selectedPrefab != null)
            SpawnMonster(selectedPrefab, spawnPoint.position);
    }

    void SpawnSpecial(GameObject[] pool)
    {
        GameObject selectedPrefab = null;

        if (currentWave >= 10 && currentWave <= 20)
        {
            // Random.Range(0, 1)은 정수일 때 0만 나옵니다. (최댓값 제외)
            int index = Random.Range(0, 1); 
            selectedPrefab = pool[index];
        }
        else if (currentWave >= 21 && currentWave <= 30)
        {
            int index = Random.Range(0, 2);
            selectedPrefab = pool[index];
        }
        else
        {
            int index = Random.Range(0, 4);
            int finalIndex = Mathf.Clamp(index, 0, 1);
            selectedPrefab = pool[finalIndex];
        }

        // ⭐ 직접 생성 대신 공용 함수 호출!
        if (selectedPrefab != null)
            SpawnMonster(selectedPrefab, spawnPoint.position);
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
            if (currentWave >= 40)
            {
                SoundManager.Instance.PlayWin();
                GameMenuManager.Instance.GameOver(true);
                return;
            }
            SetButtonState(true);
            SoundManager.Instance.ChangeBGM(BGMType.Normal, 2f);
        }
    }

    void SetButtonState(bool canStart)
    {
        nextWaveButton.interactable = canStart;
        disableOverlay.SetActive(!canStart);
    }   

    public void SpawnMonster(GameObject monsterPrefab, Vector3 pos)
    {
        Instantiate(monsterPrefab, pos, Quaternion.identity);
        RegisterMonster();
    }

    public void RegisterMonster()
    {
        aliveMonsterCount++;
    }    

    public void ResetWave()
    {
        StopAllCoroutines();

        currentWave = 0;
        aliveMonsterCount = 0;
        isSpawning = false;

        waveText.text = "Wave : 0";

        SetButtonState(true);
    }

    public bool CheckLastWave()
    {
        if (currentWave == 40)
        {
            isLastWave = true;
            return true;
        }
        else 
        {
            return false; 
        }
    }
}
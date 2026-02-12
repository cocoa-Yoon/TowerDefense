using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum BGMType
{ Normal, Boss }

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxPrefab;

    [Header("Pool Settings")]
    public int sfxPoolSize = 20;

    [Header("BGM Clips")]
    public AudioClip normalBgm;
    public AudioClip bossBgm;

    [Header("SFX Clips - Tower")]
    public AudioClip[] towerAttackClips;
    public AudioClip towerPlaceClip;
    public AudioClip towerUpgradeClip;
    public AudioClip towerSellClip;

    [Header("SFX Clips - Monster")]
    public AudioClip[] monsterDieClips;

    [Header("SFX Clips - Boss")]
    public AudioClip bossDisable;
    public AudioClip bossDestroy;
    public AudioClip bossDieClip;


    [Header("SFX Clips - UI")]
    public AudioClip[] uiClickClips;

    [Header("SFX Clips - Enc")]
    public AudioClip winClips;
    public AudioClip loseClips;

    public AudioClip lifeDecreaseClips;

    [Header("BGM Toggle UI")]
    public Image bgmButtonImage;      // 버튼 이미지
    public GameObject bgmOffIcon;     // X 이미지

    bool isBgmOn = true;

    BGMType currentType;
    List<AudioSource> sfxPool = new List<AudioSource>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (sfxPrefab == null)
        {
            Debug.LogError("SoundManager: SfxPrefab이 비어있습니다!");
            return;
        }

        // SFX 풀 생성 (중복 루프 제거)
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource source = Instantiate(sfxPrefab, transform);
            source.playOnAwake = false;
            sfxPool.Add(source);
        }
    }
    

    // ==========================
    // BGM
    // ==========================

    // volume 인자를 제거하여 인스펙터에 설정된 bgmSource의 볼륨을 그대로 사용합니다.
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
        // bgmSource.volume 설정을 지웠으므로 인스펙터 값이 유지됩니다.
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void ToggleBGM()
    {
        isBgmOn = !isBgmOn;

        if (isBgmOn)
        {
            PlayBGM(normalBgm);
            bgmOffIcon.SetActive(false);
            

            // 밝게
            bgmButtonImage.color = Color.white;
        }
        else
        {
            StopBGM();
            bgmOffIcon.SetActive(true);
            

            // 어둡게
            bgmButtonImage.color = new Color(1f, 1f, 1f, 0.4f);
        }
    }

    // ==========================
    // SFX
    // ==========================

    public void PlaySFX(AudioClip clip, float volume = -1f, bool randomPitch = false)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSource();
        if (source == null) return;

        source.gameObject.SetActive(true);
        source.enabled = true;

        source.clip = clip;

        // 볼륨 설정: -1이면 프리팹 볼륨 유지, 아니면 입력된 값 사용
        if (volume >= 0f) 
            source.volume = volume;
        else 
            source.volume = sfxPrefab.volume; // 초기화 보장

        source.pitch = randomPitch ? Random.Range(0.9f, 1.1f) : 1f;
        source.Play();
    }

    

   public void PlayTowerAttack(int index) { 
        if (index < towerAttackClips.Length)            
            PlaySFX(towerAttackClips[index], 0.05f); 
    }

    public void PlayTowerPlace() { 
        PlaySFX(towerPlaceClip, 0.12f); 
    }

    public void PlayTowerUpgrade() { 
        PlaySFX(towerUpgradeClip); 
    }

    public void PlayTowerSell() { 
        PlaySFX(towerSellClip); 
    }

    public void PlayMonsterDie(int index) { 
        if (index < monsterDieClips.Length)
            if(index == 0)
            {
                PlaySFX(monsterDieClips[index], 0.05f, true);
            }
            else PlaySFX(monsterDieClips[index], randomPitch: true); 
    }

    public void PlayBossDisable()
    {
        PlaySFX(bossDisable);
    }

    public void PlayBossDestroy()
    {
        PlaySFX(bossDestroy);
    }

    public void PlayBossDie() { 
        PlaySFX(bossDieClip); 
    }

    public void PlayUIClick(int index) 
    {         
        PlaySFX(uiClickClips[index]);            
    }

    public void PlayWin() 
    { 
        PlaySFX(winClips); 
    } 

     public void PlayLose() 
    { 
        PlaySFX(loseClips); 
    } 

    public void PlayLifeDecrease()
    {
        PlaySFX(lifeDecreaseClips);
    }

    AudioSource GetAvailableSource()
    {
        for (int i = sfxPool.Count - 1; i >= 0; i--)
        {
            if (sfxPool[i] == null) { sfxPool.RemoveAt(i); continue; }
            if (!sfxPool[i].isPlaying)
            {
                if (!sfxPool[i].gameObject.activeSelf) sfxPool[i].gameObject.SetActive(true);
                sfxPool[i].enabled = true; 
                sfxPool[i].volume = sfxPrefab.volume; 

                return sfxPool[i];
            }
        }

        AudioSource newSource = Instantiate(sfxPrefab, transform);
        newSource.gameObject.SetActive(true);
        newSource.enabled = true;
        newSource.playOnAwake = false;
        sfxPool.Add(newSource);
        return newSource;
    }

    // 필요할 때만 호출할 수 있도록 함수 자체는 남겨두되, 초기화에서는 호출하지 않습니다.
    public void SetBGMVolume(float value) { bgmSource.volume = value; }
    public void SetSFXVolume(float value) { foreach (var source in sfxPool) source.volume = value; }

    public void ChangeBGM(BGMType type, float fadeTime = 1.5f)
    {
        if (currentType == type) return;
        currentType = type;
        AudioClip targetClip = (type == BGMType.Normal) ? normalBgm : bossBgm;
        FadeToBGM(targetClip, fadeTime);
    }

    public void FadeToBGM(AudioClip newClip, float fadeTime = 1.5f)
    {
        StartCoroutine(FadeBGMCoroutine(newClip, fadeTime));
    }

    IEnumerator FadeBGMCoroutine(AudioClip newClip, float fadeTime)
    {
        float targetVolume = bgmSource.volume; // 현재 인스펙터 볼륨을 타겟으로 잡음
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(targetVolume, 0f, t / fadeTime);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();

        t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeTime);
            yield return null;
        }
        bgmSource.volume = targetVolume;
    }
}
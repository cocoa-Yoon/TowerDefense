using UnityEngine;
using UnityEngine.UI;

public class MonsterHP : MonoBehaviour
{
    public Transform hpBarCanvas; 
    public Image redBar;    

    [Header("Stats")]
    public float maxHp = 100;
    public int reward = 5;

    [Header("UI Offset")]
    public Vector3 hpBarOffset = new Vector3(0, 1.2f, 0);

    float currentHp;
    bool hasBeenHit = false;
    public bool isBoss = false;
    public bool isSpecial = false;
    bool isDead = false;

    void Awake()
    {
        if (GameMenuManager.Instance != null)
        {
            maxHp *= GameMenuManager.Instance.DifficultyMultiplier;
        }

        currentHp = maxHp;
    }

    void Start()
    {        
        UpdateHP();         

        if (hpBarCanvas != null)
            hpBarCanvas.gameObject.SetActive(false);
    }
    
    void UpdateHP()
    {
        if (maxHp > 0)
        {
            redBar.fillAmount = currentHp / maxHp;
        }
    }
    

    void LateUpdate()
    {
        // HP바 회전 고정 (몬스터가 돌아도 안 돌아가게)
        if (hpBarCanvas != null)
        {
            hpBarCanvas.position = transform.position + hpBarOffset;            
            hpBarCanvas.rotation = Quaternion.identity;
        }
            
    }

    public void TakeDamage(float dmg)
    {
        // 이미 죽은 상태라면 데미지 계산을 하지 않음
        if (isDead) return;

        currentHp -= dmg;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateHP();

        if (!hasBeenHit)
        {
            hasBeenHit = true;
            if (hpBarCanvas != null)
                hpBarCanvas.gameObject.SetActive(true);
        }

        // 체력이 0 이하이고, 아직 죽은 처리가 되지 않았을 때만 Die 호출
        if (currentHp <= 0 && !isDead)
        {
            isDead = true; // 죽었다고 표시 (중복 방지 lock)
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isBoss)
        {
            if (WaveManager.Instance.CheckLastWave())
            {
                return;
            }
            else SoundManager.Instance.PlayBossDie();
        }
        else if (isSpecial)
        {
            SoundManager.Instance.PlayMonsterDie(1);
        }
        else SoundManager.Instance.PlayMonsterDie(0);

        WaveManager.Instance.OnMonsterDead();
        
        GameManager.instance.AddGold(reward);
        
        Destroy(gameObject);
    }
   
}
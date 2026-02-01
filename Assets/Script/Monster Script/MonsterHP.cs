using UnityEngine;
using UnityEngine.UI;

public class MonsterHP : MonoBehaviour
{
    public Transform hpBarCanvas;   // HPBarCanvas
    public Image redBar;    

    [Header("Stats")]
    public float maxHp = 100;
    public int reward = 5;

    float currentHp;
    bool hasBeenHit = false;

    void Start()
    {
        currentHp = maxHp;
        UpdateHP();

        if (hpBarCanvas != null)
            hpBarCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentHp <= 0)
        {
            Die();
        }
    }

    void LateUpdate()
    {
        // HP바 회전 고정 (몬스터가 돌아도 안 돌아가게)
        if (hpBarCanvas != null)
            hpBarCanvas.rotation = Quaternion.identity;
    }

    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        UpdateHP();

        if (!hasBeenHit)
        {
            hasBeenHit = true;
            if (hpBarCanvas != null)
                hpBarCanvas.gameObject.SetActive(true);
        }
    }

    void UpdateHP()
    {
        redBar.fillAmount = currentHp / maxHp;
    }

    protected virtual void Die()
    {
        GameManager.instance.AddGold(reward);
        Destroy(gameObject);
    }
}
using UnityEngine;

public class FireZone : MonoBehaviour
{
    public float radius = 2.5f;
    public float duration = 3f;
    public float tickInterval = 0.5f;

    float damagePerTick;
    float timer;
    float tickTimer;

    public void Init(float baseDamage)
    {
        damagePerTick = baseDamage;
    }

    void Update()
    {
        timer += Time.deltaTime;
        tickTimer += Time.deltaTime;

        if (tickTimer >= tickInterval)
        {
            tickTimer = 0f;
            DealDamage();
        }

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }

    void DealDamage()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D col in hits)
        {
            MonsterHP hp = col.GetComponent<MonsterHP>();

            if (hp == null) continue;

            hp.TakeDamage(damagePerTick);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
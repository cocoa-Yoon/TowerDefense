using UnityEngine;

public class SlowBullet : MonoBehaviour
{
    public float speed = 6f;
    float damage;

    public float slowRadius = 2.5f;
    public float slowRate = 0.5f;     // 50% 느려짐
    public float slowDuration = 2f;

    Transform target;
    bool exploded;

    public void Init(float d, Transform t)
    {
        damage = d;
        target = t;
    }

    void Update()
    {
        if (exploded) return;

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 dir = (target.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        if (Vector2.Distance(transform.position, target.position) <= 0.15f)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (exploded) return;
        exploded = true;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, slowRadius);

        foreach (Collider2D col in hits)
        {
            if (!col.CompareTag("Monster")) continue;

            col.GetComponent<MonsterHP>()?.TakeDamage(damage);

            if (col.GetComponent<BossMonster>() != null) 
            {
                continue; 
            }

            col.GetComponent<MonsterMove>()?.ApplySlow(slowRate, slowDuration);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, slowRadius);
    }
}
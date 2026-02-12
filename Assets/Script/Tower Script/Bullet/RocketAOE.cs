using UnityEngine;

public class RocketAOE : MonoBehaviour
{
    public float speed = 6f;
    
    public float explosionRadius = 2.5f;
    float damage;

    public GameObject explosionEffect;

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

        // 회전
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        // ⭐ 거리 기반 폭발 (여기가 핵심)
        float dist = Vector2.Distance(transform.position, target.position);
        if (dist <= 0.15f)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (exploded) return;
        exploded = true;

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D col in hits)
        {
            if (!col.CompareTag("Monster")) continue;

            col.GetComponent<MonsterHP>()?.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    // 🔍 디버그용
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
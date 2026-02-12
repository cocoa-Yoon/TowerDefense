using UnityEngine;

public class MagicTowerAttack : Tower
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    float timer;
    Transform target;
    Tower tower;

    void Awake()
    {
        tower = GetComponent<Tower>();
    }

    void Update()
    {
        if (tower != null && tower.IsDisabled()) return;

        FindNearestTarget();

        if (target == null)
        {
            timer = 0f;
            return;
        }

        RotateToTarget();

        timer += Time.deltaTime;
        if (timer >= attackDelay)
        {
            timer = 0f;
            Fire();
        }
    }

    void FindNearestTarget()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, attackRange);

        float closestDist = Mathf.Infinity;
        Transform closest = null;

        foreach (Collider2D col in hits)
        {
            if (!col.CompareTag("Monster")) continue;

            float dist =
                Vector2.Distance(transform.position, col.transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                closest = col.transform;
            }
        }

        target = closest;
    }

    void RotateToTarget()
    {
        Vector2 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Fire()
    {
        if (target == null) return;

        SoundManager.Instance.PlayTowerAttack(2);

        GameObject b1 = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        if (b1.GetComponent<FireBullet>() != null)
            b1.GetComponent<FireBullet>().Init(damage, target);
        else if (b1.GetComponent<SlowBullet>() != null)
            b1.GetComponent<SlowBullet>().Init(damage, target);
        else
            b1.GetComponent<Rocket>()?.Init(damage, target);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
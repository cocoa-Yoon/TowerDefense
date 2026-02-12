using UnityEngine;

public class TowerAttack : Tower
{
    public GameObject rocketPrefab;
    public Transform firePoint1;
    public Transform firePoint2;

    float timer;
    Animator anim;
    Transform target;
    PathManager pathManager;
    Tower tower;

    bool wasAttacking;

    void Awake()
    {
        anim = GetComponent<Animator>();
        tower = GetComponentInParent<Tower>();
        pathManager = PathManager.instance;
    }

    void Update()
    {
        if (tower != null && tower.IsDisabled()) return;

        FindTarget();

        bool isAttacking = target != null;
        anim.SetBool("isAttacking", isAttacking);

        // 🔥 타겟이 있으면 항상 바라보게
        if (target != null)
        {
            RotateToTarget();
        }

        // 🔥 Idle → Attack 진입 순간 즉시 발사
        if (isAttacking && !wasAttacking)
        {
            FireRocket();
            timer = 0f;
        }

        // 🔁 공격 중 반복 발사
        if (isAttacking)
        {
            timer += Time.deltaTime;
            if (timer >= attackDelay)
            {
                timer = 0f;
                FireRocket();
            }
        }
        else
        {
            timer = 0f;
        }

        wasAttacking = isAttacking;
    }

    void FindTarget()
{
    Collider2D[] monsters = Physics2D.OverlapCircleAll(
        transform.position, attackRange);

    MonsterMove best = null;
    int bestOrder = -1;
    float bestDist = Mathf.Infinity;

    foreach (Collider2D col in monsters)
    {
        if (!col.CompareTag("Monster")) continue;

        MonsterMove m = col.GetComponent<MonsterMove>();
        if (m == null) continue;

        if (!pathManager.pathOrder.ContainsKey(m.currentTile))
            continue;

        int order = pathManager.pathOrder[m.currentTile];
        float dist = Vector2.Distance(transform.position, col.transform.position);

        // 🔥 더 앞에 있는 적 우선
        if (order > bestOrder ||
            (order == bestOrder && dist < bestDist))
        {
            best = m;
            bestOrder = order;
            bestDist = dist;
        }
    }

    target = best != null ? best.transform : null;
}

    void RotateToTarget()
    {
        Vector2 dir = target.position - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRot = Quaternion.Euler(0, 0, angle);

        
        // 🔥 부드러운 회전 (추천)
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
    }

    void FireRocket()
    {
        if (target == null) return;

        SoundManager.Instance.PlayTowerAttack(0);

        Vector3 offset = firePoint1.right * 0.2f;

        GameObject rocket = Instantiate(rocketPrefab, firePoint1.position, Quaternion.identity);
        rocket.GetComponent<Rocket>().Init(damage, target);
        
        GameObject rocket2 = Instantiate(rocketPrefab, firePoint2.position, Quaternion.identity);
        rocket2.GetComponent<Rocket>().Init(damage, target);
    }
    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
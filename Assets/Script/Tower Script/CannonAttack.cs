using UnityEngine;

public enum FireMode
{
    Single,     // 한 발
    Alternating // 두 발 번갈아
}

public class CannonAttack : Tower
{
    public enum FireMode { Single, Alternating }
    public FireMode fireMode = FireMode.Single;

    public GameObject rocketPrefab;

    // 🔥 발사 위치들
    public Transform[] firePoints;

    float timer;
    int fireIndex = 0;

    Transform target;
    PathManager pathManager;
    Tower tower;

    void Awake()
    {
        tower = GetComponentInParent<Tower>();
        pathManager = PathManager.instance;        
    }

   void Update()
    {
        if (tower.IsDisabled()) return;

        FindTarget();

        if (target != null)
        {
            RotateToTarget();

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
    }

    void FireRocket()
    {
        if (target == null) return;

        SoundManager.Instance.PlayTowerAttack(1);

        Transform firePoint = firePoints[0];

        // 🔥 발사 모드 분기
        if (fireMode == FireMode.Alternating && firePoints.Length >= 2)
        {
            firePoint = firePoints[fireIndex];
            fireIndex = (fireIndex + 1) % firePoints.Length;
        }

        if(SoundManager.Instance.towerAttackClips.Length > 0)
        {
            SoundManager.Instance.PlayTowerAttack(1);
        }

        GameObject rocket = Instantiate(rocketPrefab, firePoint.position, Quaternion.identity);
        rocket.GetComponent<Rocket>().Init(damage, target);
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

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
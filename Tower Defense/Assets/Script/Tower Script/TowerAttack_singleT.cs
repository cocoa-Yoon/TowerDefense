using UnityEngine;

public class TowerAttack_singleT : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackDelay = 1.5f;
    public float rotateSpeed = 720f;   // 초당 회전 속도 (선택)

    public GameObject rocketPrefab;
    public Transform firePoint;

    float timer;
    Animator anim;
    Transform target;
    PathManager pathManager;

    bool wasAttacking;

    void Start()
    {
        anim = GetComponent<Animator>();
        pathManager = PathManager.instance;
    }

    void Update()
    {
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
        
        GameObject rocket = Instantiate(
            rocketPrefab,
            firePoint.position,
            Quaternion.identity
        );
        rocket.GetComponent<RocketAOE>().SetTarget(target);
    }
    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
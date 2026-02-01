using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Stats")]
    public float attackRange = 3f;
    public float attackDamage = 10f;
    public float attackDelay = 1f;

    float attackTimer = 0f;

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackDelay)
        {
            MonsterHP target = FindTarget();

            if (target != null)
            {
                Attack(target);
                attackTimer = 0f;
            }
        }
    }

   MonsterHP FindTarget()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        MonsterHP bestTarget = null;
        int maxPathIndex = -1;

        foreach (GameObject monster in monsters)
        {
            float dist = Vector3.Distance(transform.position, monster.transform.position);

            // 사거리 밖이면 패스
            if (dist > attackRange) continue;

            MonsterMove move = monster.GetComponent<MonsterMove>();
            MonsterHP hp = monster.GetComponent<MonsterHP>();

            if (move == null || hp == null) continue;

            // 더 앞선 몬스터 선택
            if (move.GetPathIndex() > maxPathIndex)
            {
                maxPathIndex = move.GetPathIndex();
                bestTarget = hp;
            }
        }

        return bestTarget;
    }

    void Attack(MonsterHP target)
    {
        target.TakeDamage(attackDamage);
        // TODO: 공격 이펙트 / 사운드
    }

    // 에디터에서 사거리 표시
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
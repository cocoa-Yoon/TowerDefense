using UnityEngine;

public class AoeTower : MonoBehaviour
{
    [Header("Tower Stats")]
    public float attackRange = 4f;
    public float attackDelay = 1.5f;
    public float damage = 15f;

    [Header("Area Damage")]
    public float explosionRadius = 1.5f;

    float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= attackDelay)
        {
            MonsterMove target = FindTarget();

            if (target != null)
            {
                Explode(target.transform.position);
                timer = 0f;
            }
        }
    }

    // 가장 앞선 몬스터 선택
    MonsterMove FindTarget()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        MonsterMove bestTarget = null;
        int maxPathIndex = -1;

        foreach (GameObject monster in monsters)
        {
            float dist = Vector3.Distance(transform.position, monster.transform.position);
            if (dist > attackRange) continue;

            MonsterMove move = monster.GetComponent<MonsterMove>();
            if (move == null) continue;

            if (move.GetPathIndex() > maxPathIndex)
            {
                maxPathIndex = move.GetPathIndex();
                bestTarget = move;
            }
        }

        return bestTarget;
    }

    void Explode(Vector3 center)
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            float dist = Vector3.Distance(center, monster.transform.position);

            if (dist <= explosionRadius)
            {
                MonsterHP hp = monster.GetComponent<MonsterHP>();
                if (hp != null)
                {
                    hp.TakeDamage(damage);
                }
            }
        }

        // TODO: 폭발 이펙트
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
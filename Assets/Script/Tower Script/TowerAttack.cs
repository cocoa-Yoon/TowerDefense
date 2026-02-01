using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackDelay = 1.5f;

    public GameObject rocketPrefab;
    public Transform firePoint;

    float timer;
    Animator anim;
    Transform target;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        FindTarget();

        if (target != null && timer >= attackDelay)
        {
            timer = 0f;
            anim.SetTrigger("Attack");
        }
    }

    void FindTarget()
    {
        Collider2D[] monsters = Physics2D.OverlapCircleAll(
            transform.position, attackRange);

        float minDist = Mathf.Infinity;
        Transform nearest = null;

        foreach (Collider2D col in monsters)
        {
            if (col.CompareTag("Monster"))
            {
                float dist = Vector2.Distance(
                    transform.position, col.transform.position);

                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = col.transform;
                }
            }
        }

        target = nearest;
    }

    // 🔥 애니메이션 이벤트에서 호출
    public void FireRocket()
    {
        if (target == null) return;

        GameObject rocket = Instantiate(
            rocketPrefab, firePoint.position, Quaternion.identity);

        rocket.GetComponent<Rocket>().SetTarget(target);
    }
}
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 6f;
    float damage;

    Transform target;

    public void Init(float d, Transform t)
    {
        damage = d;
        target = t;
    }


    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 dir = (target.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);

        // 🔥 위쪽이 진행 방향이 되도록 회전
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<MonsterHP>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
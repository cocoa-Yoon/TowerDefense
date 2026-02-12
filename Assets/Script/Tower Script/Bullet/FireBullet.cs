using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float speed = 6f;
    float damage;

    public GameObject fireZonePrefab;

    Transform target;
    bool hit;

    public void Init(float d, Transform t)
    {
        damage = d;
        target = t;
    }

    void Update()
    {
        if (hit) return;

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
            Hit();
        }
    }

    void Hit()
    {
        hit = true;

        GameObject zone =
            Instantiate(fireZonePrefab, transform.position, Quaternion.identity);

        zone.GetComponent<FireZone>()?.Init(damage);

        Destroy(gameObject);
    }
}
using UnityEngine;
using System.Collections;

public abstract class BossMonster : MonoBehaviour
{
    [Header("Skill Timing")]
    public float minSkillDelay = 3f;
    public float maxSkillDelay = 6f;

    [Header("Skill Range")]
    public float skillRange = 3f;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    protected MonsterMove move;
    bool isUsingSkill;

    protected virtual void Awake()
    {
        move = GetComponent<MonsterMove>();
    }

    protected virtual void Start()
    {
        StartCoroutine(SkillRoutine());
    }

    IEnumerator SkillRoutine()
    {
        while (gameObject.activeInHierarchy)
        {
            float delay = Random.Range(minSkillDelay, maxSkillDelay);
            yield return new WaitForSeconds(delay);

            if (isUsingSkill) continue;

            isUsingSkill = true;
            move.SetStop(true);

            ExecuteSkill();

            yield return new WaitForSeconds(0.5f); // 캐스팅 시간
            move.SetStop(false);
            isUsingSkill = false;
        }
    }

    protected abstract void ExecuteSkill();

    protected Tower GetRandomTowerInRange()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, skillRange);

        var towers = new System.Collections.Generic.List<Tower>();

        foreach (var col in hits)
        {
            Tower t = col.GetComponent<Tower>();
            if (t != null)
                towers.Add(t);
        }

        if (towers.Count == 0) return null;

        return towers[Random.Range(0, towers.Count)];
    }

    protected void FireProjectile(Tower tower, BossSkillType type)
    {
        GameObject proj =
            Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        proj.GetComponent<BossProjectile>().Init(tower, type);
    }

    protected void SummonMonsters(
        GameObject[] prefabs,
        int min,
        int max,
        float radius
    )
    {
        if (prefabs == null || prefabs.Length == 0) return;

        int count = Random.Range(min, max + 1);

        for (int i = 0; i < count; i++)
        {
            GameObject prefab =
                prefabs[Random.Range(0, prefabs.Length)];

            Vector3 pos = transform.position +
                          (Vector3)(Random.insideUnitCircle * radius);

            WaveManager.Instance.SpawnMonster(prefab, pos);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, skillRange);
    }
}
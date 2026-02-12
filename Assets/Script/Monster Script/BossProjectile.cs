using UnityEngine;
using System;

public enum BossSkillType
{
    Disable,
    Destroy
}

public class BossProjectile : MonoBehaviour
{
    public float speed = 6f;
    public float disableTime = 8f;

    [Header("Fly Effects")]
    public GameObject disableFlyEffect;
    public GameObject destroyFlyEffect;

    [Header("Hit Effects")]
    public GameObject disableHitEffect;
    public GameObject destroyHitEffect;

    Tower target;
    BossSkillType skillType;

    public void Init(Tower target, BossSkillType type)
    {
        this.target = target;
        this.skillType = type;

        if (skillType == BossSkillType.Disable && disableFlyEffect)
            Instantiate(disableFlyEffect, transform);
        else if (skillType == BossSkillType.Destroy && destroyFlyEffect)
            Instantiate(destroyFlyEffect, transform);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.transform.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            Hit();
    }

    void Hit()
    {
        // 이미 Init에서 Tower를 target으로 받아왔으므로 다시 찾을 필요가 없습니다.
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (skillType == BossSkillType.Disable)
        {
            if (disableHitEffect)
            {
                SoundManager.Instance.PlayBossDisable();
                Instantiate(disableHitEffect, target.transform.position, Quaternion.identity);
            }
            // 타워의 Disable 함수 호출
            target.Disable(disableTime);
        }
        else if (skillType == BossSkillType.Destroy)
        {
            if (destroyHitEffect)
            {
                SoundManager.Instance.PlayBossDestroy();
                GameObject go = Instantiate(destroyHitEffect, target.transform.position, Quaternion.identity);
                Destroy(go,0.5f);
            }
            // 타워의 DestroyTower 함수 호출
            target.DestroyTower();
        }

        Destroy(gameObject);
    }
}
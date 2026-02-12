using UnityEngine;

public class Boss2_Random : BossMonster
{
    [Header("Disable Skill")]
    public float disableTime = 3f;

    [Header("Summon Skill")]
    public GameObject[] summonMonsterPrefabs;
    public int summonMin = 2;
    public int summonMax = 4;
    public float summonRadius = 1.5f;

    protected override void ExecuteSkill()
    {
        int rand = Random.Range(0, 3);

        if (rand == 2)
        {
            SummonMonsters(
                summonMonsterPrefabs,
                summonMin,
                summonMax,
                summonRadius
            );
            return;
        }

        Tower tower = GetRandomTowerInRange();
        if (tower == null) return;

        if (rand == 0)
        {
            FireProjectile(tower, BossSkillType.Disable);
        }
        else
        {
            FireProjectile(tower, BossSkillType.Destroy);
        }
    }
}
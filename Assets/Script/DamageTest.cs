using UnityEngine;

public class MonsterGlobalDamage : MonoBehaviour
{
    public int damage = 20;

    void Update()
{
    if (Input.GetKeyDown(KeyCode.LeftControl))
    {
        DamageAllMonsters();
    }
}

    public void DamageAllMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            MonsterHP hp = monster.GetComponent<MonsterHP>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }
    }
}
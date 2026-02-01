using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform startPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(monsterPrefab, startPoint.position, Quaternion.identity);
        }
    }
}
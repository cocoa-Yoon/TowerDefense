using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    [Header("Move")]
    public float speed = 2.5f;
    public float arriveDistance = 0.05f;

    private List<Vector3> path;
    private int pathIndex = 0;

    private PathFinder pathFinder;

    void Start()
{
    pathFinder = FindFirstObjectByType<PathFinder>();

    if (pathFinder == null)
    {
        Debug.LogError("PathFinder not found in scene!");
        return;
    }

    path = pathFinder.GetPath();

    if (path == null || path.Count == 0)
    {
        Debug.LogError("Path is empty!");
        return;
    }

    transform.position = path[0];
    pathIndex = 1;
}

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (pathIndex >= path.Count)
        {
            ReachEnd();
            return;
        }

        Vector3 target = path[pathIndex];
        Vector3 dir = (target - transform.position).normalized;

        // 이동
        transform.position += dir * speed * Time.deltaTime;

        // 회전 (2D 기준)
        Rotate(dir);

        // 다음 포인트 도착 체크
        if (Vector3.Distance(transform.position, target) <= arriveDistance)
        {
            pathIndex++;
        }
    }

    void Rotate(Vector3 dir)
    {
        // 좌우 이동 기준 회전
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void ReachEnd()
    {
        GameManager.instance.OnMonsterArrive();

        Destroy(gameObject);
    }

    public int GetPathIndex()
    {
        return pathIndex;
    }

}

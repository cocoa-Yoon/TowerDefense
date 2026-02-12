using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterMove : MonoBehaviour
{
    [Header("Move")]
    public float baseSpeed = 2.5f;
    public float arriveDistance = 0.1f;
    float currentSpeed;

    private List<Vector3> path;
    private int pathIndex = 0;

    private PathFinder pathFinder;
    private Tilemap pathTilemap;
    public Vector3Int currentTile;

    private Color originalColor;
    SpriteRenderer sr;
    Coroutine slowRoutine;

    bool isStopped = false;

    public void SetStop(bool stop)
    {
        isStopped = stop;
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        originalColor = sr.color;
        currentSpeed = baseSpeed;
    }
    
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

        pathTilemap = GameObject.Find("Tilemap_Path")
        .GetComponent<Tilemap>();

        transform.position = path[0];
        pathIndex = 1;
    }

    void Update()
    {
        if (isStopped) return;

        MoveAlongPath();
        currentTile = pathTilemap.WorldToCell(transform.position);
    }

    public void ApplySlow(float slowRate, float duration)
    {
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(SlowCoroutine(slowRate, duration));
    }

    IEnumerator SlowCoroutine(float slowRate, float duration)
    {
        // 속도 감소
        currentSpeed = baseSpeed * slowRate;

        // 색상 적용 (항상 '저장된 originalColor'를 기준으로 계산합니다)
        Color slowColor = new Color(
            originalColor.r * 0.6f,
            originalColor.g * 0.8f,
            1f, 
            originalColor.a
        );
        sr.color = slowColor;

        yield return new WaitForSeconds(duration);

        // 복구 (저장해둔 원래 색상으로 완벽하게 복원)
        currentSpeed = baseSpeed;
        sr.color = originalColor;

        slowRoutine = null;
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
        transform.position += dir * currentSpeed * Time.deltaTime;

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
        MonsterHP hp = GetComponent<MonsterHP>();

        if (hp != null)
        {
            GameManager.instance.OnMonsterArrive(hp.isBoss);
        }            
        else
            GameManager.instance.OnMonsterArrive(false);

        SoundManager.Instance.PlayLifeDecrease();
        Destroy(gameObject);
    }

    public int GetPathIndex()
    {
        return pathIndex;
    }

}
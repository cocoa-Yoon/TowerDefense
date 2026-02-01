using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{
    [Header("References")]
    public Tilemap pathTilemap;
    public Transform startPoint;
    public Transform endPoint;

    // 계산된 경로 캐싱
    private List<Vector3> cachedPath;

    // 4방향 이동
    private readonly Vector3Int[] dirs =
    {
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right
    };

    void Awake()
    {
        CalculatePath();
    }

    
    // 외부(몬스터)에서 경로 요청    
    public List<Vector3> GetPath()
    {
        return cachedPath;
    }

   
    // BFS로 경로 계산 (1회)    
    void CalculatePath()
    {
        Vector3Int startCell = pathTilemap.WorldToCell(startPoint.position);
        Vector3Int endCell = pathTilemap.WorldToCell(endPoint.position);

        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        queue.Enqueue(startCell);
        cameFrom[startCell] = startCell;

        while (queue.Count > 0)
        {
            Vector3Int current = queue.Dequeue();

            if (current == endCell)
                break;

            foreach (Vector3Int dir in dirs)
            {
                Vector3Int next = current + dir;

                if (cameFrom.ContainsKey(next))
                    continue;

                if (!pathTilemap.HasTile(next))
                    continue;

                queue.Enqueue(next);
                cameFrom[next] = current;
            }
        }

        BuildPath(cameFrom, startCell, endCell);
    }

    
    // 역추적해서 월드 좌표 path 생성
    void BuildPath(
        Dictionary<Vector3Int, Vector3Int> cameFrom,
        Vector3Int start,
        Vector3Int end)
    {
        cachedPath = new List<Vector3>();

        if (!cameFrom.ContainsKey(end))
        {
            Debug.LogError("Path not found!");
            return;
        }

        Vector3Int current = end;

        while (current != start)
        {
            cachedPath.Add(pathTilemap.GetCellCenterWorld(current));
            current = cameFrom[current];
        }

        cachedPath.Add(pathTilemap.GetCellCenterWorld(start));
        cachedPath.Reverse();
    }
}
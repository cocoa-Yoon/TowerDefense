using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TowerBuildManager : MonoBehaviour
{
    public Grid grid;
    public Tilemap buildableTilemap;
    public Tilemap pathTilemap;
    public Tilemap blockTilemap;

    HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryBuild();
        }
    }

    void TryBuild()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = grid.WorldToCell(mouseWorldPos);
        
        // 막힌 영역이면 설치 불가
        if (blockTilemap.HasTile(cellPos)) return;
        if (occupiedCells.Contains(cellPos)) return;

        if (!buildableTilemap.HasTile(cellPos)) return;

        Debug.Log("여기에 타워 설치 가능!");

        occupiedCells.Add(cellPos);
    }
}
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class PathManager : MonoBehaviour
{
    public Tilemap pathTilemap;
    public static PathManager instance;

    public Dictionary<Vector3Int, int> pathOrder
        = new Dictionary<Vector3Int, int>();

    void Awake()
    {
        BuildPathOrder();
        instance = this;
    }

    void BuildPathOrder()
    {
        // 👇 예시: 길을 직접 순서대로 지정
        Vector3Int[] pathTiles = new Vector3Int[]
        {
            new Vector3Int(-9, -8, 0),
            new Vector3Int(-9, -7, 0),
            new Vector3Int(-9, -6, 0),
            new Vector3Int(-9, -5, 0),
            new Vector3Int(-9, -4, 0),
            new Vector3Int(-10, -4, 0),
            new Vector3Int(-11, -4, 0),
            new Vector3Int(-12, -4, 0),
            new Vector3Int(-13, -4, 0),
            new Vector3Int(-13, -3, 0),
            new Vector3Int(-13, -2, 0),
            new Vector3Int(-13, -1, 0),
            new Vector3Int(-13, 0, 0),
            new Vector3Int(-13, 1, 0),
            new Vector3Int(-13, 2, 0),
            new Vector3Int(-13, 3, 0),
            new Vector3Int(-13, 4, 0),
            new Vector3Int(-13, 5, 0),
            new Vector3Int(-13, 6, 0),
            new Vector3Int(-12, 6, 0),
            new Vector3Int(-11, 6, 0),
            new Vector3Int(-10, 6, 0),
            new Vector3Int(-9, 6, 0),
            new Vector3Int(-8, 6, 0),
            new Vector3Int(-7, 6, 0),
            new Vector3Int(-6, 6, 0),
            new Vector3Int(-5, 6, 0),
            new Vector3Int(-4, 6, 0),
            new Vector3Int(-3, 6, 0),
            new Vector3Int(-3, 5, 0),
            new Vector3Int(-3, 4, 0),
            new Vector3Int(-3, 3, 0),
            new Vector3Int(-2, 3, 0),
            new Vector3Int(-1, 3, 0),
            new Vector3Int(0, 3, 0),
            new Vector3Int(1, 3, 0),
            new Vector3Int(2, 3, 0),
            new Vector3Int(2, 2, 0),
            new Vector3Int(2, 1, 0),
            new Vector3Int(2, 0, 0),
            new Vector3Int(2, -1, 0),
            new Vector3Int(2, -2, 0),
            new Vector3Int(2, -3, 0),
            new Vector3Int(2, -4, 0),
            new Vector3Int(2, -5, 0),
            new Vector3Int(2, -6, 0),
            new Vector3Int(3, -6, 0),
            new Vector3Int(4, -6, 0),
            new Vector3Int(5, -6, 0),
            new Vector3Int(6, -6, 0),
            new Vector3Int(7, -6, 0),
            new Vector3Int(8, -6, 0),
            new Vector3Int(8, -5, 0),
            new Vector3Int(8, -4, 0),
            new Vector3Int(8, -3, 0),
            new Vector3Int(8, -2, 0),
            new Vector3Int(8, -1, 0),
            new Vector3Int(8, 0, 0),
            new Vector3Int(8, 1, 0),
            new Vector3Int(8, 2, 0),
            new Vector3Int(8, 3, 0),
            new Vector3Int(8, 4, 0),
            new Vector3Int(8, 5, 0),
            new Vector3Int(8, 6, 0),
            new Vector3Int(8, 7, 0),
        };

        for (int i = 0; i < pathTiles.Length; i++)
        {
            pathOrder[pathTiles[i]] = i;
        }
    }
}
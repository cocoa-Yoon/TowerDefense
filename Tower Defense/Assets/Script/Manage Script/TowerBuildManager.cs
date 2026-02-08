using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TowerBuildManager : MonoBehaviour
{
    public static TowerBuildManager Instance;

    public Tilemap buildableTilemap;
    public Tilemap blockTilemap;

    HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    bool isBuildMode = false;

    GameObject selectedTowerPrefab;
    int selectedTowerCost;

    GameObject previewTower;
    SpriteRenderer[] previewRenderers;

    Color canBuildColor = new Color(0f, 1f, 0f, 0.5f);
    Color cantBuildColor = new Color(1f, 0f, 0f, 0.5f);

    [Header("Preview Outline")]
    public Sprite outlineSprite;

    GameObject outlineObj;
    SpriteRenderer outlineRenderer;


    void Awake()
    {
        Instance = this;
    }

    public void SelectTower(GameObject prefab, int cost)
    {
        selectedTowerPrefab = prefab;
        selectedTowerCost = cost;
        isBuildMode = true;

        CreatePreview();
    }

    void CreatePreview()
    {
        if (previewTower != null)
            Destroy(previewTower);

        if (outlineObj != null)
            Destroy(outlineObj);

        // 🔹 타워 프리뷰
        previewTower = Instantiate(selectedTowerPrefab);

        foreach (var attack in previewTower.GetComponentsInChildren<TowerAttack>())
            attack.enabled = false;
        foreach (var attack in previewTower.GetComponentsInChildren<CannonAttack>())
            attack.enabled = false;
        foreach (var attack in previewTower.GetComponentsInChildren<TowerAttack_singleT>())
            attack.enabled = false;

        foreach (var col in previewTower.GetComponentsInChildren<Collider2D>())
            Destroy(col);

        previewRenderers = previewTower.GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in previewRenderers)
            r.color = canBuildColor;

        // 🔹 네모 테두리
        outlineObj = new GameObject("BuildOutline");
        outlineRenderer = outlineObj.AddComponent<SpriteRenderer>();
        outlineRenderer.sprite = outlineSprite;
        outlineRenderer.color = canBuildColor;

        outlineRenderer.sortingOrder = 100; // 항상 위에
    }

    void Update()
    {
        if (!isBuildMode) return;

        

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cellPos = buildableTilemap.WorldToCell(worldPos);        

        Vector3 cellCenter = buildableTilemap.GetCellCenterWorld(cellPos);
        previewTower.transform.position = cellCenter;
        outlineObj.transform.position = cellCenter;

        bool canBuild =
        buildableTilemap.HasTile(cellPos) &&
        !blockTilemap.HasTile(cellPos) &&
        !occupiedCells.Contains(cellPos) &&
        GameManager.instance.gold >= selectedTowerCost;

        SetPreviewColor(canBuild);

        if (Input.GetMouseButtonDown(0) && canBuild)
        {
            PlaceTower(cellPos);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBuild();
        }
    }

    void SetPreviewColor(bool canBuild)
    {
        Color targetColor = canBuild ? canBuildColor : cantBuildColor;

        foreach (var r in previewRenderers)
            r.color = targetColor;

        if (outlineRenderer != null)
            outlineRenderer.color = targetColor;
    }

    void CancelBuild()
    {
        isBuildMode = false;

        if (previewTower != null)
            Destroy(previewTower);

        if (outlineObj != null)
            Destroy(outlineObj);
    }

    void PlaceTower(Vector3Int cellPos)
    {
        Vector3 spawnPos = buildableTilemap.GetCellCenterWorld(cellPos);
        Instantiate(selectedTowerPrefab, spawnPos, Quaternion.identity);

        occupiedCells.Add(cellPos);
        GameManager.instance.SpendGold(selectedTowerCost);

        CancelBuild();
    }

}
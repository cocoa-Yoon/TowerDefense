using UnityEngine;
using System.Collections;

public abstract class Tower : MonoBehaviour
{
    [Header("Base Stat")]
    public float damage = 10f;
    public float attackRange = 5f;
    public float attackDelay = 1.5f;
    public float rotateSpeed = 720f;

    [Header("Level")]
    public int level = 1;
    public int maxLevel = 3;
    public int damagePerLevel = 5;
    public float rangePerLevel = 0.5f;
    public float delayPerLevel = 0.1f;

    [Header("Cost")]
    public int upgradeCost = 50;
    public int upgradeCostIncrement = 50;
    public int sellGold = 25;

    [Header("Range Visual")]
    public GameObject rangeObj;

    public Vector3Int placedCell;
    [SerializeField] GameObject disableIcon;
    bool disabled;

    private Coroutine disableRoutine;

    protected virtual void Start()
    {
        UpdateRangeVisual();
        ShowRange(false);
    }

    public void Disable(float time)
    {
        if (disableRoutine != null)
        {
            StopCoroutine(disableRoutine);
        }

        disableRoutine = StartCoroutine(DisableRoutine(time));
    }

    IEnumerator DisableRoutine(float time)
    {
        disabled = true;
        if (disableIcon) disableIcon.SetActive(true);

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.color = new Color(0.5f, 0.5f, 0.5f, 1f);

        yield return new WaitForSeconds(time);

        disabled = false;
        if (disableIcon) disableIcon.SetActive(false);
        
        if (sr != null) sr.color = Color.white;

        disableRoutine = null;
    }

    public bool IsDisabled()
    {
        return disabled;
    }


    #region Upgrade
    public virtual void Upgrade()
    {
        if (level >= maxLevel) return;
        if (GameManager.instance.gold < upgradeCost) return;

        GameManager.instance.SpendGold(upgradeCost);

        SoundManager.Instance.PlayTowerUpgrade();

        level++;
        damage += damagePerLevel;
        attackRange += rangePerLevel;
        attackDelay -= delayPerLevel;
        upgradeCost += upgradeCostIncrement;

        UpdateRangeVisual();
        OnUpgrade();
    }

    protected virtual void OnUpgrade() { }
    #endregion

    #region Range
    public void ShowRange(bool show)
    {
        if (rangeObj != null)
            rangeObj.SetActive(show);
    }

    protected void UpdateRangeVisual()
    {
        if (rangeObj == null) return;

        float d = attackRange * 2f;

        // ⭐ 부모 스케일 보정 (핵심)
        Vector3 parentScale = transform.lossyScale;

        rangeObj.transform.localScale = new Vector3(
            d / parentScale.x,
            d / parentScale.y,
            1f
        );

        // 회전 영향 제거 (선택)
        rangeObj.transform.rotation = Quaternion.identity;
    }
    #endregion

    public virtual void Sell()
    {
        // 💰 골드 환급
        GameManager.instance.AddGold(sellGold);

        SoundManager.Instance.PlayTowerSell();

        // ⭐ 타일 점유 해제
        TowerBuildManager.Instance.UnregisterCell(placedCell);

        Destroy(gameObject);
    }

    public void DestroyTower()
    {
        if (this == null) return;

        if (TowerBuildManager.Instance != null)
        {
            TowerBuildManager.Instance.UnregisterCell(placedCell);
        }

        if (TowerUI.Instance != null) TowerUI.Instance.Hide();

        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        TowerUI.Instance.Show(this);
    }
}
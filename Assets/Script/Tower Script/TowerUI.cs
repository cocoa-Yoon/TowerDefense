using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerUI : MonoBehaviour
{
    public static TowerUI Instance;

    public GameObject panel;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI delayText;
    public TextMeshProUGUI rangeText;

    public Button upgradeButton;
    public Button sellButton;

    public TextMeshProUGUI upgradeButtonText;
    public TextMeshProUGUI sellButtonText;

    private Tower currentTower;
    private bool justOpened;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    void Update()
    {
        if (justOpened) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                Hide();
                currentTower?.ShowRange(false);
            }
        }
    }

    public void Show(Tower tower)
    {
        justOpened = true;

        // ⭐ 이전 타워 range 끄기
        if (currentTower != null)
            currentTower.ShowRange(false);

        currentTower = tower;

        // ⭐ 새 타워 range 켜기
        currentTower.ShowRange(true);

        panel.SetActive(true);
        Refresh();

        Invoke(nameof(ReleaseBlock), 0.05f);
    }

    public void Hide()
    {
        if (currentTower != null)
            currentTower.ShowRange(false);

        currentTower = null;
        panel.SetActive(false);
    }

    public void OnClickUpgrade()
    {
        currentTower.Upgrade();
        Refresh();
    }

    public void OnClickSell()
    {
        if (currentTower == null) return;

        currentTower.Sell();
        Hide();
    }

    void Refresh()
    {
        levelText.text = $"Level {currentTower.level}";
        damageText.text = $"Dmg {currentTower.damage}";
        delayText.text = $"Delay {currentTower.attackDelay:F2}";
        rangeText.text = $"Range {currentTower.attackRange:F1}";

        upgradeButton.interactable =
            currentTower.level < currentTower.maxLevel &&
            GameManager.instance.gold >= currentTower.upgradeCost;
        
        upgradeButtonText.text =
            currentTower.level >= currentTower.maxLevel ?
            "Max Level" :
            $"Upgrade\n{currentTower.upgradeCost}G";
        sellButtonText.text = $"Sell\n{currentTower.sellGold}G";

    }

    void ReleaseBlock()
    {
        justOpened = false;
    }
}
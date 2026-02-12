using UnityEngine;
using UnityEngine.UI;

public class TowerButtonUI : MonoBehaviour
{
    public GameObject towerPrefab;
    public int towerCost;

    public Button button;
    public Image icon;

    Color enableColor = Color.white;
    Color disableColor = new Color(0.7f, 0.7f, 0.7f, 1f);

    void Update()
    {
        bool canBuild = GameManager.instance.gold >= towerCost;
        button.interactable = canBuild;
        icon.color = canBuild ? enableColor : disableColor;
    }

    public void OnClickTower()
    {
        if (GameManager.instance.gold < towerCost) return;

        SoundManager.Instance.PlayUIClick(0);

        TowerBuildManager.Instance.SelectTower(towerPrefab, towerCost);
    }
}
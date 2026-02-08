using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Text")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI waveText;

    [Header("Buttons")]
    public Button speedButton;
    public Button pauseButton;

    [Header("Time Control Icons")]
    public GameObject playIcon;
    public GameObject pauseIcon;
    public Image speedButtonImage;

    public GameObject pauseOverlay;

    [Header("Speed Settings")]
    public float normalSpeed = 1f;
    public float fastSpeed = 2f;

    bool isFast = false;
    bool isPaused = false;

    Color speedNormalColor = Color.white;
    Color speedActiveColor = new Color(0.7f, 0.7f, 0.7f);

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ApplyTimeScale();
        pauseOverlay.SetActive(false);
    }

    // ======================
    // 기존 UI
    // ======================
    public void UpdateGold(int gold)
    {
        goldText.text = $"Gold : {gold} G";
    }

    public void UpdateLife(int life)
    {
        lifeText.text = $"Life : {life} ♥";
    }

    public void UpdateWave(int wave)
    {
        waveText.text = $"Wave : {wave}";
    }

    // ======================
    // Speed Toggle (x1 ↔ x2)
    // ======================
    public void ToggleSpeed()
    {
        isFast = !isFast;
        ApplyTimeScale();
        UpdateSpeedButtonVisual();
    }

    // ======================
    // Pause Toggle (Play ↔ Pause)
    // ======================
    public void TogglePause()
    {
        isPaused = !isPaused;
        ApplyTimeScale();

        pauseOverlay.SetActive(isPaused);

        playIcon.SetActive(isPaused);   // 멈춤 상태 → ▶
        pauseIcon.SetActive(!isPaused); // 재생 상태 → ⏸
    }

    // ======================
    // TimeScale 결정 로직 (핵심)
    // ======================
    void ApplyTimeScale()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = isFast ? fastSpeed : normalSpeed;
        }
    }

    void UpdateSpeedButtonVisual()
    {
        if (speedButtonImage != null)
            speedButtonImage.color = isFast ? speedActiveColor : speedNormalColor;
    }
}
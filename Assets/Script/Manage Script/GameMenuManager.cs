using UnityEngine;

public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager Instance;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject difficultyPanel;
    public GameObject optionPanel;
    public GameObject creditPanel;
    public GameObject gameWinPanel;
    public GameObject gameLosePanel;
    public GameObject sideCanvas; // 인게임 UI

    public Difficulty currentDifficulty = Difficulty.Normal;

    public int startLife = 10;
    public int startGold = 30;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ShowMainMenu();
    }

    // ======================
    // 패널 전환 로직
    // ======================

    public void ShowMainMenu()
    {
        AllClose();
        mainMenuPanel.SetActive(true);
        sideCanvas.SetActive(false);
        Time.timeScale = 0f;
    }

    

    public void OnClickStart()
    {
        AllClose();
        difficultyPanel.SetActive(true);
    }

    public void OnClickOption() { AllClose(); optionPanel.SetActive(true); }
    public void OnClickCredit() { AllClose(); creditPanel.SetActive(true); }
    
    public void OnClickBack() 
    { 
        SoundManager.Instance.PlayBGM(SoundManager.Instance.normalBgm); 
        ShowMainMenu(); 
    }

    public float DifficultyMultiplier
    {
        get
        {
            switch (currentDifficulty)
            {
                case Difficulty.Easy: return 0.8f;
                case Difficulty.Hard: return 1.2f;
                default: return 1f;
            }
        }
    }

    public void SetDifficulty(int level)
    {
        currentDifficulty = (Difficulty)level;
    }    

    public void SelectDifficultyAndStart(int level)
    {
        ResetGame();
        SetDifficulty(level);        
        StartGame();
    }

    public void StartGame()
    {
        ResetGame();

        AllClose();
        sideCanvas.SetActive(true);
        Time.timeScale = 1f;

        Debug.Log($"선택된 난이도: {currentDifficulty} - 게임 시작!");
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;

        GameManager.instance.life = startLife;
        GameManager.instance.gold = startGold;

        UIManager.Instance.UpdateLife(startLife);
        UIManager.Instance.UpdateGold(startGold);

        WaveManager.Instance.ResetWave();

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject m in monsters)
            Destroy(m);

        TowerBuildManager.Instance.ResetTowers();

        Debug.Log("FULL GAME RESET COMPLETE");
    }

    // ======================
    // 게임 종료
    // ======================

    public void GameOver(bool isWin)
    {
        AllClose();
        sideCanvas.SetActive(false);
        Time.timeScale = 0f;
        SoundManager.Instance.StopBGM();

        if (isWin) gameWinPanel.SetActive(true);
        else gameLosePanel.SetActive(true);
    }

    public void OnClickRetry()
    {
        ResetGame();
        SoundManager.Instance.PlayBGM(SoundManager.Instance.normalBgm);
        OnClickStart(); 
    }

    public void OnClickQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void AllClose()
    {
        mainMenuPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        optionPanel.SetActive(false);
        creditPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameLosePanel.SetActive(false);
    }
}
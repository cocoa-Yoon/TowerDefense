using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Status")]
    public int life = 10;
    public int gold = 0;

    void Awake()
    {
        // 싱글톤
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddGold(int reward)
    {
        gold += reward;
    }
         
    public void OnMonsterArrive()
    {
        life--;

        Debug.Log("몬스터 도착! 남은 라이프: " + life);

        if (life <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        // TODO: 게임오버 UI, 정지 처리
        Time.timeScale = 0f;
    }

    
}
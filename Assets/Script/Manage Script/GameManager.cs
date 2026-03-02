using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Status")]
    public int life = 10;
    public int gold = 20;

    void Awake()
    {
        // 싱글톤
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddLife(int reward)
    {
        life = reward;
        UIManager.Instance.UpdateLife(life);
    }

    public void AddGold(int reward)
    {
        gold += reward;
        UIManager.Instance.UpdateGold(gold);
    }

    public void SpendGold(int amount)
    {
        gold -= amount;
        UIManager.Instance.UpdateGold(gold);
    }
         
    public void OnMonsterArrive(bool isBoss)
    {
        if (isBoss)
        {
            GameOver();
            return;
        }

        life--;

        WaveManager.Instance.OnMonsterDead();

        UIManager.Instance.UpdateLife(life);

        if (life <= 0)
        {
            GameOver();
        }
    }
    

    void GameOver()
    {
        Debug.Log("GAME OVER");
        GameMenuManager.Instance.GameOver(false);
        SoundManager.Instance.PlayLose();
        Time.timeScale = 0f;
    }

    
}
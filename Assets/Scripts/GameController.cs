using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public Text scoreCounter;
    public Text enemyCounter;
    public GameObject statsMenu;

    public static GameController instance;

    private GameController() { }

    private int enemiesLeft = 0;

    private Assets.Scripts.Stats stats = Assets.Scripts.Stats.getInstance();

    void Awake()
    {
        // Singleton-related stuff
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        enemiesLeft += GameObject.FindGameObjectsWithTag("Tank").Length;
        enemiesLeft += GameObject.FindGameObjectsWithTag("Turret").Length;
        enemyCounter.text = "Enemies left: " + enemiesLeft.ToString();
        stats.NewRound();
    }

    void PlayerDied()
    {
        stats.PlayerDefeated();
        statsMenu.GetComponent<StatsMenuInGame>().Defeat();
        statsMenu.SetActive(true);
    }

    public void Murder(GameObject whoDied)
    {
        if (whoDied.CompareTag("Player"))
        {
            PlayerDied();
            return;
        }
        if (whoDied.CompareTag("Tank"))
        {
            if (whoDied.GetComponent<AITankControl0>() != null)
                stats.AddScore(20);
            else if (whoDied.GetComponent<AITankControl1>() != null)
                stats.AddScore(30);
            else; // Unknown tank type!
        }
        else if (whoDied.CompareTag("Turret"))
        {
            stats.AddScore(10);
        }
        else; // Who is this anyway? 

        if (scoreCounter != null)
            scoreCounter.text = "Score: " + stats.Score.ToString();
        if(enemyCounter != null)
        enemyCounter.text = "Enemies left: " + (--enemiesLeft).ToString();
        stats.EnemyDefeated();

        if (enemiesLeft == 0)
            Win();
    }  
    
    public void Death(GameObject whoDied)
    {
        if (whoDied.CompareTag("Player"))
        {
            PlayerDied();
            return;
        }
        if (enemyCounter != null)
            enemyCounter.text = "Enemies left: " + (--enemiesLeft).ToString();
        if (enemiesLeft == 0)
            Win();
    } 

    private void Win()
    {
        statsMenu.GetComponent<StatsMenuInGame>().Win();
        statsMenu.SetActive(true);
    }
}

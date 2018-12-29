using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatsMenuInGame : MonoBehaviour {
    public Text State;
    public Text ShotsFired;
    public Text EnemiesDefeated;
    public Text Precision;
    public Text Score;

    void Awake()
    {
        Assets.Scripts.Stats stats = Assets.Scripts.Stats.getInstance();

        ShotsFired.text += stats.ShotCount.ToString();
        EnemiesDefeated.text += stats.EnemiesDefeated.ToString();
        Precision.text += stats.Precision.ToString("F2") + "%";
        Score.text += stats.Score.ToString();
    }    
    

    public void BackPressed()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void Defeat()
    {
        State.text = "Defeat!";
    }

    public void Win()
    {
        State.text = "Win!";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour {
    public GameObject parentMenu;

    public Text ShotsFired;
    public Text EnemiesDefeated;
    public Text Precision;
    public Text Score;
    public Text DeathCount;

    void Start()
    {
        Assets.Scripts.Stats stats = Assets.Scripts.Stats.getInstance();

        ShotsFired.text += stats.OverallShotCount.ToString();
        EnemiesDefeated.text += stats.OverallEnemiesDefeated.ToString();
        Precision.text += stats.OverallPrecision.ToString("F2") + "%";
        Score.text += stats.OverallScore.ToString();
        DeathCount.text += stats.OverallDeathCount.ToString();
    }    
    

    public void BackPressed()
    {
        gameObject.SetActive(false);
        if (parentMenu != null)
            parentMenu.SetActive(true);
    }
}

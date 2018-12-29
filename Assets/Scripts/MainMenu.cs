using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenu : MonoBehaviour {
    public GameObject statsMenu;
    public GameObject levelMenu;

    public void PlayPressed()
    {
        gameObject.SetActive(false);
        levelMenu.SetActive(true);
    }

    public void StatsPressed()
    {
        gameObject.SetActive(false);
        statsMenu.SetActive(true);
    }

    public void ExitPressed()
    {
        Application.Quit();
    }
}

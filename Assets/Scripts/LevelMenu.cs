using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour {
    public GameObject parentMenu;
    public void Level1Pressed()
    {
        SceneManager.LoadScene("Level1");
    }
    public void Level2Pressed()
    {
        SceneManager.LoadScene("Level2");
    }
    public void Level3Pressed()
    {
        SceneManager.LoadScene("Level3");
    }
    public void BackPressed()
    {
        gameObject.SetActive(false);
        if (parentMenu != null)
            parentMenu.SetActive(true);
    }
}

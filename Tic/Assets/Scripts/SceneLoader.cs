using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public static SceneLoader Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void GoToLevel(int world, int level)
    {
        GameData.instance.currentWorld = world;
        GameData.instance.levelToLoad = level;
        SceneManager.LoadScene("Test Level 2");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("New Level Select 2");
    }

    public void QuitGame()
    {

    }
}

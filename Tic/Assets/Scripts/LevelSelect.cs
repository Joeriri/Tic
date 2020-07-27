using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelSelect : MonoBehaviour
{
    Keyboard kb;

    private void Awake()
    {
        kb = InputSystem.GetDevice<Keyboard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (kb.escapeKey.wasPressedThisFrame)
        {
            GoToMainMenu();
        }
    }

    public void GoToLevel(int index)
    {
        SceneManager.LoadScene("Level_" + index.ToString());
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Grid tileGrid;
    LevelLoader levelLoader;
    LevelUI levelUI;
    FadeTransition transition;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;

        tileGrid = FindObjectOfType<Grid>();
        levelLoader = FindObjectOfType<LevelLoader>();
        levelUI = FindObjectOfType<LevelUI>();
        transition = FindObjectOfType<FadeTransition>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 PlaceOnGrid(Vector2 position)
    {
        return (Vector2)tileGrid.CellToWorld(tileGrid.WorldToCell(position)) + new Vector2(tileGrid.cellSize.x * 0.5f, tileGrid.cellSize.y * 0.5f);
    }

    public void LevelWon()
    {
        //unlock next level
    }

    public void LoadNextLevel()
    {
        int nextLevel = levelLoader.levelnumber + 1;
        int nextWorld = levelLoader.worldNumber;

        // if current level is greater then 9, go to next world and next level is 1.
        if (nextLevel > 9)
        {
            nextWorld += 1;
            nextLevel = 1;
        }

        // Load level scene again but with new level/world numbers.
        PlayerPrefs.SetInt("currentLevel", nextLevel);
        PlayerPrefs.SetInt("currentWorld", nextWorld);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {

    }

    IEnumerator FadeIn()
    {
        // fade in
        transition.FadeIn();
        yield return new WaitForSeconds(transition.transitionClip.length);
    }

    IEnumerator FadeOut()
    {
        // fade out
        transition.FadeOut();
        yield return new WaitForSeconds(transition.transitionClip.length);
    }
}

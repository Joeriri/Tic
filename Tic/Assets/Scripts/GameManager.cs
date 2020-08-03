using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Grid tileGrid;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;

        tileGrid = FindObjectOfType<Grid>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 PlaceOnGrid(Vector2 position)
    {
        return (Vector2)tileGrid.CellToWorld(tileGrid.WorldToCell(position)) + new Vector2(tileGrid.cellSize.x * 0.5f, tileGrid.cellSize.y * 0.5f);
    }

    public void GoToLevel(int world, int level)
    {
        GameData.instance.currentWorld = world;
        GameData.instance.levelToLoad = level;
        SceneManager.LoadScene("Test Level 2");
    }
}

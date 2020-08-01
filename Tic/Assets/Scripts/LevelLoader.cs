using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelLoader : MonoBehaviour
{
    public Tilemap wallMap;
    public RuleTile wallTile;

    [SerializeField] private Vector2Int levelSize = new Vector2Int(9,9);
    [SerializeField] private Vector2Int levelOrigin = new Vector2Int(-5, -5);

    // Start is called before the first frame update
    void Start()
    {
        LoadLevel(7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadLevel(int levelNumber)
    {
        // Get text string from text file.
        TextAsset textFile = (TextAsset)Resources.Load("Levels/Level" + levelNumber.ToString(), typeof(TextAsset));
        string text = textFile.text;
        // Get rid of line brakes so we have a continious string.
        text = text.Replace("\n", "").Replace("\r", "");
        // Go through the string and place tiles at the correspinding characters.
        for (int i = 0; i < text.Length; i++)
        {
            // Wall
            if (text[i] == '#')
            {
                // Get the row and column of this character (in 2D space)
                int column = i % levelSize.x;
                int row = i / levelSize.x;
                // Place a wall tile in the same 2D space on the grid.
                // NOTE: we are drawing from bottom left so we have to flip the y drawing order.
                wallMap.SetTile(new Vector3Int(levelOrigin.x + column, levelOrigin.y + levelSize.y - row - 1, 0), wallTile);
            }
        }
    }
}

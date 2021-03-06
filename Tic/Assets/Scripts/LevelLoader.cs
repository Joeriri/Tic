﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Adaptation from this source: https://learntocreategames.com/creating-your-level-from-a-text-file/

public class LevelLoader : MonoBehaviour
{
    [Header("Level Objects")]
    public Tilemap groundMap;
    public Tilemap wallMap;
    public RuleTile wallTile;
    public GameObject playerPrefab;
    public GameObject goalPrefab;

    [Header("Level Information")]
    public int worldNumber = 1;
    public int levelnumber = 1;
    public bool loadCustomLevel = false;
    private Vector2Int levelSize = new Vector2Int(9,9);
    private Vector2Int levelOrigin = new Vector2Int(-5, -5);

    Recolorer worldColor;

    private void Awake()
    {
        worldColor = FindObjectOfType<Recolorer>();

        if (GameData.instance.firstBoot)
        {
            PlayerPrefs.SetInt("currentWorld", worldNumber);
            PlayerPrefs.SetInt("currentLevel", levelnumber);
            GameData.instance.firstBoot = false;
        }
        else
        {
            worldNumber = PlayerPrefs.GetInt("currentWorld");
            levelnumber = PlayerPrefs.GetInt("currentLevel");
        }

        LoadLevel(worldNumber, levelnumber);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void LoadLevel(int worldNumber, int levelNumber)
    {
        // Get text string from text file.
        TextAsset textFile = (TextAsset)Resources.Load("Levels/World" + worldNumber.ToString() + "/Level" + levelNumber.ToString(), typeof(TextAsset));
        string text = textFile.text;
        // Get rid of line brakes so we have a continious string.
        text = text.Replace("\n", "").Replace("\r", "");
        // Go through the string and place tiles at the corresponding characters.
        for (int i = 0; i < text.Length; i++)
        {
            // Wall
            if (text[i] == '#')
            {
                Vector2Int pos = CharacterToPosition(i);
                wallMap.SetTile(new Vector3Int(pos.x, pos.y, 0), wallTile);
            }

            //Player
            if (text[i] == 'S')
            {
                Vector2Int pos = CharacterToPosition(i);
                Instantiate(playerPrefab, new Vector3(pos.x, pos.y, 0), Quaternion.Euler(Vector3.zero));
            }

            //Goal
            if (text[i] == 'O')
            {
                Vector2Int pos = CharacterToPosition(i);
                Instantiate(goalPrefab, new Vector3(pos.x, pos.y, 0), Quaternion.Euler(Vector3.zero));
            }

        }

        // set world palette
        worldColor.SetWorldColors(worldColor.worldColorProfiles[worldNumber - 1]);

        Debug.Log("Loaded World " + worldNumber.ToString() + " Level " + levelNumber.ToString() + ".");
    }

    Vector2Int CharacterToPosition(int index)
    {
        // column is x position.
        int column = index % levelSize.x;
        // row is y position.
        int row = index / levelSize.x;
        // return coordinates as vector. NOTE: we are drawing from bottom left so we have to flip the y drawing order.
        Vector2Int pos = new Vector2Int(levelOrigin.x + column, levelOrigin.y + levelSize.y - row - 1);
        return pos;
    }
}

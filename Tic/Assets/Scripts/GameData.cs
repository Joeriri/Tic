using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source: https://answers.unity.com/questions/1224993/how-to-save-variables-between-scenes.html

public class GameData
{
    // keep constructor private
    private GameData()
    {
    }

    static private GameData _instance;
    static public GameData instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameData();
            return _instance;
        }
    }

    public float extraTime;
    public float extraTimeSetting = 0.0f;
    public float audioVolumeSetting = 1.0f;
    public int levelToLoad = 7;
    public int currentWorld = 1;
}

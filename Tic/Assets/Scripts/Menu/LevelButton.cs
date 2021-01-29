using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int worldNumber;
    public int levelNumber;
    
    // called via NavItem
    void OnConfirm()
    {
        NavManager.Instance.GoToLevel(worldNumber, levelNumber);
    }
}

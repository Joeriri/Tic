using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int worldNumber;
    public int levelNumber;

    // called via NavItem
    void OnHover()
    {
        transform.localScale = new Vector2(1.5f, 1.5f);
    }

    // called via NavItem
    void OnUnhover()
    {
        transform.localScale = Vector2.one;
    }

    // called via NavItem
    void OnConfirm()
    {
        NavManager.Instance.GoToLevel(worldNumber, levelNumber);
    }
}

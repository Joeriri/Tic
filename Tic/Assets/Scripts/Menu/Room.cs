using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Level Selection")]
    public bool isLevelWorld = false;
    public int worldNumber;

    Recolorer worldColor;

    private void Awake()
    {
        worldColor = FindObjectOfType<Recolorer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "MainCamera" && isLevelWorld)
        {
            worldColor.StartWorldRecolor(worldColor.worldColorProfiles[worldNumber - 1]);
        }
    }

    
}

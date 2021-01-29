using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverDisplayObject : MonoBehaviour
{
    public GameObject displayableObject;
    
    void OnActivate()
    {
        DisplayObject(true);
    }

    void OnDeactivate()
    {
        DisplayObject(false);
    }

    void DisplayObject(bool display)
    {
        if (displayableObject != null)
        {
            displayableObject.SetActive(display);
        }
        else
        {
            Debug.LogWarning(gameObject.name + ": No GameObject set!");
        }
    }
}

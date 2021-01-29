using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayObject : MonoBehaviour
{
    public GameObject displayableObject;

    public void Display(bool displayObject)
    {
        if (displayableObject != null)
        {
            displayableObject.SetActive(displayObject);
        }
        else
        {
            Debug.LogWarning(gameObject.name + ": No GameObject set!");
        }
    }
}

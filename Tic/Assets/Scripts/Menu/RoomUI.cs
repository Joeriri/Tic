using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUI : MonoBehaviour
{
    public GameObject hoverObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnHover()
    {
        hoverObjects.SetActive(true);
    }

    void OnUnhover()
    {
        hoverObjects.SetActive(false);
    }
}

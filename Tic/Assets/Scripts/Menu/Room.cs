using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject hoverObjects;

    void OnEnterRoom()
    {
        if (hoverObjects != null)
        {
            hoverObjects.SetActive(true);
        }
    }

    void OnLeaveRoom()
    {
        if (hoverObjects != null)
        {
            hoverObjects.SetActive(false);
        }
    }
}

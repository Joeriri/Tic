using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = GameManager.Instance.PlaceOnGrid(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("thing collided");
        if (collider.GetComponent<Player>() != null)
        {
            Debug.Log("player collided");
            //collider.GetComponent<Player>().WinLevel();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NavItem : MonoBehaviour
{
    public Room room;

    [Header("Routes")]
    public UnityEvent leftEvent;
    public UnityEvent rightEvent;
    public UnityEvent upEvent;
    public UnityEvent downEvent;
}

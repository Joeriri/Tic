using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NavItem : MonoBehaviour
{
    [Header("Routes")]
    public UnityEvent leftEvent;
    public UnityEvent rightEvent;
    public UnityEvent upEvent;
    public UnityEvent downEvent;

    
}

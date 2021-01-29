using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NavItem : MonoBehaviour
{
    [Header("Info")]
    public Room room;
    
    [Header("Routes")]
    public UnityEvent leftEvent;
    public UnityEvent rightEvent;
    public UnityEvent upEvent;
    public UnityEvent downEvent;

    [Header("Events")]
    public UnityEvent onActivation;
    public UnityEvent onDeactivation;

    public void Activate()
    {
        onActivation.Invoke();
        gameObject.SendMessage("OnActivate", SendMessageOptions.DontRequireReceiver);
    }

    public void Deactivate()
    {
        onDeactivation.Invoke();
        gameObject.SendMessage("OnDeactivate", SendMessageOptions.DontRequireReceiver);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NavManager : MonoBehaviour
{
    public GameObject levelSelect;
    public NavItem activeNavItem;
    public float camSwipeDuration = 1.0f;

    [Header("Camera-influenced NavItems")]
    public NavItem settingsNavItem;
    public NavItem titleNavItem;
    public NavItem[] worldNavItems;

    [Header("Camera Positions")]
    public Transform settingsPos;
    public Transform titlePos;
    public Transform levelSelectPos;

    [Header("World Selection")]
    public int roomWidth = 23;
    public int roomPadding = 3;
    public AnimationCurve worldSelectSwipeCurve;

    private bool canNavigate = true;

    Keyboard kb;
    CameraMovement camMovement;

    private void Awake()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        camMovement = Camera.main.GetComponent<CameraMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Input

    void OnLeft()
    {
        if (canNavigate)
            activeNavItem.leftEvent.Invoke();
    }

    void OnRight()
    {
        if (canNavigate)
            activeNavItem.rightEvent.Invoke();
    }

    void OnUp()
    {
        if (canNavigate)
            activeNavItem.upEvent.Invoke();
    }

    void OnDown()
    {
        if (canNavigate)
            activeNavItem.downEvent.Invoke();
    }

    void OnConfirm()
    {
        if (canNavigate)
            activeNavItem.gameObject.SendMessage("OnConfirm", SendMessageOptions.DontRequireReceiver);
    }

    #endregion

    public void SetNavItemActive(NavItem newNavItem)
    {
        activeNavItem.gameObject.SendMessage("OnUnhover", SendMessageOptions.DontRequireReceiver);
        activeNavItem = newNavItem;
        newNavItem.gameObject.SendMessage("OnHover", SendMessageOptions.DontRequireReceiver);
    }

    public void GoToSettings()
    {
        
        StartCoroutine(MoveCameraToPosition(settingsPos.position, settingsNavItem));
    }

    public void GoToTitle()
    {
        StartCoroutine(MoveCameraToPosition(titlePos.position, titleNavItem));
    }

    public void GoToLevelSelect()
    {
        StartCoroutine(MoveCameraToPosition(levelSelectPos.position, worldNavItems[GameData.instance.currentWorld - 1]));
    }

    IEnumerator MoveCameraToPosition(Vector3 targetPosition, NavItem newNavItem)
    {
        // unhover current nav item
        activeNavItem.gameObject.SendMessage("OnUnhover", SendMessageOptions.DontRequireReceiver);
        //disable nav
        canNavigate = false;
        // move camera
        camMovement.GoToPosition(targetPosition, camSwipeDuration);
        yield return new WaitForSeconds(camSwipeDuration);
        // when done, renable nav
        canNavigate = true;
        // set new current nav and hover it
        activeNavItem = newNavItem;
        activeNavItem.gameObject.SendMessage("OnHover", SendMessageOptions.DontRequireReceiver);
    }

    public void GoToWorld(int targetWorld)
    {
        GameData.instance.currentWorld = targetWorld;
        StartCoroutine(MoveWorldSelect(targetWorld));
    }

    IEnumerator MoveWorldSelect(int targetWorld)
    {
        //unhover current NavItem
        activeNavItem.gameObject.SendMessage("OnUnhover", SendMessageOptions.DontRequireReceiver);
        //disable nav
        canNavigate = false;

        // swipe world ("camera")
        Vector3 startPosition = levelSelect.transform.position;
        Vector3 targetPosition = new Vector3((targetWorld - 1) * -(roomWidth + roomPadding), levelSelect.transform.position.y, 0);
        float duration = camSwipeDuration;

        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            float prc = timer / duration;
            levelSelect.transform.position = Vector3.Lerp(startPosition, targetPosition, worldSelectSwipeCurve.Evaluate(prc));

            yield return null;
        }

        // when done, renable nav
        canNavigate = true;
        // set new current nav and hover over it
        activeNavItem = worldNavItems[targetWorld - 1];
        activeNavItem.gameObject.SendMessage("OnHover", SendMessageOptions.DontRequireReceiver);
    }
}

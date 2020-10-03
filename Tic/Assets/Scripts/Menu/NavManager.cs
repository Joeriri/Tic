using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NavManager : MonoBehaviour
{
    public NavItem currentNavItem;

    [Header("Scene Transition")]
    public Transition transition;

    [Header("Camera")]
    public float camSwipeDuration = 1.0f;
    public float camSwipeNavDisableDuration = 0.5f;
    public AnimationCurve camSwipeCurve;

    [Header("Room NavItems")]
    public NavItem settingsNavItem;
    public NavItem titleNavItem;
    public NavItem[] worldNavItems;
    
    [Header("World Selection")]
    public int roomWidth = 23;
    public int roomPadding = 3;
    public WorldRecolorer worldRrecolorer;
    
    bool canNavigate = true;
    bool swipingCamera = false;
    Coroutine camSwipeRoutine;

    Keyboard kb;
    CameraMovement camMovement;

    public static NavManager Instance;

    private void Awake()
    {
        Instance = this;

        kb = InputSystem.GetDevice<Keyboard>();
        camMovement = Camera.main.GetComponent<CameraMovement>();
    }

    private void Start()
    {
        StartCoroutine(TransitionIn());
    }

    #region Input

    void OnLeft()
    {
        if (canNavigate)
            currentNavItem.leftEvent.Invoke();
    }

    void OnRight()
    {
        if (canNavigate)
            currentNavItem.rightEvent.Invoke();
    }

    void OnUp()
    {
        if (canNavigate)
            currentNavItem.upEvent.Invoke();
    }

    void OnDown()
    {
        if (canNavigate)
            currentNavItem.downEvent.Invoke();
    }

    void OnConfirm()
    {
        if (canNavigate)
            currentNavItem.gameObject.SendMessage("OnConfirm", SendMessageOptions.DontRequireReceiver);
    }

    void OnBack()
    {
        if (canNavigate)
        {
            // if not on title, go to title
            if (currentNavItem != titleNavItem)
            {
                GoToTitle();
            }
            // if on title, quit game
            else
            {
                QuitGame();
            }
        }
        
    }

    #endregion

    public void GoToTitle()
    {

    }

    public void GoToSettings()
    {

    }

    public void SetNavItemActive(NavItem newNavItem)
    {
        currentNavItem.gameObject.SendMessage("OnUnhover", SendMessageOptions.DontRequireReceiver);
        currentNavItem = newNavItem;
        newNavItem.gameObject.SendMessage("OnHover", SendMessageOptions.DontRequireReceiver);
    }

    public void GoToWorld(int world)
    {
        GameData.instance.currentWorld = world;
    }
    
    #region Scene Management And Transitions

    IEnumerator TransitionIn()
    {
        canNavigate = false;
        currentNavItem.gameObject.SendMessage("OnUnhover", SendMessageOptions.DontRequireReceiver);
        // do transition
        transition.StartTransitionIn();
        yield return new WaitForSeconds(transition.transitionClip.length);
        // enable navigation
        canNavigate = true;
        currentNavItem.gameObject.SendMessage("OnHover", SendMessageOptions.DontRequireReceiver);
    }

    public void QuitGame()
    {
        StartCoroutine(TransitionToQuit());
    }

    IEnumerator TransitionToQuit()
    {
        canNavigate = false;
        currentNavItem.gameObject.SendMessage("OnUnhover", SendMessageOptions.DontRequireReceiver);
        // do transition
        transition.StartTransitionOut();
        yield return new WaitForSeconds(transition.transitionClip.length);
        // quit game
        yield return new WaitForSeconds(0.2f);
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void GoToLevel(int world, int level)
    {
        GameData.instance.currentWorld = world;
        GameData.instance.levelToLoad = level;
        StartCoroutine(TransitionToLevel());
    }

    IEnumerator TransitionToLevel()
    {
        canNavigate = false;
        // do transition
        transition.StartTransitionOut();
        yield return new WaitForSeconds(transition.transitionClip.length);
        // go to scene
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Test Level 2");
    }

    #endregion

    //------------------------------------------------------------------------------------------------------------------------------------------------------

    public void MoveToNavItem(NavItem targetNavItem)
    {
        currentNavItem.gameObject.SendMessage("OnDeactivated", SendMessageOptions.DontRequireReceiver);
        
        if (targetNavItem.room == currentNavItem.room)
        {
            // if nav item is in same room
            currentNavItem = targetNavItem;
            currentNavItem.gameObject.SendMessage("OnActivated", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            // if nav item is in another oom
            currentNavItem.room.SendMessage("OnLeaveRoom", SendMessageOptions.DontRequireReceiver);
            currentNavItem = targetNavItem;
            MoveToRoom(targetNavItem.room, targetNavItem);
        }
    }

    public void MoveToRoom(Room targetRoom, NavItem newNavItem)
    {
        if (swipingCamera)
            StopCoroutine(camSwipeRoutine);
        camSwipeRoutine = StartCoroutine(MoveToRoomRoutine(targetRoom, newNavItem));
    }

    IEnumerator MoveToRoomRoutine(Room targetRoom, NavItem newNavItem)
    {
        swipingCamera = true;
        canNavigate = false;
        
        // get some refs
        float timer = 0;
        float duration = camSwipeDuration;
        Camera cam = Camera.main;
        Vector3 startPos = cam.transform.position;
        Vector3 targetPos = new Vector3(targetRoom.transform.position.x, targetRoom.transform.position.y, -10);

        // routine
        while (timer < duration)
        {
            timer += Time.deltaTime;

            // swipe camera to room
            float prc = timer / duration;
            cam.transform.position = Vector3.Lerp(startPos, targetPos, camSwipeCurve.Evaluate(prc));

            //// enable navigation before finishing the camera movement
            //if (timer > camSwipeNavDisableDuration && !canNavigate)
            //{
            //    canNavigate = true;
            //}

            yield return null;
        }
        cam.transform.position = targetPos;

        swipingCamera = false;
        canNavigate = true;

        currentNavItem.room.SendMessage("OnEnterRoom", SendMessageOptions.DontRequireReceiver);
        currentNavItem.gameObject.SendMessage("OnActivated", SendMessageOptions.DontRequireReceiver);
    }

    public void SetCurrentNavItem(NavItem newNavItem)
    {
        currentNavItem = newNavItem;
    }

    void FinishIntro()
    {
        canNavigate = true;
        currentNavItem.SendMessage("OnActivated");
        currentNavItem.room.SendMessage("OnEnterRoom");
    }
}

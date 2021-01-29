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
        
    [Header("World Selection")]
    public int roomWidth = 23;
    public int roomPadding = 3;
    public WorldRecolorer worldRrecolorer;
    
    bool canNavigate = true;
    bool swipingCamera = false;
    Coroutine camSwipeRoutine;

    Keyboard kb;
    CameraMovement camMovement;
    Recolorer worldColor;
    NavItem titleNav;

    public static NavManager Instance;

    private void Awake()
    {
        Instance = this;

        kb = InputSystem.GetDevice<Keyboard>();
        camMovement = Camera.main.GetComponent<CameraMovement>();
        worldColor = FindObjectOfType<Recolorer>();
        titleNav = GameObject.FindGameObjectWithTag("Title Nav").GetComponent<NavItem>();
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
            if (currentNavItem != titleNav)
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
    
    //------------------------------------------------------------------------------------------------------------------------------------------------------

    void GoToTitle()
    {
        MoveToNavItem(titleNav);
    }

    void QuitGame()
    {

    }

    #region Scene Management And Transitions

    IEnumerator TransitionIn()
    {
        //disable navigation
        canNavigate = false;
        currentNavItem.Deactivate();
        // do transition
        transition.StartTransitionIn();
        yield return new WaitForSeconds(transition.transitionClip.length);
        // enable navigation
        canNavigate = true;
        currentNavItem.Activate();
    }

    public void GoToLevel(int world, int level)
    {
        PlayerPrefs.SetInt("currentLevel", level);
        PlayerPrefs.SetInt("currentWorld", world);
        SceneManager.LoadScene("Test Level 2");
    }

    #endregion

    public void MoveToNavItem(NavItem targetNavItem)
    {
        StartCoroutine(MoveToNavItemRoutine(targetNavItem));
    }

    IEnumerator MoveToNavItemRoutine(NavItem targetNavItem)
    {
        // disable current nav item
        currentNavItem.Deactivate();
        
        // when target NavItem is not in the same room, move to correct room
        if (targetNavItem.room != currentNavItem.room)
        {
            MoveToRoom(targetNavItem.room);
            yield return new WaitForSeconds(camSwipeDuration);
        }
        
        // change current nav item and enable it!
        currentNavItem = targetNavItem;
        currentNavItem.Activate();
    }

    public void MoveToRoom(Room targetRoom)
    {
        if (swipingCamera)
            StopCoroutine(camSwipeRoutine);
        camSwipeRoutine = StartCoroutine(MoveToRoomRoutine(targetRoom));
    }

    IEnumerator MoveToRoomRoutine(Room targetRoom)
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
    }
}

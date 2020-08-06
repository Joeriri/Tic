﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NavManager : MonoBehaviour
{
    public NavItem currentNavItem;
    public Room currentRoom;

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

    [Header("Rooms")]
    public Room titleRoom;
    public Room settingsRoom;
    public Room[] worldRooms;

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

    #endregion

    public void SetNavItemActive(NavItem newNavItem)
    {
        currentNavItem.gameObject.SendMessage("OnUnhover", SendMessageOptions.DontRequireReceiver);
        currentNavItem = newNavItem;
        newNavItem.gameObject.SendMessage("OnHover", SendMessageOptions.DontRequireReceiver);
    }

    public void GoToTitle()
    {
        GoToRoom(titleRoom, titleNavItem);
    }

    public void GoToSettings()
    {
        GoToRoom(settingsRoom, settingsNavItem);
    }

    public void GoToWorld(int world)
    {
        GameData.instance.currentWorld = world;
        GoToRoom(worldRooms[world -1], worldNavItems[world - 1]);
    }

    private void GoToRoom(Room room, NavItem navItem)
    {
        if (swipingCamera)
            StopCoroutine(camSwipeRoutine);
        camSwipeRoutine = StartCoroutine(MoveToRoom(room, navItem));
    }

    IEnumerator MoveToRoom(Room targetRoom, NavItem targetNavItem)
    {
        swipingCamera = true;
        canNavigate = false;
        // tell current NavItem it is no longer hovered
        currentNavItem.gameObject.SendMessage("OnUnhover", SendMessageOptions.DontRequireReceiver);

        // get some refs
        float timer = 0;
        float duration = camSwipeDuration;
        Camera cam = Camera.main;
        Vector3 startPos = new Vector3(currentRoom.transform.position.x, currentRoom.transform.position.y, -10);
        Vector3 targetPos = new Vector3(targetRoom.transform.position.x, targetRoom.transform.position.y, -10);

        while (timer < duration)
        {
            timer += Time.deltaTime;

            // swipe camera to room
            float prc = timer / duration;
            cam.transform.position = Vector3.Lerp(startPos, targetPos, camSwipeCurve.Evaluate(prc));
            

            // enable navigation before finishing the camera movement
            if (timer > camSwipeNavDisableDuration && !canNavigate)
            {
                canNavigate = true;
                currentRoom = targetRoom;
                currentNavItem = targetNavItem;
            }

            yield return null;
        }

        swipingCamera = false;
        // when done with the camera swipe, tell current Navitem it is hovered
        currentNavItem.gameObject.SendMessage("OnHover", SendMessageOptions.DontRequireReceiver);
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
        yield return new WaitForSeconds(0.5f);
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
        SceneManager.LoadScene("Test Level 2");
    }

    #endregion
}

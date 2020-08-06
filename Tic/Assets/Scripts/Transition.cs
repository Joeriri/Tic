using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [Header("Clips")]
    public AnimationClip transitionClip;

    Animator animator;
    Animator camAnimator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        camAnimator = Camera.main.GetComponent<Animator>();
    }

    public void StartTransitionIn()
    {
        animator.Play("TransitionIn");
        camAnimator.Play("CameraTransitionIn");
    }

    public void StartTransitionOut()
    {
        animator.Play("TransitionOut");
        camAnimator.Play("CameraTransitionOut");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    [Header("Clip")]
    public AnimationClip transitionClip;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        animator.Play("FadeIn");
    }

    public void FadeOut()
    {
        animator.Play("FadeOut");
    }
}

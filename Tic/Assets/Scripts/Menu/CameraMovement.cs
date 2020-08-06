﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public AnimationCurve swipeCurve;

    private Coroutine swipeRoutine;

    public void GoToPosition(Vector3 pos, float duration)
    {
        // stop current routine if it's allready running. Then start routine.
        if (swipeRoutine != null) StopCoroutine(swipeRoutine);
        swipeRoutine = StartCoroutine(MoveToPos(transform.position, pos, duration));
    }

    IEnumerator MoveToPos(Vector3 startPos, Vector3 targetPos, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            float prc = timer / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, swipeCurve.Evaluate(prc));

            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverScaleEffect : MonoBehaviour
{
    public float scaleDuration = 0.1f;
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 0f, 1.0f, 1.0f);
    [Header("Active")]
    public Vector2 activeBoxScale = new Vector2(1f, 1f);
    //NOTE: inactive box size is the size of the box on start of scene
    //[Header("Inactive")]
    //public Vector2 inactiveBoxSize;
    
    private Vector2 inactiveBoxScale = new Vector2(1f, 1f);

    Image box;

    Coroutine boxScaleRoutine;

    private void Awake()
    {
        box = GetComponent<Image>();
    }
    
    void OnActivate()
    {
        StartScale(activeBoxScale);
    }

    void OnDeactivate()
    {
        StartScale(inactiveBoxScale);
    }

    void StartScale(Vector2 targetScale)
    {
        if (boxScaleRoutine != null)
            StopCoroutine(boxScaleRoutine);
        boxScaleRoutine = StartCoroutine(ScaleSize(targetScale));
    }

    IEnumerator ScaleSize(Vector2 targetScale)
    {
        Vector2 startScale = box.rectTransform.localScale;

        float timer = 0;
        float prc = 0;
        while (timer < scaleDuration)
        {
            timer += Time.deltaTime;
            prc = timer / scaleDuration;
            
            box.rectTransform.localScale = Vector2.Lerp(startScale, targetScale, scaleCurve.Evaluate(prc));

            yield return null;
        }

        box.rectTransform.localScale = targetScale;
    }
}

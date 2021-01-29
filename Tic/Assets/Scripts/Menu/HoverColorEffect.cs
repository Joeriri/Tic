using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverColorEffect : MonoBehaviour
{
    public float fadeDuration = 0.1f;
    [Header("Active")]
    public Color activeTextColor = Color.black;
    public Color activeBoxColor = Color.white;
    [Header("Inactive")]
    public Color inactiveTextColor = new Color32(100, 100, 100, 255);
    public Color inactiveBoxColor = new Color32(200, 200, 200, 255);

    Text text;
    Image box;

    Coroutine colorFadeRoutine;

    private void Awake()
    {
        box = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
    }

    void OnActivate()
    {
        StartFade(activeTextColor, activeBoxColor);
    }

    void OnDeactivate()
    {
        StartFade(inactiveTextColor, inactiveBoxColor);
    }

    void StartFade(Color textTargetColor, Color boxTargetColor)
    {
        if (colorFadeRoutine != null)
            StopCoroutine(colorFadeRoutine);
        colorFadeRoutine = StartCoroutine(FadeColor(textTargetColor, boxTargetColor));
    }

    IEnumerator FadeColor(Color textTargetColor, Color boxTargetColor)
    {
        Color textStartColor = text.color;
        Color boxStartColor = box.color;

        float timer = 0;
        float prc = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            prc = timer / fadeDuration;

            text.color = Color.Lerp(textStartColor, textTargetColor, prc);
            box.color = Color.Lerp(boxStartColor, boxTargetColor, prc);

            yield return null;
        }

        text.color = textTargetColor;
        box.color = boxTargetColor;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SliderSetting : MonoBehaviour
{
    public int value;
    public int maxValue;
    public RectTransform fillRect;
    public RectTransform handleRect;
    public UnityEvent onValueChange;

    private RectTransform barRect;

    private void Awake()
    {
        barRect = GetComponent<RectTransform>();
    }

    public void IncreaseValue()
    {
        if (value < maxValue)
        {
            value++;
            UpdateSlider();
        }
    }

    public void DecreaseValue()
    {
        if (value > 0)
        {
            value--;
            UpdateSlider();
        }
    }

    public void SetValue(int newValue)
    {
        if (newValue > 0 && newValue < maxValue)
        {
            value = newValue;
            UpdateSlider();
        }
    }
    
    public void UpdateSlider()
    {
        float percentageFilled = (float)value / maxValue;
        // fill bar
        fillRect.anchoredPosition = new Vector2(barRect.anchoredPosition.x - barRect.sizeDelta.x * 0.5f + percentageFilled * barRect.sizeDelta.x * 0.5f, fillRect.anchoredPosition.y);
        fillRect.sizeDelta = new Vector2(percentageFilled * barRect.sizeDelta.x, fillRect.sizeDelta.y);
        // set handle position
        float width = barRect.sizeDelta.x - handleRect.sizeDelta.x;
        float offset = Mathf.Lerp(0, width, percentageFilled);
        handleRect.anchoredPosition = new Vector2(barRect.anchoredPosition.x - (barRect.sizeDelta.x - handleRect.sizeDelta.x) * 0.5f + offset, handleRect.anchoredPosition.y);

        onValueChange.Invoke();
    }
}

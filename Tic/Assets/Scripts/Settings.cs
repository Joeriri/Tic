using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

// Audio stuff is from http://answers.unity.com/answers/1586797/view.html.
// Also helpful documentation on the audio mixer: https://docs.unity3d.com/Manual/AudioMixerOverview.html.

public class Settings : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider audioSlider;
    public Text audioText;

    [Header("Accessibility")]
    public Slider addedTimeSlider;
    [SerializeField] private float maxAddedTime = 1.0f;
    public Text addedTimeText;

    private Keyboard kb;

    private void Awake()
    {
        // update audio slider
        audioSlider.value = Data.instance.audioVolumeSetting * audioSlider.maxValue;
        ChangeAudioVolume();
        // update extra time slider
        addedTimeSlider.value = Data.instance.extraTimeSetting * addedTimeSlider.maxValue;
        ChangeAddedTime();

        kb = InputSystem.GetDevice<Keyboard>();
    }

    private void Update()
    {
        if (kb.escapeKey.wasPressedThisFrame)
        {
            GoBackToMainMenu();
        }
    }

    public void ChangeAddedTime()
    {
        float newValue = (addedTimeSlider.value / addedTimeSlider.maxValue) * maxAddedTime;
        Data.instance.extraTime = newValue;
        Data.instance.extraTimeSetting = addedTimeSlider.value / addedTimeSlider.maxValue;

        int seconds = Mathf.FloorToInt(newValue);
        int tenths = Mathf.FloorToInt(newValue * 10f) % 10;
        addedTimeText.text = "+ " + string.Format("{0:0}.{1:0}", seconds, tenths) + " s";
    }

    public void ChangeAudioVolume()
    {
        audioMixer.SetFloat("MasterVol", ConvertToDecibel(audioSlider.value / audioSlider.maxValue));
        Data.instance.audioVolumeSetting = audioSlider.value / audioSlider.maxValue;

        audioText.text = audioSlider.value.ToString("00");
    }

    /// <summary>
    /// Converts a percentage fraction to decibels,
    /// with a lower clamp of 0.0001 for a minimum of -80dB, same as Unity's Mixers.
    /// </summary>
    public float ConvertToDecibel(float _value)
    {
        return Mathf.Log10(Mathf.Max(_value, 0.0001f)) * 20f;
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

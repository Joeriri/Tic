using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

// Audio stuff is from http://answers.unity.com/answers/1586797/view.html.
// Also helpful documentation on the audio mixer: https://docs.unity3d.com/Manual/AudioMixerOverview.html.

public class Settings : MonoBehaviour
{
    [Header("Volume")]
    public AudioMixer audioMixer;
    public SliderSetting volumeSlider;
    public Text volumeText;
    
    [Header("Extra Time")]
    public float maxExtraTime = 1.0f;
    public SliderSetting extraTimeSlider;
    public Text extraTimeText;
    
    private void Start()
    {
        // on first startup
        if (!PlayerPrefs.HasKey("volumeSetting"))
        {
            PlayerPrefs.SetFloat("volumeSetting", 1.0f);
        }

        // set slider values and visuals to saved values (which then sets saved values again but whatever)
        volumeSlider.SetValue((int)(PlayerPrefs.GetFloat("volumeSetting") * volumeSlider.maxValue));
        extraTimeSlider.SetValue((int)(PlayerPrefs.GetFloat("addedTimeSetting") * extraTimeSlider.maxValue));
    }

    public void ChangeExtraTime()
    {
        float setting = (float)extraTimeSlider.value / extraTimeSlider.maxValue;
        float extraTime = setting * maxExtraTime;

        PlayerPrefs.SetFloat("addedTime", extraTime);
        PlayerPrefs.SetFloat("addedTimeSetting", setting);

        int seconds = Mathf.FloorToInt(extraTime);
        int tenths = Mathf.FloorToInt(extraTime * 10f) % 10;
        extraTimeText.text = "Extra Time: + " + string.Format("{0:0}.{1:0}", seconds, tenths) + " s";
    }

    public void ChangeAudioVolume()
    {
        float setting = (float)volumeSlider.value / volumeSlider.maxValue;

        audioMixer.SetFloat("MasterVol", ConvertToDecibel(setting));
        PlayerPrefs.SetFloat("volumeSetting", setting);

        volumeText.text = "Volume: " + (setting * 10).ToString("00");
    }

    /// <summary>
    /// Converts a percentage fraction to decibels,
    /// with a lower clamp of 0.0001 for a minimum of -80dB, same as Unity's Mixers.
    /// </summary>
    public float ConvertToDecibel(float _value)
    {
        return Mathf.Log10(Mathf.Max(_value, 0.0001f)) * 20f;
    }
}

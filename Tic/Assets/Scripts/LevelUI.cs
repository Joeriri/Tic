using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelUI : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private int levelNumber = 0;
    public Text levelNumberText;
    public Text timerText;
    public Image healthBar;

    [Header("Win Screen")]
    [SerializeField] private float waitForContinueDuration = 2.0f;
    public GameObject winScreen;
    public GameObject buttons;
    public Text totalTimeText;

    private float origHealthBarWidth;

    Keyboard kb;

    private void Awake()
    {
        kb = InputSystem.GetDevice<Keyboard>();

        origHealthBarWidth = healthBar.rectTransform.sizeDelta.x;
    }

    // Start is called before the first frame update
    void Start()
    {
        levelNumberText.text = levelNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (kb.escapeKey.wasPressedThisFrame)
        {
            GoToLevelSelect();
        }

        if (buttons.activeInHierarchy && levelNumber != 9 && (kb.rightArrowKey.wasPressedThisFrame || kb.dKey.wasPressedThisFrame || kb.lKey.wasPressedThisFrame))
        {
            GoToNextLevel();
        }
    }

    public void SetHealthBarValue(float value)
    {
        float newWidth = value * origHealthBarWidth;
        healthBar.rectTransform.sizeDelta = new Vector2(newWidth, healthBar.rectTransform.sizeDelta.y);
    }

    public void SetTimerText(float timeLeft)
    {
        timerText.text = FormatTimeThousand(timeLeft)  + " s";
    }

    public void SetTotalTimeText(float timeTaken, float maxTime)
    {
        totalTimeText.text = FormatTimeThousand(timeTaken) + " s" + " / " + FormatTimeTen(maxTime) + " s";
    }

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
        StartCoroutine(ShowButtonsRoutine());
    }

    public void GoToLevelSelect()
    {
        SceneManager.LoadScene("Level Select");
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene("Level_" + (levelNumber + 1).ToString());
    }

    IEnumerator ShowButtonsRoutine()
    {
        yield return new WaitForSeconds(waitForContinueDuration);
        buttons.SetActive(true);
    }

    // Ik gebruik deze niet, maar komt van pas om te hebben
    public string FormatTime(float time)
    {
        int d = (int)(time * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        int hundredths = d % 100;
        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
    }

    public string FormatTimeThousand(float time)
    {
        int d = (int)(time * 1000.0f);
        int seconds = d / 1000;
        int thousandths = d % 1000;

        return string.Format("{0:0}.{1:000}", seconds, thousandths);
    }

    public string FormatTimeTen(float time)
    {
        int d = (int)(time * 10.0f);
        int seconds = d / 10;
        int tenths = d % 10;

        return string.Format("{0:0}.{1:0}", seconds, tenths);
    }
}

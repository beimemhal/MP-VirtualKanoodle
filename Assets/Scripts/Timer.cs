using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static float timeValue = 0;

    public TMP_Text timeText;
    public static string timerText = "00:00:000";

    void Start()
    {
        timeText.text = "0";
    }

    void Update()
    {
        if (!GameManager.won)
        {
            timeValue += Time.deltaTime;

            timeText.text = DisplayTime(timeValue);
            timerText = timeText.text;
        }
    }

    public static string DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); // two digits as an int
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = (timeToDisplay % 1) * 1000; // three digits as an int

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);

        // TODO maybe if an hour+ quit application
    }
}

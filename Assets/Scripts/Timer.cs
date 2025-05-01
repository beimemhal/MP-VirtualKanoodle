using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer staticTime;

    public float timeValue = 0;

    public TMP_Text timeText;
    public string timerText = "00:00:000";

    void Start()
    {
        staticTime = this;
    }

    void Update()
    {
        if (!GameManager.won)
        {
            this.timeValue += Time.deltaTime;

            timeText.text = DisplayTime(this.timeValue);
            timerText = timeText.text;
        }
    }

    public static string DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); // two digits as an int
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = (timeToDisplay % 1) * 1000; // three digits as an int

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
    }
}

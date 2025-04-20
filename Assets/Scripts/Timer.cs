using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static float timeValue = 0;

    public static int timerValueText = 0;

    public static TMP_Text timeText;

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
        }
        else
        {
            // int a = Mathf.FloorToInt(Timer.timeValue / 60) * 10000000; // first two digits
            // int b = Mathf.FloorToInt(Timer.timeValue % 60) * 100000; // third and forth digits
            // int c = (int)(Timer.timeValue % 1) * 1000; // last three digits
            timerValueText = Mathf.FloorToInt(Timer.timeValue / 60) * 10000000 + Mathf.FloorToInt(Timer.timeValue % 60) * 100000 + (int)(Timer.timeValue % 1) * 1000;
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

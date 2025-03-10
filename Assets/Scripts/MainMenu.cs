using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play() // TODO difficulty levels
    {
        SceneManager.LoadScene("GamePlayScene");
    }

    public void Quit() // only useable if desktop application
    {
        Application.Quit();
    }

    // info: How to play & credits TODO ? later

    // (settings? -> sound) TODO later

}

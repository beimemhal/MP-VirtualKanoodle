using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Text infoButtonText;
    public GameObject infoText;
    public TMP_Text mainMenuText;
    public GameObject difficulties;
    public GameObject leaderboardButton;

    // starts gameplay scene with 10 initially placed pieces (2 left for player)
    public void PlayEasy()
    {
        SolutionManager.difficulty = 10;
        SceneManager.LoadScene("GamePlayScene");
    }

    // starts gameplay scene with 9 initially placed pieces
    public void PlayMedium()
    {
        SolutionManager.difficulty = 9;
        SceneManager.LoadScene("GamePlayScene");
    }

    // starts gameplay scene with 8 initially placed pieces
    public void PlayHard()
    {
        SolutionManager.difficulty = 8;
        SceneManager.LoadScene("GamePlayScene");
    }

    // starts gameplay scene with 7 initially placed pieces
    public void PlayExtraHard()
    {
        SolutionManager.difficulty = 7;
        SceneManager.LoadScene("GamePlayScene");
    }

    // starts gameplay scene with 5 initially placed pieces
    public void PlayExtreme()
    {
        SolutionManager.difficulty = 5;
        SceneManager.LoadScene("GamePlayScene");
    }
    
    /*
    public void PlayImpossible()
    {
        SolutionManager.difficulty = 0;
        SceneManager.LoadScene("GamePlayScene");
    }
    */

    // shows and hides description text (and hides or re-enables the rest of the main menu reversed)
    public void InfoButton() 
    {
        if (infoText.activeSelf) // info text is showing: hide
        {
            infoButtonText.text = "How To Play";
            infoText.SetActive(false);
            mainMenuText.enabled = true;
            difficulties.SetActive(true);
            leaderboardButton.SetActive(true);
        }
        else // info text not showing (normal main menu screen): show
        {
            leaderboardButton.SetActive(false);
            difficulties.SetActive(false);
            infoButtonText.text = "Close Info";
            mainMenuText.enabled = false;
            infoText.SetActive(true);
        }
    }

    // go to leaderboard scene
    public void LeaderboardButton()
    {
        SceneManager.LoadScene("Login&Leaderboard");
    }

    // quits desktop application, unused in web build
    public void Quit()
    {
        Application.Quit();
    }
}

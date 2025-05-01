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

    public void PlayEasy()
    {
        SolutionManager.difficulty = 10; // 10 pieces placed initially, player has to place 2
        SceneManager.LoadScene("GamePlayScene");
    }
    
    public void PlayMedium()
    {
        SolutionManager.difficulty = 9;
        SceneManager.LoadScene("GamePlayScene");
    }
    
    public void PlayHard()
    {
        SolutionManager.difficulty = 8;
        SceneManager.LoadScene("GamePlayScene");
    }
    
    public void PlayExtraHard()
    {
        SolutionManager.difficulty = 7;
        SceneManager.LoadScene("GamePlayScene");
    }

    public void PlayExtreme()
    {
        SolutionManager.difficulty = 5;
        SceneManager.LoadScene("GamePlayScene");
    }
    
    public void PlayImpossible()
    {
        SolutionManager.difficulty = 0;
        SceneManager.LoadScene("GamePlayScene");
    }

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

    public void LeaderboardButton()
    {
        SceneManager.LoadScene("Login&Leaderboard");
    }

    public void Quit() // only useable if desktop application TODO delete for web application
    {
        Application.Quit();
    }
}

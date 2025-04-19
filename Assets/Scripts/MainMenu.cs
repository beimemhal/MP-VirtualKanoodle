using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_Text infoButtonText;
    public TMP_Text infoText;
    public TMP_Text mainMenuText;
    public GameObject difficulties;
    public GameObject leaderboardButton;

    public Canvas mainMenu;

    public void PlayEasy()
    {
        SolutionManager.difficulty = 9; // 9 pieces placed initially, player has to place 3
        SceneManager.LoadScene("GamePlayScene");
    }
    
    public void PlayMedium()
    {
        SolutionManager.difficulty = 7;
        SceneManager.LoadScene("GamePlayScene");
    }
    
    public void PlayHard()
    {
        SolutionManager.difficulty = 6;
        SceneManager.LoadScene("GamePlayScene");
    }
    
    public void PlayExtraHard()
    {
        SolutionManager.difficulty = 4;
        SceneManager.LoadScene("GamePlayScene");
    }
    
    public void PlayImpossible()
    {
        SolutionManager.difficulty = 0;
        SceneManager.LoadScene("GamePlayScene");
    }

    public void InfoButton() 
    {
        if (infoText.enabled) // info text is showing: hide
        {
            infoButtonText.text = "Show Info";
            infoText.enabled = false;
            difficulties.SetActive(true);
            leaderboardButton.SetActive(true);
        }
        else // info text not showing: show
        {
            leaderboardButton.SetActive(false);
            difficulties.SetActive(false);
            infoButtonText.text = "Close Info";
            infoText.enabled = true;
        }
    }

    public void LeaderboardButton()
    {
        // TODO if else
        // leaderboardParent.SetActive(false);
    }

    public void Quit() // only useable if desktop application
    {
        Application.Quit();
    }
}

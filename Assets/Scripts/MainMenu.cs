using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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

    public void Quit() // only useable if desktop application
    {
        Application.Quit();
    }

    // TODO play against time?
    public void TimerToggle()
    {
        // TODO 
    }

    // info: How to play, button image explainations & credits TODO ? later

}

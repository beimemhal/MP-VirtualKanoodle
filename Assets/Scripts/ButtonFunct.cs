using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunct : MonoBehaviour
{
    SolutionManager solutionManager;
    public int buttonNr;

    public static bool updateLeaderboardEntry = false;

    private void Start()
    {
        GameManager.gameManager = GameObject.Find("GridPrefab").GetComponent<GameManager>();
        GameManager.gameManager.buttons.Add(this);

        solutionManager = GameObject.Find("SolutionManagerHolder").GetComponent<SolutionManager>();
    }

    // implements clicking a button functionality
    private void Update() // only when Button it's attached to is active
    {
        // if mouse button down call one of the functions
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // for checking wether mouse click is on the button
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // turn grid
                    if (buttonNr == 1) GameManager.TurnGridY(1);
                    else if (buttonNr == 2) GameManager.TurnGridY(-1);
                    // move pieces
                    else if (buttonNr == 3) GameManager.MovePiece('x', -1); // left
                    else if (buttonNr == 4) GameManager.MovePiece('x', 1); // right
                    else if (buttonNr == 5) GameManager.MovePiece('y', -1); // down
                    else if (buttonNr == 6) GameManager.MovePiece('y', 1); // up
                    else if (buttonNr == 7) GameManager.MovePiece('z', -1); // forward left 
                    else if (buttonNr == 8) GameManager.MovePiece('z', 1); // backwards right
                    // turn pieces
                    else if (buttonNr == 9) GameManager.TurnPiece('x', 1);
                    else if (buttonNr == 10) GameManager.TurnPiece('x', -1);
                    else if (buttonNr == 11) GameManager.TurnPiece('y', 1);
                    else if (buttonNr == 12) GameManager.TurnPiece('y', -1);
                    else if (buttonNr == 13) GameManager.TurnPiece('z', 1);
                    else if (buttonNr == 14) GameManager.TurnPiece('z', -1);
                    // other
                    else if (buttonNr == 15) GameManager.gameManager.Place();
                    else if (buttonNr == 16) GameManager.Remove();
                    else if (buttonNr == 17) GameManager.Restart();
                    else if (buttonNr == 18) BackToMainMenu();
                    else if (buttonNr == 19) solutionManager.GiveHint();

                    else 
                        Debug.Log("Error in ButtonFunct for buttonNr.: " + buttonNr);
                }
            }
        }
    }

    // assigned to button "Entry Leaderboard", initiates entry to leaderboard by setting variables accordingly and starts leaderboard scene
    public void AddEntryToLeaderboard() // passes/ stores time as mmss(ms)(ms)(ms) as an int
    {
        string difficultyText = "Easy";
        if (SolutionManager.difficulty == 9) difficultyText = "Medium";
        else if (SolutionManager.difficulty == 8) difficultyText = "Hard";
        else if (SolutionManager.difficulty == 7) difficultyText = "VeryHard";
        else if (SolutionManager.difficulty == 5) difficultyText = "Extreme";

            MainMenuImport.leaderboardID = "leaderboard" + difficultyText;

        updateLeaderboardEntry = true;

        // reset game scene
        ResetGameScene();

        SceneManager.LoadScene("Login&Leaderboard");
    }

    // resets gameplay scene and load main menu scene
    public void BackToMainMenu()
    {
        // reset game scene: empty button and pieces list (static variables meant reset when leaving the scene) 
        ResetGameScene();
        Timer.staticTime.timeValue = 0;

        SceneManager.LoadScene("MainMenuScene");
    }

    // resets gameplay scene's static variables
    void ResetGameScene()
    {
        SolutionManager.solutionRotations.Clear();
        SolutionManager.solutionPositions.Clear();
        HardCodedLvl.lastPlaced.Clear();
        GridFunct.gridTurns = 0;
        SolutionManager.hintNr = 0;
    }
}

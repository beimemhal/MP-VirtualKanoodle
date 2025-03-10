using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunct : MonoBehaviour
{
    public int buttonNr;

    private void Start()
    {
        GameManager.buttons.Add(this);
    }

    private void Update() // only be when Button it's attached to is active
    {
        // if mouse button down call one of the functions, source for raycast functionality: ChatGPT (altered)
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // for checking wether mouse click is on the button
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // turn grid
                    if (buttonNr == 1) GameManager.TurnGridY(1); // TODO refactor switch-case
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
                    else if (buttonNr == 15) GameManager.Place();
                    else if (buttonNr == 16) GameManager.Remove();
                    else if (buttonNr == 17) GameManager.Restart();
                    else if (buttonNr == 18) BackToMainMenu();

                    else 
                        Debug.Log("Error in ButtonFunct for buttonNr.: " + buttonNr);
                }
            }
        }
    }

    public void BackToMainMenu()
    {
        // empty button and pieces list
        GameManager.buttons.Clear();
        GameManager.allPieces.Clear();

        SceneManager.LoadScene("MainMenuScene");
    }
}

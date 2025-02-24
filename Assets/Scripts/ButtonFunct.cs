using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunct : MonoBehaviour
{
    public int buttonNr;

    private void Start()
    {
        GameManager.buttons.Add(gameObject);
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
                    else if (buttonNr == 3) GameManager.MovePiece('x', 1);
                    else if (buttonNr == 4) GameManager.MovePiece('x', -1);
                    else if (buttonNr == 5) GameManager.MovePiece('y', 1);
                    else if (buttonNr == 6) GameManager.MovePiece('y', -1);
                    else if (buttonNr == 7) GameManager.MovePiece('z', 1);
                    else if (buttonNr == 8) GameManager.MovePiece('z', -1);
                    // turn pieces
                    else if (buttonNr == 9) GameManager.TurnPiece('x', 1);
                    else if (buttonNr == 10) GameManager.TurnPiece('x', -1);
                    else if (buttonNr == 11) GameManager.TurnPiece('y', 1);
                    else if (buttonNr == 12) GameManager.TurnPiece('y', -1);
                    // other
                    else if (buttonNr == 13) Place();
                    else if (buttonNr == 14) Remove();
                    else if (buttonNr == 15) Restart();
                    else if (buttonNr == 16) BackToMainMenu();

                    else Debug.Log("Error in ButtonFunct for buttonNr.: " + buttonNr);
                }
            }
        }
    }

    public void Place() // 1
    {
        // check if position valid/ placeable -> all spheres on active ghost spheres TODO 

        // attach piece to grid TODO

        // disable each ghost sphere that overlaps with a pieces sphere TODO

    }

    public void Remove()  // 1
    { 
        // TODO
    }

    public void Restart() // in GameManager?
    { 
        // put pieces at initial positions (after detaching them from the grid) TODO
        // reset grid spheres
    }

    public void BackToMainMenu() // in GameManager?
    { 
        // TODO
    }
}

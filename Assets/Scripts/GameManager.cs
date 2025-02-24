using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

// component of EventSystem
public class GameManager : MonoBehaviour
{
    bool won = false; // TODO check function

    public static Piece selectedPiece;

    public static List<GameObject> allPieces = new();
    public static List<GameObject> buttons = new();
    public static GameObject gridParent;
    
    // turn grids ghost spheres & its attached pieces -> pieces still on disabled ghost spheres
    public static void TurnGridY(int i) // i = 1 if turned right, i = -1 if turned left
    {
        gridParent.transform.Rotate(0, 120 * i, 0);
    }

    public static void MovePiece(char dir, int posNeg) // dir = 'x' or 'y' or 'z'
    {
        // check if outside grid
        if (OutsideGrid('x')) { 
            // TODO 
            return; 
        }
        
        // calculate new position in grid
        if (dir == 'x') // x + () * i
            selectedPiece.gridPos.x += posNeg;
        else if (dir == 'y')
            selectedPiece.gridPos.y += posNeg;
        else // z
            selectedPiece.gridPos.z += posNeg;

        selectedPiece.gameObject.transform.position = GridFunct.CalcGridToGlobalSpace(selectedPiece.gridPos); // no move piece when only child is moved -> transform parent
    }

    public static void TurnPiece(char dir, int negPos)
    {
        if (dir ==  'y') selectedPiece.gameObject.transform.Rotate(0, 120 * negPos, 0);
        else // x
            selectedPiece.gameObject.transform.Rotate(120 * negPos, 0, 0);
        // TODO turn in z ?
    }

    void Update() // TODO
    {
        // if selected != null
        // check for movement
    }

    // restart funct here ? TODO

    public void DisableButtons()
    {
        if (selectedPiece == null)
        {
            foreach (GameObject g in buttons) g.SetActive(false);
        }
        // TODO
    }
    
    // at least one of the pieces spheres has to remain on a grid position
    static bool OutsideGrid(char dir) // dir = 'x' or 'y' or 'z'
    {
        // if ( == 1) disable Minus Buttons TODO
        return false; 
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

// component of EventSystem
public class GameManager : MonoBehaviour
{
    public static List<GameObject> allPieces = new();
    public static List<ButtonFunct> buttons = new();
    public static GameObject gridParent;

    public static Piece selectedPiece;

    bool won = false; // TODO check function
    Dictionary<string, Vector3> solutionPositions = new();
    Dictionary<string, Quaternion> solutionRotations = new();

    private void Start()
    {
        // needed for Rotate which needs gridParent to be static 
        gridParent = gameObject; 
    }

    
    void Update() // TODO
    {
        /*
        // moving with wasd buttons ? extra TODO
        if (selectedPiece != null)
        {
            if (Input.GetButtonDown("Up")) GameManager.MovePiece('y', 1); // up
            else if (Input.GetButtonDown("Left")) GameManager.MovePiece('x', -1); // left
            else if (Input.GetButtonDown("Down")) GameManager.MovePiece('y', -1); // down
            else if (Input.GetButtonDown("Right")) GameManager.MovePiece('x', 1); // right
        }
        */

        // check selectedPiece position and dis/enable buttons accordingly TODO: no after movement button

    }

    // turn grids ghost spheres & its attached pieces -> pieces still on disabled ghost spheres
    public static void TurnGridY(int negPos) // i = 1 if turned right, i = -1 if turned left
    {
        // gridParent.transform.Rotate(Vector3.up * 120F * negPos);

        Vector3 turnPoint = new Vector3(-5.5F, 2.541241F, 2.165064F); // = middle of the grid
        
        Vector3 direction = gridParent.transform.position - turnPoint; // vector from turnPoint to object

        // rotate this vector around the axis
        Quaternion rotation = Quaternion.AngleAxis(120F * negPos, Vector3.up);
        Vector3 newDirection = rotation * direction;

        // Apply new position and rotation
        gridParent.transform.position = turnPoint + newDirection;
        gridParent.transform.Rotate(Vector3.up, 120F * negPos, Space.World);
    }

    public static void MovePiece(char dir, int posNeg) // dir = 'x' or 'y' or 'z'
    {
        Vector3Int newPos = new Vector3Int();
        // calculate new position in grid
        if (dir == 'x')
            newPos.x = selectedPiece.gridPos.x + posNeg;
        else if (dir == 'y')
            newPos.y = selectedPiece.gridPos.y + posNeg;
        else // z
            newPos.z = selectedPiece.gridPos.z + posNeg;

        // check if outside grid
        if (OutsideGrid(newPos)) {
            Debug.Log("Piece can't be moved in " + (posNeg > 0 ? "positive " : "negative ") + dir + "-direction because it's outside the grid.");
            return; 
        }

        // move is possible
        selectedPiece.gridPos = newPos;
        // piece doesn't move when only child is moved -> transform parent
        selectedPiece.gameObject.transform.position = GridFunct.CalcGridToGlobalSpace(selectedPiece.gridPos);
    }

    public static void TurnPiece(char dir, int negPos)
    {
        if (dir == 'y') selectedPiece.gameObject.transform.Rotate(eulers: 60F * negPos * Vector3.up); // rotation *= Quaternion.AngleAxis(60F * negPos, Vector3.up); // 0, 60F * negPos, 0);
        else if (dir == 'x')
            selectedPiece.gameObject.transform.Rotate(eulers: 60F * negPos * Vector3.right); // rotation *= Quaternion.AngleAxis(60F * negPos, Vector3.right); // 60F * negPos, 0, 0);
        else // z
            selectedPiece.gameObject.transform.Rotate(eulers: 60F * negPos * Vector3.forward); // rotation *= Quaternion.AngleAxis(60F * negPos, Vector3.forward); // 0, 0, 60F * negPos);
    }

    // restart funct here ? TODO = initial game state + initial level build

    public static void DisOrEnableButtons(int[] toChange, bool disOrEnable) // button numbers of buttons that should be dis- or enabled
    {
        foreach (ButtonFunct button in buttons) 
        {
            for (int i = 0; i < toChange.Length; i++)
            {
                if (button.buttonNr == toChange[i])
                {
                    button.gameObject.SetActive(disOrEnable);
                }
            }
        }
    }

    public static void DisOrEnableMovement(bool disOrEnable)
    {
        int[] changeButtons = new int[13];
        for (int i = 3; i < 16; i++)
            changeButtons[i - 3] = i;

        DisOrEnableButtons(changeButtons, disOrEnable);
    }

    // piece sphere 1 has to remain on a grid position
    static bool OutsideGrid(Vector3Int newPos)
    {
        if (newPos.x < 0 || newPos.x > 5 - newPos.y - newPos.z ||
            newPos.y < 0 || newPos.y > 5 - newPos.x - newPos.z ||
            newPos.z < 0 || newPos.z > 5 - newPos.y - newPos.x)
            return true;
        return false; 
    }

    // disable buttons according to piece's position -> check after each movement
    void DynamicButtonCheck() // TODO later: also check place & remove button
    {
        int[] toDisable = new int[6]; // on top: x and z buttons in both dir.s disabled
        int[] toEnable = new int[6];
        int i = 0;
        int j = 0;

        if (selectedPiece.gridPos.x == 0) // no negative x
            toDisable[i++] = 3; // TODO does i++ work? debug point
        else 
            toEnable[j++] = 3;
        if (selectedPiece.gridPos.x == 5 - selectedPiece.gridPos.y - selectedPiece.gridPos.z) // no positive x
            toDisable[i++] = 4;
        else
            toEnable[j++] = 4;

        if (selectedPiece.gridPos.y == 0) // no negative y
            toDisable[i++] = 5;
        else
            toEnable[j++] = 5;
        if (selectedPiece.gridPos.y == 5 - selectedPiece.gridPos.x - selectedPiece.gridPos.z) // no positive y
            toDisable[i++] = 6;
        else
            toEnable[j++] = 6;

        if (selectedPiece.gridPos.z == 0) // no negative z
            toDisable[i++] = 7;
        else
            toEnable[j++] = 7;
        if (selectedPiece.gridPos.z == 5 - selectedPiece.gridPos.y - selectedPiece.gridPos.x) // no positive z
            toDisable[i++] = 8;
        else
            toEnable[j++] = 8;

        DisOrEnableButtons(toDisable, false);
        DisOrEnableButtons(toEnable, true);
    }

    public static void Place() // 1 TODO
    {
        // 1 check if position valid/ placeable -> all spheres on active ghost spheres
        if (CheckPlacingPossible())
        {
            // TODO show pop up message "Piece can't be placed there (bc out of grid/ overlaps with other piece)"
            return;
        }

        // 2 attach piece to grid
        selectedPiece.transform.SetParent(gridParent.transform);

        // 3 disable ghost spheres that overlap with a pieces sphere TODO
        DisableGhostSpheres();

        // 4 placed variable in piece = true
        selectedPiece.placed = true;
    }

    public static void Remove()  // 1 TODO
    {
        // 1 detach piece from grid TODO

        // 2 ensable each ghost sphere that overlaps with a pieces sphere TODO

        // 3 placed variable in piece = false

    }

    public static void Restart() // in GameManager? TODO
    {
        // 1 put pieces at initial positions (after detaching them from the grid) TODO

        // 2 reset grid spheres TODO

        // 3 rebuild initial pieces TODO

        // 4 disable removeButton TODO

    }

    // return array/ list of ghost spheres that have to be disabled and save in placed pieces (piece variable: sphereNr = length of array) if array[sphereNr-1] == null {placement not possible}
    public static GameObject[] CheckPlacingPossible() // TODO 1
    {

        return new GameObject[5];
    }

    static void DisableGhostSpheres() // TODO
    {

    }
}

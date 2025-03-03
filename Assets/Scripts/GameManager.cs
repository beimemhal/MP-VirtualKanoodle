using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

// component of EventSystem
public class GameManager : MonoBehaviour // TODO move main fcts to top and help fcts down in same order as called main fct
{
    public static List<GameObject> allPieces = new();
    public static List<ButtonFunct> buttons = new();
    public static GameObject gridParent;

    public static Piece selectedPiece;

    Dictionary<string, Vector3> solutionPositions = new();
    Dictionary<string, Quaternion> solutionRotations = new();

    public static GameObject WinMessageCanvas;

    private void Start()
    {
        // needed for Rotate which needs gridParent to be static 
        gridParent = gameObject;
        WinMessageCanvas = GameObject.Find("WinMessageCanvas");

        // disable all buttons
        int[] toDisable = new int[16];

        for (int i = 0; i < toDisable.Length; i++)
            toDisable[i] = i + 3;

        DisOrEnableButtons(toDisable, false);
    }

    // turn grids ghost spheres & its attached pieces -> pieces still on disabled ghost spheres
    public static void TurnGridY(int negPos) // i = 1 if turned right, i = -1 if turned left
    {
        Vector3 turnPoint = new Vector3(-5.5F, 2.541241F, 1.443376F); // = middle of the grid
        
        Vector3 direction = gridParent.transform.position - turnPoint; // vector from turnPoint to object

        // rotate this vector around the axis
        Quaternion rotation = Quaternion.AngleAxis(120F * negPos, Vector3.up);
        Vector3 newDirection = rotation * direction;

        // Apply new position and rotation
        gridParent.transform.position = turnPoint + newDirection;
        gridParent.transform.Rotate(Vector3.up, 120F * negPos, Space.World);

        Debug.Log("Grid turned");
    }

    public static void MovePiece(char dir, int posNeg) // dir = 'x' or 'y' or 'z'
    {
        Vector3Int newPos = selectedPiece.gridPos;
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
        selectedPiece.gameObject.transform.position = GridFunct.CalcGridToGlobalSpace(selectedPiece.gridPos);

        // check for button activation and deactivation 
        DynamicButtonCheck();
    }

    public static void TurnPiece(char dir, int negPos) // TODO rework !!!
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
                    button.gameObject.SetActive(disOrEnable); // TODO not setActive, dis- or enable collidor
                    // TODO if disabled make button colour darker
                }
            }
        }
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
    public static void DynamicButtonCheck() // TODO
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

    public static void Place()
    {
        Debug.Log("Place button pressed");
        
        // 1 check if position valid/ placeable -> all piece spheres on active grid spheres
        GameObject[] gridSpheres = new GameObject[selectedPiece.sphereNr];
        gridSpheres = GetOverlappingSpheres();
        if (gridSpheres[selectedPiece.sphereNr - 1] == null) // not placeable
        {
            // TODO show pop up message "Piece can't be placed there (bc out of grid/ overlaps with other piece)"
            return;
        }
        else // save in placed piece
        {
            selectedPiece.overlapsGridSpheres = gridSpheres;
        }

        // 2 attach piece to grid
        selectedPiece.transform.SetParent(gridParent.transform);

        // 3 disable ghost spheres that overlap with a pieces sphere
        DisOrEnableGridSpheres(gridSpheres, false);

        // 4 placed variable in piece = true
        selectedPiece.placed = true;

        // 5 selectedPiece = null
        selectedPiece = null;

        // 6 check if won
        if (CheckWon())
            // activate winning screen/ pop up window 
            WinMessageCanvas.SetActive(true); // TODO disable/ block everything else
    }

    public static void Remove()
    {
        Debug.Log("Remove button pressed");

        // 1 enable ghost spheres that overlap with a piece's sphere
        DisOrEnableGridSpheres(selectedPiece.overlapsGridSpheres, true);

        // 2 placed variable in piece = false
        selectedPiece.placed = false;

        // 3 deselect (inital position)
        PieceUnselected();
    }

    public static void Restart() // TODO
    {
        Debug.Log("Restart button pressed");

        // 1 reset selectedPiece
        PieceUnselected();

        // 1 put pieces in grid at initial positions if moveable TODO
        // 1.1 after detaching them from the grid 
        // 1.2 reset grid spheres piece was on

        // reset piece's overlapsGridSpheres var to empty

    }

    // returns array of grid spheres that have to be disabled
    public static GameObject[] GetOverlappingSpheres()
    {
        GameObject[] overlappingSpheres = new GameObject[selectedPiece.sphereNr];
        int j = 0;

        // iterate through all spheres of the piece
        for (int i = 0; i < selectedPiece.sphereNr; i++)
        {
            // check if collider detects collisions (doesn't if gridSphere disabled)
            Collider pieceSphere = selectedPiece.gameObject.transform.GetChild(i).gameObject.GetComponent<SphereCollider>();

            bool isOverlapping = false;

            for (int k = 0; k < 56; k++) // iterate through all gridSpheres
            {
                Collider gridSphere = gridParent.gameObject.transform.GetChild(k).gameObject.GetComponent<SphereCollider>();
                isOverlapping = pieceSphere.bounds.Intersects(gridSphere.bounds); // TODO source ChatGPT
                if (isOverlapping)
                {
                    overlappingSpheres[j++] = gridSphere.gameObject;
                    break; // go to next pieceSphere if overlapping gridSphere found
                }
            }
        }

        return overlappingSpheres;
    }

    static void DisOrEnableGridSpheres(GameObject[] gridSpheres, bool disOrEnable)
    {
        foreach (GameObject sphere in gridSpheres)
            sphere.SetActive(disOrEnable);
    }

    static bool CheckWon()
    {
        SphereCollider[] gridSpheres = gridParent.transform.GetComponentsInChildren<SphereCollider>(); // only direct children, not SphereColliders in prefabs

        // if all grid spheres are disabled = solution found
        foreach (SphereCollider sphere in gridSpheres)
            if (sphere.gameObject.activeSelf)
                return false;

        return true;
    }

    // when piece is no longer selected but also not placed on grid => put back at initial position
    public static void PieceUnselected()
    {
        // 1 put selected to initial position & reset orientation
        selectedPiece.gameObject.transform.SetPositionAndRotation(selectedPiece.initialPosition, Quaternion.identity);

        // 2 detach from grid (no child of grid anymore)
        selectedPiece.gameObject.transform.SetParent(null);

        // 3 selected = null
        selectedPiece = null;

        // 4 disable buttons
        int[] toDisable = new int[14];

        for (int i = 0; i < toDisable.Length; i++)
            toDisable[i] = i + 3;

        DisOrEnableButtons(toDisable, false);
    }
}

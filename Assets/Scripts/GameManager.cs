using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

// component of GridPrefab
public class GameManager : MonoBehaviour
{
    public static List<GameObject> allPieces = new();
    public static List<ButtonFunct> buttons = new();

    public static GameObject gridParent;

    public static Piece selectedPiece = null;

    public static bool userNotAlgo = true; // TODO change back for solver, reset when back to main menu?
    public static bool won = false;

    public GameObject winMessageCanvas;
    public GameObject popUpCanvas;

    private void Start()
    {
        // needed for Rotate which needs gridParent to be static 
        gridParent = gameObject;

        // disable all (movement) buttons
        DisableAllButtons();

        // TODO if difficulty = 0 : disable hint button
        if (SolutionManager.difficulty == 0)
        {
            int[] hintButton = new int[1];
            hintButton[0] = 19;
            DisOrEnableButtons(hintButton, false);
        }
    }

    // turn grids ghost spheres & its attached pieces -> pieces still on disabled ghost spheres
    public static void TurnGridY(int negPos) // i = 1 if turned right, i = -1 if turned left
    {
        Vector3 turnPoint = new(-5.5F, 2.541241F, 1.443376F); // = middle of the grid
        Vector3 direction = gridParent.transform.position - turnPoint; // vector from turnPoint to object

        // rotate this vector around the y axis
        Quaternion rotation = Quaternion.AngleAxis(120F * negPos, Vector3.up);
        Vector3 newDirection = rotation * direction;

        // set grids new position and rotate
        gridParent.transform.position = turnPoint + newDirection; // grid centre + rotated vector from turnPoint to gridPrefab
        gridParent.transform.Rotate(Vector3.up, 120F * negPos, Space.World);

        // Debug.Log("gridparent " + gridParent.name);

        // change grid pos of child pieces
        foreach (var piece in allPieces) // iterate through all pieces
        {
            // Debug.Log("piece " + piece.name);
            if (piece.transform.parent != null)
            {
                // Debug.Log("pieces parent " + piece.transform.parent.name);

                if (piece.transform.parent.name == gridParent.name) // if piece is a child of grid
                {
                    // Debug.Log("is child");

                    piece.GetComponent<Piece>().CalcNewGridCoords(negPos);
                }
            }
        }

        // check if movement buttons changed
        if (userNotAlgo && selectedPiece != null) DynamicButtonCheck();

        Debug.Log("Grid turned");
    }

    public static void MovePiece(char dir, int posNeg) // dir = 'x' or 'y' or 'z'
    {
        Vector3Int newPos = selectedPiece.gridPos;
        // 1 calculate new position in grid
        if (dir == 'x')
            newPos.x = selectedPiece.gridPos.x + posNeg;
        else if (dir == 'y')
            newPos.y = selectedPiece.gridPos.y + posNeg;
        else // z
            newPos.z = selectedPiece.gridPos.z + posNeg;

        // 2 check if outside grid
        if (GridFunct.OutsideGrid(newPos)) { // shouldn't be possible because buttons disabled accordingly -> happens in solvingAlgo
            Debug.Log("Piece can't be moved in " + (posNeg > 0 ? "positive " : "negative ") + dir + "-direction because it's outside the grid.");
            return; 
        }

        // 3 move is possible
        selectedPiece.gridPos = newPos;
        selectedPiece.gameObject.transform.position = GridFunct.CalcGridToGlobalSpace(selectedPiece.gridPos);

        // 4 check for button activation and deactivation 
        if (userNotAlgo)
            DynamicButtonCheck();
    }

    public static void TurnPiece(char dir, int negPos)
    {
        if (dir == 'y')
        {
            selectedPiece.gameObject.transform.Rotate(eulers: 60F * negPos * Vector3.up); // rotation *= Quaternion.AngleAxis(60F * negPos, Vector3.up); // 0, 60F * negPos, 0);
            
            selectedPiece.rotationNrs.y = (selectedPiece.rotationNrs.y + negPos + 6) % 6;
        }
        else if (dir == 'x')
        {
            if  (CalcRotationAmount(selectedPiece.rotationNrs.x, selectedPiece.rotationNrs.z, selectedPiece.rotationNrs.y, negPos))
                selectedPiece.gameObject.transform.Rotate(eulers: 70.52877936F * negPos * Vector3.right); // rotation *= Quaternion.AngleAxis(60F * negPos, Vector3.right); // 60F * negPos, 0, 0);
            else
                selectedPiece.gameObject.transform.Rotate(eulers: 2 * 54.73561032F * negPos * Vector3.right);

            selectedPiece.rotationNrs.x = (selectedPiece.rotationNrs.x + negPos + 4) % 4;
        }
        else // z
        {
            if  (CalcRotationAmount(selectedPiece.rotationNrs.z - 1, selectedPiece.rotationNrs.x, selectedPiece.rotationNrs.y, negPos))
                selectedPiece.gameObject.transform.rotation *= Quaternion.AngleAxis(70.52877937F * negPos, new Vector3(0.5F, 0, 0.8660254F)); // Vector3.forward); // Rotate(eulers: 60F * negPos * new Vector3(0.5F, 0, 0.8660254F)); // 0, 0, 60F * negPos);
            else
                selectedPiece.gameObject.transform.rotation *= Quaternion.AngleAxis(2 * 54.73561032F * negPos, new Vector3(0.5F, 0, 0.8660254F));

            selectedPiece.rotationNrs.z = (selectedPiece.rotationNrs.z + negPos + 4) % 4;
        }
    }

    public bool Place()
    {
        // 1 check if position valid/ placeable -> all piece spheres on active grid spheres
        if (!GetOverlappingSpheres()) // not placeable
        {
            // if player (not solution algo) show explaining popUp message
            if (userNotAlgo)
            {
                StartCoroutine(popUpCanvas.GetComponent<PopUpManager>().ShowNotification("Piece can't be placed here (because it's out of grid or overlaps with another piece)!"));
            }
            // Debug.Log("Placing not possible");
            return false;
        }

        Debug.Log("Placing piece " + selectedPiece.name + " successful.");

        // 2 attach piece to grid TODO unnecessary bc already done in selected
        // selectedPiece.transform.SetParent(gridParent.transform);

        // 3 disable ghost spheres that overlap with a pieces sphere
        DisOrEnableGridSpheres(selectedPiece.overlapsGridSpheres, false);

        // 4 placed variable in piece = true
        selectedPiece.placed = true;

        // 5 remove outline script
        if (userNotAlgo)
            Destroy(selectedPiece.gameObject.GetComponent<Outline>());

        // 6 selectedPiece = null
        selectedPiece = null;

        // 7 disable buttons
        if (userNotAlgo)
            DisableAllButtons();

        // 8 check if won (only place because its more efficient than checking in update)
        if (userNotAlgo && GridFunct.CheckWon())
        {
            // deactivate all pieces colliders & disable all buttons so that nothing can be changed after win
            DisableAllButtons(); 
            int[] otherButtons = new int[5]; // restart, backtomainmenu, hint & turnGrid
            otherButtons[0] = 17;
            otherButtons[1] = 18;
            otherButtons[2] = 19;
            otherButtons[3] = 1;
            otherButtons[4] = 2;
            DisOrEnableButtons(otherButtons, false);
            foreach (GameObject p in allPieces)
            {
                p.SetActive(false);
            }

            won = true;

            // activate winning screen/ pop up window 
            gridParent.GetComponent<GameManager>().winMessageCanvas.SetActive(true);
        }

        return true;
    }

    public static void Remove()
    {
        // Debug.Log("Remove button pressed");

        // 1 enable ghost spheres that overlap with a piece's sphere
        DisOrEnableGridSpheres(selectedPiece.overlapsGridSpheres, true);

        // 2 reset piece's overlapsGridSpheres var to empty
        selectedPiece.overlapsGridSpheres = new GameObject[selectedPiece.sphereNr];

        // 3 placed variable in piece = false
        selectedPiece.placed = false;

        // 4 deselect (inital position) TODO userTest: should stay on the grid as a selected but not placed piece?
        PieceUnselected();
    }

    // = initial game state + initial level build/ removes all user placed pieces 
    public static void Restart() // TODO later/ refactoring maybe rework
    {
        Debug.Log("Restart button pressed");

        // 1 reset selectedPiece
        PieceUnselected();

        // 1 remove all moveable pieces in grid TODO userTest not the hint pieces or schon? (remove all lastPlaced except the first difficulty ones)
        foreach (GameObject p in allPieces)
        {
            Piece piece = p.GetComponent<Piece>();
            if (piece.moveable && piece.placed)
            {
                GameManager.selectedPiece = piece;
                GameManager.Remove();
            }
        }
    }

    static bool CalcRotationAmount(int toTurn, int other, int y, int negPos) // toTurn x if dir=x, z if dir=z; independent of grid rotation
    {
        return (
                  (toTurn % 2 != 0 && negPos == 1 || toTurn % 2 == 0 && negPos == -1) && y % 2 == 0
                  ||
                  !(toTurn % 2 != 0 && negPos == 1 || toTurn % 2 == 0 && negPos == -1) && y % 2 != 0
                 )
                 &&
                 other % 2 == 0

                 ||

                !(
                  (toTurn % 2 != 0 && negPos == 1 || toTurn % 2 == 0 && negPos == -1) && y % 2 == 0
                  ||
                  !(toTurn % 2 != 0 && negPos == 1 || toTurn % 2 == 0 && negPos == -1) && y % 2 != 0
                 )
                 &&
                 other % 2 != 0;
    }

    public static void DisOrEnableButtons(int[] toChange, bool disOrEnable) // button numbers of buttons that should be dis- or enabled
    {
        for (int i = 0; i < toChange.Length; i++)
        {
            // Debug.Log("Buttons " + toChange[i] + " disabled.");

            foreach (ButtonFunct button in buttons) 
            {                
                if (button.buttonNr == toChange[i])
                {
                    button.gameObject.SetActive(disOrEnable);
                }
            }
        }
    }

    static void DisableAllButtons() // except restart, backtomainmenu and hint 
    {
        int[] toDisable = new int[14];

        for (int i = 0; i < toDisable.Length; i++)
            toDisable[i] = i + 3;

        DisOrEnableButtons(toDisable, false);
    }

    // disable buttons according to piece's position -> check after each movement
    public static void DynamicButtonCheck()
    {
        if (selectedPiece.placed)
        {
            DisableAllButtons();
            int[] toEnable = new int[1];
            toEnable[0] = 16;
            DisOrEnableButtons(toEnable, true);
        }
        else
        {

            int[] toDisable = new int[6]; // on top: x and z buttons in both dir.s disabled
            int[] toEnable = new int[6];
            int i = 0;
            int j = 0;

            if (selectedPiece.gridPos.x == 0) // no negative x
                toDisable[i++] = 3;
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
    }

    // returns array of grid spheres that have to be disabled
    public static bool GetOverlappingSpheres() // returns false if not placeable
    {
        selectedPiece.overlapsGridSpheres = new GameObject[selectedPiece.sphereNr];

        // 1 iterate through all spheres of the piece
        for (int i = 0; i < selectedPiece.sphereNr; i++)
        {
            // 2 check if collider detects collisions
            Collider pieceSphere = selectedPiece.gameObject.transform.GetChild(i).gameObject.GetComponent<SphereCollider>();
    
            // 3 iterate through all gridSpheres
            foreach (SphereCollider gridSphere in GridFunct.gridPoints)
            {
                // 4 check if gridSphere is active, if not: continue with next gridSphere
                if (!gridSphere.gameObject.activeSelf) continue;

                // isOverlapping = pieceSphere.bounds.Intersects(gridSphere.bounds); // does not work in solver: sphere still at initial position TODO delete
                if (CheckIntersects(pieceSphere, gridSphere))
                {
                    selectedPiece.overlapsGridSpheres[i] = gridSphere.gameObject;
                    break; // go to next pieceSphere if overlapping gridSphere found
                }
            }
            
            // 5 if no overlapping grid sphere found for one of the piece spheres: piece not placeable 
            if (selectedPiece.overlapsGridSpheres[i] == null)
                return false;
        }

        if (selectedPiece.overlapsGridSpheres[selectedPiece.sphereNr - 1] == null)
            return false;
        return true;
    }

    static void DisOrEnableGridSpheres(GameObject[] gridSpheres, bool disOrEnable)
    {
        foreach (GameObject sphere in gridSpheres)
            sphere.SetActive(disOrEnable);
    }

    // self written intersects method (of bounds)
    public static bool CheckIntersects(Collider sphere1, Collider sphere2)
    {
        if (sphere1.transform.position.x + sphere1.bounds.extents.x > sphere2.transform.position.x - sphere2.bounds.extents.x
            &&
            sphere1.transform.position.x - sphere1.bounds.extents.x < sphere2.transform.position.x + sphere2.bounds.extents.x
            &&
            sphere1.transform.position.z + sphere1.bounds.extents.z > sphere2.transform.position.z - sphere2.bounds.extents.z
            &&
            sphere1.transform.position.z - sphere1.bounds.extents.z < sphere2.transform.position.z + sphere2.bounds.extents.z
            &&
            sphere1.transform.position.y + sphere1.bounds.extents.y > sphere2.transform.position.y - sphere2.bounds.extents.y
            &&
            sphere1.transform.position.y - sphere1.bounds.extents.y < sphere2.transform.position.y + sphere2.bounds.extents.y
            )
            return true;
        return false;
    }

    // when piece is no longer selected but also not placed on grid => put back at initial position
    public static void PieceUnselected()
    {
        if (selectedPiece != null)
        {
            // 1 put selected to initial position & reset orientation
            if (!selectedPiece.placed)
            {
                selectedPiece.gameObject.transform.SetPositionAndRotation(selectedPiece.initialPosition, Quaternion.identity);

                // 2 detach from grid (no child of grid anymore)
                selectedPiece.gameObject.transform.SetParent(null);

                // 3 reset rotationNrs
                selectedPiece.rotationNrs = new Vector3Int(0, 0, 0);
            }

            // 4 remove outline script
            Destroy(selectedPiece.gameObject.GetComponent<Outline>());

            // 5 selected = null
            selectedPiece = null;

            // 6 disable buttons (movement, rotation and place/ remove)
            int[] toDisable = new int[14];

            for (int i = 0; i < toDisable.Length; i++)
                toDisable[i] = i + 3;

            DisOrEnableButtons(toDisable, false);
        }
    }
}

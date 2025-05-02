using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using TMPro;

// component of GridPrefab, contains all functions that use the gameManager.selectedPiece
public class GameManager : MonoBehaviour
{
    [SerializeField] public static GameManager gameManager; // this variable to make it available in other scripts
    public static GameObject gridParent;

    public List<GameObject> allPieces = new();
    public List<ButtonFunct> buttons = new();
    public List<SphereCollider> gridPoints = new();
    public Piece selectedPiece = null;

    public bool userNotAlgo = false;
    public bool won = false; 

    public GameObject winMessageCanvas;
    public TMP_Text winMessageText;
    public GameObject leaderboardMessage; // to dis- and enable leaderboard add button

    public GameObject popUpCanvas;

    private void Start()
    {
        // needed for Rotate which needs gridParent to be static 
        gridParent = gameObject;
        gameManager.gridPoints = gameObject.transform.GetComponentsInChildren<SphereCollider>().ToList();

        // disable all (movement) buttons
        DisableAllButtons();

        // if difficulty = 0 : disable hint button -> used for testing
        /*
        if (SolutionManager.difficulty == 0)
        {
            int[] hintButton = new int[1];
            hintButton[0] = 19;
            DisOrEnableButtons(hintButton, false);
        }
        */
    }

    // turn grids ghost spheres & its child related pieces -> pieces still on disabled ghost spheres
    public static void TurnGridY(int negPos) // i = 1 if turned right, i = -1 if turned left
    {
        Vector3 turnPoint = new(-5.5F, 2.541241F, 1.443376F); // = middle of the grid
        Vector3 direction = gridParent.transform.position - turnPoint; // vector from turnPoint to object

        // 1 rotate this vector around the y axis
        Quaternion rotation = Quaternion.AngleAxis(120F * negPos, Vector3.up);
        Vector3 newDirection = rotation * direction;

        // 2 set grids new position and rotate
        gridParent.transform.position = turnPoint + newDirection; // grid centre + rotated vector from turnPoint to gridPrefab
        gridParent.transform.Rotate(Vector3.up, 120F * negPos, Space.World);

        // Debug.Log("gridparent " + gridParent.name);

        // 3 change grid pos of child pieces (if not fixed)
        foreach (var piece in gameManager.allPieces) // iterate through all pieces
        {
            // Debug.Log("piece " + piece.name);

            if (piece.GetComponent<Piece>().moveable && piece.transform.parent != null)
            {
                piece.GetComponent<Piece>().CalcNewGridCoords(negPos);
            }
        }

        // 4 count up gridTurns
        GridFunct.gridTurns = (GridFunct.gridTurns + negPos + 3) % 3;

        // 5 check if movement buttons changed
        if (gameManager.userNotAlgo && gameManager.selectedPiece != null) DynamicButtonCheck();

        // Debug.Log("Grid turned");
    }

    // moves piece in the axis and direction as specified (checks if buttons pressable)
    public static void MovePiece(char dir, int posNeg) // dir = 'x' or 'y' or 'z'; posNeg = '1' or '-1'
    {
        Vector3Int newPos = gameManager.selectedPiece.gridPos;
        // 1 calculate new position in grid
        if (dir == 'x')
            newPos.x = gameManager.selectedPiece.gridPos.x + posNeg;
        else if (dir == 'y')
            newPos.y = gameManager.selectedPiece.gridPos.y + posNeg;
        else // z
            newPos.z = gameManager.selectedPiece.gridPos.z + posNeg;

        // 2 check if outside grid -> shouldn't be possible because buttons disabled accordingly (can happens in solvingAlgo)
        // if (GridFunct.OutsideGrid(newPos)) {
            // Debug.Log("Piece can't be moved in " + (posNeg > 0 ? "positive " : "negative ") + dir + "-direction because it's outside the grid.");
            // return; 
        // }

        // 3 move is possible
        gameManager.selectedPiece.gridPos = newPos;
        gameManager.selectedPiece.gameObject.transform.position = GridFunct.CalcGridToGlobalSpace(gameManager.selectedPiece.gridPos);
        
        // Debug.Log("New grid coords: " + newPos.x + ", " + newPos.y + ", " + newPos.z);

        // 4 check for button activation and deactivation 
        if (gameManager.userNotAlgo)
            DynamicButtonCheck();
    }

    // turns piece around the axis in direction as specified
    public static void TurnPiece(char dir, int negPos) // dir = 'x' or 'y' or 'z'; posNeg = '1' or '-1'
    {
        if (dir == 'y')
        {
            gameManager.selectedPiece.gameObject.transform.Rotate(eulers: 60F * negPos * Vector3.up); 
            
            gameManager.selectedPiece.rotationNrs.y = (gameManager.selectedPiece.rotationNrs.y + negPos + 6) % 6;
        }
        else if (dir == 'x')
        {
            if  (CalcRotationAmount(gameManager.selectedPiece.rotationNrs.x, gameManager.selectedPiece.rotationNrs.z, gameManager.selectedPiece.rotationNrs.y, negPos))
                gameManager.selectedPiece.gameObject.transform.Rotate(eulers: 70.52877936F * negPos * Vector3.right);
            else
                gameManager.selectedPiece.gameObject.transform.Rotate(eulers: 2 * 54.73561032F * negPos * Vector3.right);

            gameManager.selectedPiece.rotationNrs.x = (gameManager.selectedPiece.rotationNrs.x + negPos + 4) % 4;
        }
        else // z
        {
            if  (CalcRotationAmount(gameManager.selectedPiece.rotationNrs.z - 1, gameManager.selectedPiece.rotationNrs.x, gameManager.selectedPiece.rotationNrs.y, negPos))
                gameManager.selectedPiece.gameObject.transform.rotation *= Quaternion.AngleAxis(70.52877937F * negPos, new Vector3(0.5F, 0, 0.8660254F));
            else
                gameManager.selectedPiece.gameObject.transform.rotation *= Quaternion.AngleAxis(2 * 54.73561032F * negPos, new Vector3(0.5F, 0, 0.8660254F));

            gameManager.selectedPiece.rotationNrs.z = (gameManager.selectedPiece.rotationNrs.z + negPos + 4) % 4;
        }
    }

    // puts the piece in the grid by disabling the grid spheres and changing Piece.placed to true
    public bool Place() // the piece
    {
        // 1 check if position valid/ placeable -> all piece spheres on active grid spheres
        if (!OverlappingSpheres) // not placeable
        {
            // if player (not solution algo) show explaining popUp message
            if (gameManager.userNotAlgo)
            {
                StartCoroutine(popUpCanvas.GetComponent<PopUpManager>().ShowNotification("Piece can't be placed here (because it's out of grid or overlaps with another piece)!"));
            }
            // Debug.Log("Placing not possible");
            return false;
        }

        // Debug.Log("Placing piece " + gameManager.selectedPiece.name + " successful.");

        // 2 attach piece to grid -> unnecessary bc done in select
        // gameManager.selectedPiece.transform.SetParent(gridParent.transform);

        // 3 disable ghost spheres that overlap with a pieces sphere
        DisOrEnableGridSpheres(gameManager.selectedPiece.overlapsGridSpheres, false);

        // 4 placed variable in piece = true
        gameManager.selectedPiece.placed = true;

        // 5 remove outline script
        if (gameManager.userNotAlgo)
            Destroy(gameManager.selectedPiece.gameObject.GetComponent<Outline>());

        // 6 gameManager.selectedPiece = null
        gameManager.selectedPiece = null;

        // 7 disable buttons
        if (gameManager.userNotAlgo)
            DisableAllButtons();

        // 8 check if won (only place because its more efficient than checking in update)
        if (GridFunct.CheckWon()) Won();

        return true;
    }

    // takes piece from piece and puts it back to its initial position
    public static void Remove()
    {
        // Debug.Log("Remove button pressed");

        // 1 enable ghost spheres that overlap with a piece's sphere
        DisOrEnableGridSpheres(gameManager.selectedPiece.overlapsGridSpheres, true);

        // 2 reset piece's overlapsGridSpheres var to empty
        gameManager.selectedPiece.overlapsGridSpheres = new GameObject[gameManager.selectedPiece.sphereNr];

        // 3 placed variable in piece = false
        gameManager.selectedPiece.placed = false;

        // 4 deselect (inital position)
        PieceUnselected();
    }

    // = initial game state + initial level build/ removes all user placed pieces (not pieces placed by hint)
    public static void Restart()
    {
        // Debug.Log("Restart button pressed");

        // 1 reset gameManager.selectedPiece
        PieceUnselected();

        // 1 remove all moveable pieces in grid 
        foreach (GameObject p in gameManager.allPieces)
        {
            Piece piece = p.GetComponent<Piece>();
            if (piece.moveable && piece.placed)
            {
                GameManager.gameManager.selectedPiece = piece; // TODO remove gamemanager
                GameManager.Remove();
            }
        }
    }
    
    // helper function to calculate which angle the piece has to be turned to fit in the grid
    static bool CalcRotationAmount(int toTurn, int other, int y, int negPos) // toTurn: x if dir=x, z if dir=z; independent of grid rotation
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

    // dis- or enables the specified buttons in toChange
    public static void DisOrEnableButtons(int[] toChange, bool disOrEnable) // button numbers of buttons that should be dis- or enabled as bool
    {
        for (int i = 0; i < toChange.Length; i++)
        {
            // Debug.Log("Buttons " + toChange[i] + " disabled.");

            foreach (ButtonFunct button in gameManager.buttons) 
            {                
                if (button.buttonNr == toChange[i])
                {
                    button.gameObject.SetActive(disOrEnable);
                }
            }
        }
    }

    // called on scene start
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
        int[] toDisable = new int[6]; // on top: x and z buttons in both dir.s disabled
        int[] toEnable = new int[6];
        int i = 0;
        int j = 0;

        if (gameManager.selectedPiece.gridPos.x == 0) // no negative x
            toDisable[i++] = 3;
        else
            toEnable[j++] = 3;
        if (gameManager.selectedPiece.gridPos.x == 5 - gameManager.selectedPiece.gridPos.y - gameManager.selectedPiece.gridPos.z) // no positive x
            toDisable[i++] = 4;
        else
            toEnable[j++] = 4;

        if (gameManager.selectedPiece.gridPos.y == 0) // no negative y
            toDisable[i++] = 5;
        else
            toEnable[j++] = 5;
        if (gameManager.selectedPiece.gridPos.y == 5 - gameManager.selectedPiece.gridPos.x - gameManager.selectedPiece.gridPos.z) // no positive y
            toDisable[i++] = 6;
        else
            toEnable[j++] = 6;

        if (gameManager.selectedPiece.gridPos.z == 0) // no negative z
            toDisable[i++] = 7;
        else
            toEnable[j++] = 7;
        if (gameManager.selectedPiece.gridPos.z == 5 - gameManager.selectedPiece.gridPos.y - gameManager.selectedPiece.gridPos.x) // no positive z
            toDisable[i++] = 8;
        else
            toEnable[j++] = 8;

        DisOrEnableButtons(toDisable, false);
        DisOrEnableButtons(toEnable, true);
    }

    // returns false if not placeable, and/ or puts gridSpheres in overlapsGridSpheres variable of gameManager.selectedPiece
    public static bool OverlappingSpheres
    {
        get // recommended by Editor bc after reopening scene, function was not gone into anymore
        {
            gameManager.selectedPiece.overlapsGridSpheres = new GameObject[gameManager.selectedPiece.sphereNr];

            // 1 iterate through all spheres of the piece
            for (int i = 0; i < gameManager.selectedPiece.sphereNr; i++)
            {
                // 2 check if collider detects collisions
                Collider pieceSphere = gameManager.selectedPiece.gameObject.transform.GetChild(i).gameObject.GetComponent<SphereCollider>();

                // 3 iterate through all gridSpheres
                foreach (SphereCollider gridSphere in gameManager.gridPoints)
                {
                    // 4 check if gridSphere is active, if not: continue with next gridSphere
                    if (!gridSphere.gameObject.activeSelf) continue;

                    if (CheckIntersects(pieceSphere, gridSphere))
                    {
                        gameManager.selectedPiece.overlapsGridSpheres[i] = gridSphere.gameObject;
                        break; // go to next pieceSphere if overlapping gridSphere found
                    }
                }

                // 5 if no overlapping grid sphere found for one of the piece spheres: piece not placeable 
                if (gameManager.selectedPiece.overlapsGridSpheres[i] == null)
                    return false;
            }

            if (gameManager.selectedPiece.overlapsGridSpheres[gameManager.selectedPiece.sphereNr - 1] == null)
                return false;
            return true;
        }
    }

    // helper function to de- or activate the specified gridSpheres
    public static void DisOrEnableGridSpheres(GameObject[] gridSpheres, bool disOrEnable)
    {
        foreach (GameObject sphere in gridSpheres)
            if (sphere != null)
                sphere.SetActive(disOrEnable);
    }

    // self written intersects method (equates to bounds)
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
        if (gameManager.selectedPiece != null)
        {
            // 1 put selected to initial position & reset orientation
            if (!gameManager.selectedPiece.placed)
            {
                gameManager.selectedPiece.gameObject.transform.SetPositionAndRotation(gameManager.selectedPiece.initialPosition, Quaternion.identity);

                // 2 detach from grid (no child of grid anymore)
                gameManager.selectedPiece.gameObject.transform.SetParent(null);

                // 3 reset rotationNrs
                gameManager.selectedPiece.rotationNrs = new Vector3Int(0, 0, 0);
            }

            // 4 remove outline script
            Destroy(gameManager.selectedPiece.gameObject.GetComponent<Outline>());

            // 5 selected = null
            gameManager.selectedPiece = null;

            // 6 disable buttons (movement, rotation and place/ remove)
            int[] toDisable = new int[14];

            for (int i = 0; i < toDisable.Length; i++)
                toDisable[i] = i + 3;

            DisOrEnableButtons(toDisable, false);
        }
    }

    // activates win screen after level is won
    public void Won()
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
            foreach (SphereCollider collider in p.GetComponentsInChildren<SphereCollider>())
                collider.enabled = false;
        }

        won = true;

        // set up winning screen and activate
        winMessageText.text = "You have successfully solved this puzzle in " + Timer.staticTime.timerText + "!";
        gridParent.GetComponent<GameManager>().winMessageCanvas.SetActive(true);

        // if hint used, time can't be added to leaderbord
        if (SolutionManager.hintNr > 0) leaderboardMessage.SetActive(false);
    }
}

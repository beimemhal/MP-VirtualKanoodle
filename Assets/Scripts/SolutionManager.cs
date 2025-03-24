using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolutionManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public static int difficulty = 9;

    static int hintNr = 0;

    bool won = false;

    // solution storage for each piece store position and rotation in global space TODO clear when backtomainmenu 
    static Dictionary<string, Vector3> solutionPositions = new();
    static Dictionary<string, Quaternion> solutionRotations = new();

    // store pieces in the order they're placed 
    static List<Piece> lastPlaced = new(); 

    private void Start()
    {
        if (difficulty != 0)
        {
            // TODO 1 activate loading screen (if new level in between? -> maybe go back to main menu for that)
            DeActivateLoadingScreen(true);

            // 2 calls solving algorithm
            CalcSolution1(GameManager.allPieces, new()); // give a new List of allPieces (copy but w/ the same objects)

            // TODO 3 save solution in dicts
            SaveSolution();

            // TODO 4 level setup (first difficulty ones in lastPlaced not moveable)
            // LevelSetup();

            // TODO 5 reset pieces that are moveable
            // GameManager.Restart();

            // TODO 6 de-activate loading screen
            DeActivateLoadingScreen(false);
        }
        else
        {
            GameManager.userNotAlgo = true;
        }
        // TODO clear allPieces (...?) if solver = own scene
    }

    // TODO step by sstep solver
    Piece CalcSolution(List<GameObject> notTried, List<GameObject> previouslyTried) // returns piece that couldn't be placed, if there is one
    {
        // 2 select random piece of available ones
        int rdm = Random.Range(0, notTried.Count);
        Piece tryNext = notTried[rdm].GetComponent<Piece>();
        bool success = false;

        notTried.RemoveAt(rdm);

        tryNext.PieceSelected();

        // 4 iterate through each grid position
        int x = 0;
        int xMax = 5, zMax = 5;
        for (int y = 0; y < 5; y++)
        {
            if (success || won)
            {
                break;
            }
            for (int z = 0; z < zMax; z++)
            {
                if (success || won)
                {
                    break;
                }
                for (x = 0; x < xMax; x++)
                {
                    if (success || won)
                    {
                        break;
                    }
                    // 5 iterate through each possible rotation/ orientation TODO not if current grid point disabled
                    for (int j = 0; j < 4; j++) // rotate in z
                    {
                        if (success || won)
                        {
                            break;
                        }
                        for (int k = 0; k < 6; k++) // rotate in y
                        {
                            if (success || won)
                            {
                                break;
                            }
                            for (int l = 0; l < 4; l++) // rotate in x
                            {
                                if (success || won)
                                {
                                    break;
                                }
                                // 6 try placing it
                                success = gameManager.Place();
                            }
                        }
                    }
                }
            }
        }
        GameManager.selectedPiece = null; // TODO new -> if piece placed successfully -> done automatically -> make selectedPiece again after rec. call

        // 7 if successful: remove from triedNotPlaced list and add to lastPlaced list
        if (success || won)
            lastPlaced.Add(GameManager.selectedPiece);
        
        won = true;
        GameManager.userNotAlgo = true;

        return null;
    }

    // solving algorithm TODO (tree-based: recursively)
    Piece CalcSolution1(List<GameObject> notTried, List<GameObject> previouslyTried) // returns piece that couldn't be placed, if there is one
    {
        // 0 new triedNotPlaced list
        List<GameObject> triedNotPlaced = new();
        Piece unsuccessfulPiece; // TODO delete? no, necessary

        // if piece couldn't get placed in lower tree node: remember position and rot. to go back and try placing same piece again in other positioning before moving on to the next one
        // TODO 
        Vector3Int placedPosition = new(0, 0, 0);
        Vector3Int placedRotation = new(0, 0, 0);

        // 1 base case: check if won: all pieces placed successfully
        if (BaseCaseCheck(notTried.Count))
            return null;

        // 2 select random piece of available ones
        int rdm = Random.Range(0, notTried.Count);
        Piece tryNext = notTried[rdm].GetComponent<Piece>();
        bool success = false;

        // 3 save tryNext piece in triedNotPlaced list and remove from notTried
        notTried.RemoveAt(rdm);
        triedNotPlaced.Add(tryNext.gameObject);

        tryNext.PieceSelected();

        while (placedPosition.y != 5 && placedRotation.x != 4 && !success) // try until placement successfull or all placements tried) TODO true?
        {
            // 4 iterate through each grid position
            int x = 0;
            int xMax = 5, zMax = 5;
            for (int y = placedPosition.y; y < 5; y++)
            {
                placedPosition.y = y;
                for (int z = placedPosition.z; z < zMax; z++)
                {
                    placedPosition.z = z;
                    for (x = placedPosition.x; x < xMax; x++)
                    {
                        placedPosition.x = x;
                        // 5 iterate through each possible rotation/ orientation TODO not if current grid point disabled
                        for (int j = placedRotation.x; j < 4; j++) // rotate in z
                        {
                            placedRotation.z = j;
                            for (int k = placedRotation.y; k < 6; k++) // rotate in y
                            {
                                placedRotation.y = k;
                                for (int l = placedRotation.z; l < 4; l++) // rotate in x
                                {
                                    placedRotation.x = l;

                                    // 6 try placing it
                                    success = gameManager.Place();

                                    Debug.Log("Piece " + tryNext.name + " placement is " + success); // TODO delete: "In call " + i + " piece placement is " + success);

                                    if (success || won)
                                    {
                                        Debug.Log("Piece successfully placed in " + x + ", " + y + ", " + z);

                                        // 7 if successful: remove from triedNotPlaced list and add to lastPlaced list (and unselect)
                                        triedNotPlaced.Remove(tryNext.gameObject);
                                        lastPlaced.Add(tryNext);

                                        // 9 recursive call 1
                                        if (previouslyTried.Count > 0)
                                            // if gone back previously and going down again in another branch: take previouslyTried in again as notTried (for that new branch)
                                            notTried.AddRange(previouslyTried);
                                        if (notTried.Count > 0)
                                        {
                                            unsuccessfulPiece = CalcSolution(notTried, triedNotPlaced);
                                        }
                                        else // all (3) lists = empty: all pieces placed = win TODO true???
                                        {
                                            won = true;
                                            GameManager.userNotAlgo = true;
                                        }

                                        break;
                                    }

                                    // re-select tryNext: might be unselected in rec. call
                                    if (GameManager.selectedPiece != tryNext)
                                        GameManager.selectedPiece = tryNext;
                                    // 5
                                    GameManager.TurnPiece('x', 1);
                                }
                                if (success || won)
                                {
                                    break; // iteratively break out of loops to try placing the next piece
                                }

                                // re-select tryNext: might be unselected in rec. call
                                if (GameManager.selectedPiece != tryNext)
                                    GameManager.selectedPiece = tryNext;
                                // 5
                                GameManager.TurnPiece('y', 1);
                                placedRotation.z = 0;
                            }
                            if (success || won)
                            {
                                break;
                            }

                            // re-select tryNext: might be unselected in rec. call
                            if (GameManager.selectedPiece != tryNext)
                                GameManager.selectedPiece = tryNext;
                            // 5
                            GameManager.TurnPiece('z', 1);
                            placedRotation.y = 0;
                        }
                        if (success || won)
                        {
                            break;
                        }

                        // re-select tryNext: might be unselected in rec. call
                        if (GameManager.selectedPiece != tryNext)
                            GameManager.selectedPiece = tryNext;
                        // 4 
                        GameManager.MovePiece('x', 1);
                        placedRotation.x = 0;
                    }
                    if (success || won)
                    {
                        break;
                    }

                    // re-select tryNext: might be unselected in rec. call
                    if (GameManager.selectedPiece != tryNext)
                        GameManager.selectedPiece = tryNext;
                    // 4 
                    GameManager.MovePiece('z', 1);

                    placedPosition.x = 0;
                    xMax = 5 - z - y;
                }
                if (success || won)
                {
                    break;
                }

                // re-select tryNext: might be unselected in rec. call
                if (GameManager.selectedPiece != tryNext)
                    GameManager.selectedPiece = tryNext;
                // 4 
                GameManager.MovePiece('y', 1);

                placedPosition.z = 0;
                zMax = 5 - y - x;
            }

            Debug.Log("No positioning found or won"); // in fct call " + i

            // 8 recursive call 2: rec. call 1 doesn't find a solution -> gone back from lower node
            if (!won && success)
            {
                Debug.Log("No positioning found");

                Piece lastTried = lastPlaced.ElementAt(lastPlaced.Count - 1);
                success = false;

                lastPlaced.RemoveAt(lastPlaced.Count - 1); // remove from lastPlaced list

                lastTried.PieceSelected(); // remove piece from grid
                GameManager.Remove();

                triedNotPlaced.Add(lastTried.gameObject); // add back to triedNotPlaced list

                unsuccessfulPiece = CalcSolution(notTried, triedNotPlaced);

                if (unsuccessfulPiece == null)
                    success = true;

                while (unsuccessfulPiece != null && !won) // recursive call unsuccessful (keep in same node, try all possible branches)
                {
                    triedNotPlaced.Add(unsuccessfulPiece.gameObject);
                    notTried.Remove(unsuccessfulPiece.gameObject);

                    // recursive call 3
                    unsuccessfulPiece = CalcSolution(notTried, triedNotPlaced);

                    if (unsuccessfulPiece == null)
                        success = true;
                }
            }
        }

        if (success || won)
            return null;
        else
            return tryNext;
    }

    // TODO help methods solver
    bool BaseCaseCheck(int notTriedCount) // break out of recursion call if returns true
    {
        if (GridFunct.CheckWon())
        {
            won = true;
            GameManager.userNotAlgo = true;

            Debug.Log("Solution found");

            return true;
        }
        // or: no remaining pieces in notTried list (doesn't garantee win, but needs going back up in tree)
        if (notTriedCount == 0)
            return true;
        
        return false;
    }

    // save solution in dicts
    void SaveSolution()
    {
        foreach (Piece p in lastPlaced)
        {
            solutionPositions.Add(p.name, p.transform.position);
            solutionRotations.Add(p.name, p.transform.rotation);
        }
    }

    // make (number of difficulty) pieces not moveable in order of lastPlaced
    void LevelSetup()
    {
        for (int i = 0; i < difficulty; i++)
        {
            Piece p = lastPlaced[i];
            p.moveable = false;
        }
    }

    // TODO
    void DeActivateLoadingScreen(bool deOrActive) // true if activate, false to deactivate
    {

    }

    public void GiveHint()
    {
        // 1 choose next moveable piece in lastPlaced
        Piece p = lastPlaced.ElementAt(difficulty + hintNr);

        // 2 remove intersecting other pieces
        if (p.placed) // makes sure it's not a child of the grid itself
        {
            GameManager.selectedPiece = p;
            GameManager.Remove();
        } else if (p == GameManager.selectedPiece)
            GameManager.PieceUnselected();
        // put in correct position
        p.gameObject.transform.position = solutionPositions[p.gameObject.name];
        p.gameObject.transform.rotation = solutionRotations[p.gameObject.name];

        RemoveIntersectingPieces(p);

        // 3 place piece and make not moveable
        GameManager.selectedPiece = p;
        gameManager.Place();
        p.moveable = false;

        // TODO refactorign 4 put (flickering) outline around hint piece for 3 sec

    }

    void RemoveIntersectingPieces(Piece p)
    {
        Piece[] overlapsOtherPieces = new Piece[p.sphereNr];

        // 1 iterate through all spheres of the piece
        for (int i = 0; i < p.sphereNr; i++)
        {
            // 2 check if collider detects collisions
            Collider pieceSphere = p.gameObject.transform.GetChild(i).gameObject.GetComponent<SphereCollider>();
            bool isOverlapping = false;
            
            // 3 iterate through all of grid's piece children
            for (int k = 56; k < GameManager.gridParent.transform.childCount; k++) 
            {
                if (!GameManager.gridParent.gameObject.transform.GetChild(k).gameObject.GetComponent<Piece>().moveable)
                    break;

                Collider[] otherPiecesSpheres = GameManager.gridParent.gameObject.transform.GetChild(k).gameObject.GetComponentsInChildren<SphereCollider>();

                // 4 iterate through other piece's spheres and check collisions
                foreach (Collider c in otherPiecesSpheres)
                {
                    isOverlapping = GameManager.CheckIntersects(pieceSphere, c);
                    if (isOverlapping)
                    {
                        overlapsOtherPieces[i] = c.transform.parent.GetComponent<Piece>();
                        break; // go to next pieceSphere if overlapping gridSphere found
                    }
                }
                if (isOverlapping)
                    break;
            }
        }

        // 5 remove intersecting pieces from grid
        foreach (Piece otherPiece in overlapsOtherPieces)
        {
            GameManager.selectedPiece = otherPiece;
            GameManager.Remove();
        }
    }
}

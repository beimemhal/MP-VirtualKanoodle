using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolutionManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public static int difficulty = 0;

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
            CalcSolution(GameManager.allPieces, new()); // give a new List of allPieces (copy but w/ the same objects)

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

    // solving algorithm TODO (tree-based: recursively)
    private Piece CalcSolution(List<GameObject> notTried, List<GameObject> previouslyTried) // returns piece that couldn't be placed, if there is one
    {
        int i = 0; // TODO delete

        // 0 new triedNotPlaced list
        List<GameObject> triedNotPlaced = new();
        Piece unsuccessfulPiece = null;

        // 0.5 if piece couldn't get placed in lower tree node: remember position and rot. to go back and try placing same piece again in other positioning before moving on to the next one TODO working?
        Vector3Int placedPosition = new(0, 0, 0);
        Vector3Int placedRotation = new(0, 0, 0);

        // 1 base case: check if won: all pieces placed successfully
        if (BaseCaseCheck(notTried.Count))
            return null;

        // 1.5 all pieces have been tried (pieces in previouslyTried = tried but not placeable or placement not resulted in solution) go back up to remove last successfully placed piece and try another one first
        if (notTried.Count == 0 && previouslyTried.Count > 0) // TODO: make sure previously tried correct at each iteration
            return previouslyTried.ElementAt(0).GetComponent<Piece>();
        
        else if (notTried.Count == 0)
            return null; 
        

        // 2 select random piece of available ones
        int rdm = Random.Range(0, notTried.Count);
        Piece tryNext = notTried[rdm].GetComponent<Piece>();
        bool success = false;

        // 3 save tryNext piece in triedNotPlaced list and remove from notTried
        notTried.RemoveAt(rdm);
        triedNotPlaced.Add(tryNext.gameObject);

        tryNext.PieceSelected();

        // while (placedPosition.y + placedRotation.x + placedRotation.y + placedRotation.z < 16 || success) // try until placement successfull or all placements tried) TODO true?
        {
            // 4 iterate through each grid position
            int x = 0;
            int xMax = 5, zMax = 5;
            for (int y = placedPosition.y; y <= 5; y++)
            {
                placedPosition.y = y;
                for (int z = placedPosition.z; z <= zMax; z++)
                {
                    placedPosition.z = z;
                    for (x = placedPosition.x; x <= xMax; x++)
                    {
                        placedPosition.x = x;

                        // 4.5 if grid at x, y, z = disabled: continue with next position;
                        if (!GridFunct.gridPoints[GridFunct.CalcGridSpaceToArrayIndex(new Vector3Int(x, y, z))].gameObject.activeSelf)
                        {
                            Debug.Log("Placement skipped.");

                            if (x == xMax)
                                break;

                            GameManager.MovePiece('x', 1);
                            continue;
                        }

                        // 5 iterate through each possible rotation/ orientation
                        for (int j = placedRotation.z; j < 4; j++) // rotate in z TODO refactor: start at random number of rotation to make it more random -> potentially slower if always start at weird angle
                        {
                            placedRotation.z = j;
                            for (int k = placedRotation.y; k < 6; k++) // rotate in y
                            {
                                placedRotation.y = k;
                                for (int l = placedRotation.x; l < 4; l++) // rotate in x
                                {
                                    placedRotation.x = l;

                                    // 6 try placing it
                                    success = gameManager.Place();

                                    if (!success)
                                    {
                                        i++;
                                    } 
                                    else 
                                        Debug.Log("Piece " + tryNext.name + " placed after " + i + " unsuccessful tries.");

                                    if (success || won)
                                    {
                                        // 6.5 check if grid point is isolated: remove and continue trying to place differently
                                        if (GridFunct.CheckIsolatedPoints())
                                        {
                                            if (GameManager.selectedPiece != tryNext) // Place() removes selectedPiece
                                                GameManager.selectedPiece = tryNext;

                                            GameManager.Remove();
                                            success = false;

                                            // go to next positioning
                                            if (GameManager.selectedPiece != tryNext) // Remove() removes selectedPiece
                                            GameManager.selectedPiece = tryNext;

                                            GameManager.TurnPiece('x', 1);
                                            placedRotation.x = 0;
                                            break;
                                        }

                                        Debug.Log("Piece successfully placed in " + x + ", " + y + ", " + z);

                                        // 7 if successful: remove from triedNotPlaced list and add to lastPlaced list (and unselect)
                                        triedNotPlaced.Remove(tryNext.gameObject);
                                        lastPlaced.Add(tryNext);

                                        // 9 recursive call 1
                                        List<GameObject> tmpNT = new(notTried); // TODO try adding to notPlaced
                                        if (previouslyTried.Count > 0)
                                        {
                                            // if gone back previously and going down again in another branch: take previouslyTried in again as notTried (for that new branch)
                                            tmpNT.AddRange(previouslyTried);
                                        }
                                        if (tmpNT.Count > 0)
                                        {
                                            unsuccessfulPiece = CalcSolution(new(notTried), new(triedNotPlaced));
                                        } 
                                        else if (triedNotPlaced.Count > 0)
                                            unsuccessfulPiece = CalcSolution(new(triedNotPlaced), new());
                                        else // all (three) lists (notTried, previouslyTried, triedNotPlaced) = empty: all pieces placed = win TODO true???
                                        {
                                            won = true;
                                            GameManager.userNotAlgo = true;
                                        }

                                        break;
                                    }

                                    // re-select tryNext: might be unselected in rec. call or removed in Place() or Remove()
                                    if (GameManager.selectedPiece != tryNext)
                                        GameManager.selectedPiece = tryNext;

                                    // 5
                                    GameManager.TurnPiece('x', 1);
                                    placedRotation.x = 0;
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
                                placedRotation.y = 0;
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
                            placedRotation.z = 0;
                        }
                        if (success || won)
                        {
                            break;
                        }

                        // re-select tryNext: might be unselected in rec. call
                        if (GameManager.selectedPiece != tryNext)
                            GameManager.selectedPiece = tryNext;

                        // 4 
                        if (x == xMax)
                        {
                            break;
                        }
                        GameManager.MovePiece('x', 1);
                    }
                    if (success || won)
                    {
                        break;
                    }

                    // re-select tryNext: might be unselected in rec. call
                    if (GameManager.selectedPiece != tryNext)
                        GameManager.selectedPiece = tryNext;

                    // 4 
                    if (z == zMax)
                        break;

                    placedPosition.x = 0;
                    GameManager.selectedPiece.gridPos.x = 0;
                    xMax = 4 - z - y; // = 4 because z (only, needs to calc w/ new one already) counted up after this (new loop it.)

                    GameManager.MovePiece('z', 1);
                }
                if (success || won)
                {
                    break;
                }

                // re-select tryNext: might be unselected in rec. call
                if (GameManager.selectedPiece != tryNext)
                    GameManager.selectedPiece = tryNext;

                // 4 
                if (y == 5)
                    break;

                placedPosition.z = 0;
                GameManager.selectedPiece.gridPos.z = 0;
                placedPosition.x = 0;
                GameManager.selectedPiece.gridPos.x = 0;
                zMax = 4 - y - x;

                GameManager.MovePiece('y', 1);
            }

            // 8 recursive call 2: rec. call 1 doesn't find a solution -> gone back from lower node
            if (!won && success)
            {
                Debug.Log("No solution found for placement of " + tryNext.name + ". Remove and try diff. position.");
                // remove last from lastPlaced list again
                Piece lastTried = lastPlaced.ElementAt(lastPlaced.Count - 1);
                lastPlaced.RemoveAt(lastPlaced.Count - 1); 
                success = false;

                lastTried.PieceSelected(); // remove piece from grid
                GameManager.Remove(); // TODO lastPlaced also has pieces that are not successfully placed or gridspheres not right 

                triedNotPlaced.Add(lastTried.gameObject); // add back to triedNotPlaced list

                /* TODO causes error in lastPlaced?
                if (unsuccessfulPiece != null && lastTried == tryNext)
                {
                    notTried.Add(unsuccessfulPiece.gameObject);
                    triedNotPlaced.Remove(unsuccessfulPiece.gameObject);
                }
                */
                unsuccessfulPiece = CalcSolution(new(notTried), new(triedNotPlaced));

                if (unsuccessfulPiece == null)
                    success = true;

                while (unsuccessfulPiece != null && !won) // recursive call unsuccessful (keep in same node, try all possible branches)
                {
                    if (notTried.Count == 0)
                        break;

                    triedNotPlaced.Add(unsuccessfulPiece.gameObject);
                    notTried.Remove(unsuccessfulPiece.gameObject);

                    // recursive call 3
                    unsuccessfulPiece = CalcSolution(new(notTried), new(triedNotPlaced));

                    if (unsuccessfulPiece == null) // TODO wrong?
                        success = true;
                }
            }
        }

        if (success || won)
        {
            Debug.Log("Success or won. Return at end.");
            return null;
        }
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
        // if (notTriedCount == 0)
            // return true;
        
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

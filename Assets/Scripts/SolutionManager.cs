using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class SolutionManager : MonoBehaviour
{
    public static int difficulty = 10;

    public static int hintNr = 0;

    // solution storage for each piece store position and rotation in global space TODO clear when backtomainmenu 
    public static Dictionary<string, Vector3> solutionPositions = new();
    public static Dictionary<string, Quaternion> solutionRotations = new();

    // store pieces in the order they're placed 
    public static List<Piece> lastPlaced = new(); 

    private void Start()
    {
        hintNr = 0;

        /* TODO de-comment if solver works
        if (difficulty != 0 && !GameManager.userNotAlgo)
    {
        // TODO 1 activate loading screen (if new level in between? -> maybe go back to main menu for that)
        DeActivateLoadingScreen(true);

        // 2 calls solving algorithm
        CalcSolution(GameManager.allPieces, new());

        // TODO delete
        List<GameObject> previouslyTried = new();
        while (!GameManager.userNotAlgo)
        {
            Piece unsuccessfulPiece = CalcSolution1(GameManager.allPieces, new(previouslyTried), null); // give a new List of allPieces (copy but w/ the same objects)

            // first placed piece didn't result in solution
            if (unsuccessfulPiece != null)
            {
                unsuccessfulPiece.PieceSelected();
                GameManager.Remove();
                lastPlaced.Remove(unsuccessfulPiece);

                previouslyTried.Add(unsuccessfulPiece.gameObject);
            }
        }

        // TODO delete
        // CalcSolution2(new List<GameObject>(), new HashSet<GameObject>());

        /

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
    */
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
        if (BaseCaseCheck())
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
                        if (!GameManager.gridPoints[GridFunct.CalcGridSpaceToArrayIndex(new Vector3Int(x, y, z))].gameObject.activeSelf)
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
                                    success = GameManager.gameManager.Place();

                                    if (!success)
                                    {
                                        i++;
                                    } 
                                    else 
                                        Debug.Log("Piece " + tryNext.name + " placed after " + i + " unsuccessful tries.");

                                    if (success)
                                    {
                                        // 6.5 check if grid point is isolated: remove and continue trying to place differently 
                                        if (lastPlaced.Count > 4 && GridFunct.CheckIsolatedPoints()) // do only after at least 4 pieces have been placed (if won: returns false bc all inactive)
                                        {
                                            tryNext.PieceSelected(); // Place() removes selectedPiece
                                            GameManager.Remove();
                                            success = false;

                                            // go to next positioning
                                            tryNext.PieceSelected(); // Remove() removes selectedPiece
                                            GameManager.TurnPiece('x', 1);
                                            continue;
                                        }

                                        Debug.Log("Piece successfully placed in " + x + ", " + y + ", " + z);

                                        // 7 if successful: remove from triedNotPlaced list and add to lastPlaced list (and unselect)
                                        triedNotPlaced.Remove(tryNext.gameObject);
                                        lastPlaced.Add(tryNext);

                                        // 8 recursive call 1
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
                                            // GameManager.userNotAlgo = true; // indicates win/ solution found TODO true?
                                        }

                                        break;
                                    }

                                    // re-select tryNext: might be unselected in rec. call or removed in Place() or Remove()
                                    if (GameManager.selectedPiece != tryNext)
                                        tryNext.PieceSelected();

                                    // 5
                                    GameManager.TurnPiece('x', 1);
                                    placedRotation.x = 0;
                                }
                                if (success || GameManager.userNotAlgo)
                                {
                                    break; // iteratively break out of loops to try placing the next piece
                                }

                                // re-select tryNext: might be unselected in rec. call
                                if (GameManager.selectedPiece != tryNext)
                                    tryNext.PieceSelected();

                                // 5
                                GameManager.TurnPiece('y', 1);
                                placedRotation.y = 0;
                            }
                            if (success || GameManager.userNotAlgo)
                            {
                                break;
                            }

                            // re-select tryNext: might be unselected in rec. call
                            if (GameManager.selectedPiece != tryNext)
                                tryNext.PieceSelected();

                            // 5
                            GameManager.TurnPiece('z', 1);
                            placedRotation.z = 0;
                        }
                        if (success || GameManager.userNotAlgo)
                        {
                            break;
                        }

                        // re-select tryNext: might be unselected in rec. call
                        if (GameManager.selectedPiece != tryNext)
                            tryNext.PieceSelected();

                        // 4 
                        if (x == xMax)
                        {
                            break;
                        }
                        GameManager.MovePiece('x', 1);
                    }
                    if (success || GameManager.userNotAlgo)
                    {
                        break;
                    }

                    // re-select tryNext: might be unselected in rec. call
                    if (GameManager.selectedPiece != tryNext)
                        tryNext.PieceSelected();

                    // 4 
                    if (z == zMax)
                        break;

                    placedPosition.x = 0;
                    GameManager.selectedPiece.gridPos.x = 0;
                    xMax = 4 - z - y; // = 4 because z (only, needs to calc w/ new one already) counted up after this (new loop it.)

                    GameManager.MovePiece('z', 1);
                }
                if (success || GameManager.userNotAlgo)
                {
                    break;
                }

                // re-select tryNext: might be unselected in rec. call
                if (GameManager.selectedPiece != tryNext)
                    tryNext.PieceSelected();

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

            // if (unsuccessfulPiece != null) TODO ????
                // try another piece first -> put in previously tried?

            // 9 recursive call 2: rec. call 1 doesn't find a solution -> gone back from lower node
            if (!GameManager.userNotAlgo && success)
            {
                Debug.Log("No solution found for placement of " + tryNext.name + ". Remove and try diff. position.");
                // remove last from lastPlaced list again
                Piece lastTried = lastPlaced.ElementAt(lastPlaced.Count - 1);
                // if (lastTried == tryNext) TODO ?
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

                while (unsuccessfulPiece != null && !GameManager.userNotAlgo) // 10 recursive call 2 unsuccessful (keep in same node, try all possible branches)
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

        // leave function when no placement found or all pieces succesfully placed (won)
        if (GameManager.userNotAlgo) // success || TODO success not necessary cause if success, all other pieces in lower levels have to also be success in the recursive calls
        {
            Debug.Log("Success or won. Return at end.");
            return null;
        }
        else
            return tryNext;
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

    // void DeActivateLoadingScreen(bool deOrActive) // true if activate, false to deactivate
    // {}

    public void GiveHint()
    {
        // 0 turn grid to initial position
        int turns = GridFunct.gridTurns;
        if (turns == 1)
            GameManager.TurnGridY(-1);
        else if (turns == 2)
            GameManager.TurnGridY(1);

        // 1 choose next moveable piece in lastPlaced
        Piece p = GameObject.Find(HardCodedLvl.lastPlaced.ElementAt(difficulty + hintNr)).GetComponent<Piece>(); // TODO change back when solver works

        // 2 remove intersecting other pieces
        if (p.placed) // makes sure it's not a child of the grid itself
        {
            p.PieceSelected();
            GameManager.Remove();
        } 
        if (GameManager.selectedPiece != null)
            GameManager.PieceUnselected();
        // put in correct position
        p.gameObject.transform.position = solutionPositions[p.gameObject.name];
        p.gameObject.transform.rotation = solutionRotations[p.gameObject.name];

        RemoveIntersectingPieces(p);

        // 3 place piece and make not moveable
        GameManager.selectedPiece = p;
        GameManager.selectedPiece.gameObject.transform.SetParent(GameManager.gridParent.transform);
        GameManager.gameManager.Place();
        p.moveable = false;

        // 4 count up number of hints
        hintNr++;

        // turn grid back
        if (turns == 1)
            GameManager.TurnGridY(1);
        else if (turns == 2)
            GameManager.TurnGridY(-1);
    }

    // help methods solver
    bool BaseCaseCheck() // break out of recursion call if returns true
    {
        if (GridFunct.CheckWon())
        {
            GameManager.userNotAlgo = true;

            Debug.Log("Solution found");

            return true;
        }

        return false;
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
            
            // 3 iterate through all of grid's piece's children
            for (int k = 56; k < GameManager.gridParent.transform.childCount; k++) 
            {
                if (GameManager.gridParent.gameObject.transform.GetChild(k).gameObject.GetComponent<Piece>().moveable)
                {
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
        }

        // 5 remove intersecting pieces from grid
        foreach (Piece otherPiece in overlapsOtherPieces)
        {
            if (otherPiece != null)
            {
                GameManager.selectedPiece = otherPiece;
                GameManager.Remove();
            }
        }
    }

    // TODO delete try 2
    private Piece CalcSolution1(List<GameObject> notTried, List<GameObject> previouslyTried, Piece tryNext) // returns piece that couldn't be placed, if there is one
    {
        // 0 new triedNotPlaced list
        Piece unsuccessfulPiece = null;

        // 1 base case: check if won: all pieces placed successfully
        if (GridFunct.CheckWon())
        {
            GameManager.userNotAlgo = true;

            Debug.Log("Solution found");

            return null;
        }

        // 2 select random piece of available ones
        if (tryNext == null)
        {
            int rdm = Random.Range(0, notTried.Count);
            tryNext = notTried[rdm].GetComponent<Piece>();

            // 3 save tryNext piece in triedNotPlaced list and remove from notTried
            // notTried.RemoveAt(rdm); remains there until placed successfully
        }
        bool success = false;

        tryNext.PieceSelected();

        // 4 iterate through each grid position
        int x = 0;
        int xMax = 5, zMax = 5;
        for (int y = 0; y <= 5; y++)
        {
            for (int z = 0; z <= zMax; z++)
            {
                for (x = 0; x <= xMax; x++)
                {
                    // 4.5 if grid at x, y, z = disabled: continue with next position;
                    if (!GameManager.gridPoints[GridFunct.CalcGridSpaceToArrayIndex(new Vector3Int(x, y, z))].gameObject.activeSelf)
                    {
                        Debug.Log("Placement skipped.");

                        if (x == xMax) break;

                        GameManager.MovePiece('x', 1);
                        continue;
                    }

                    // 5 iterate through each possible rotation/ orientation
                    for (int j = 0; j < 4; j++) // rotate in z
                    {
                        for (int k = 0; k < 6; k++) // rotate in y
                        {
                            for (int l = 0; l < 4; l++) // rotate in x
                            {
                                // 6 try placing it
                                success = GameManager.gameManager.Place();

                                if (success)
                                {
                                    // 6.5 check if grid point is isolated: remove and continue trying to place differently 
                                    if (lastPlaced.Count > 4 && GridFunct.CheckIsolatedPoints()) // do only after at least 4 pieces have been placed (if won: returns false bc all inactive) TODO evtl delete?
                                    {
                                        tryNext.PieceSelected(); // Place() removes selectedPiece
                                        GameManager.Remove();
                                        success = false;

                                        // go to next positioning
                                        tryNext.PieceSelected(); // Remove() removes selectedPiece
                                        GameManager.TurnPiece('x', 1);
                                        continue;
                                    }

                                    Debug.Log("Piece successfully placed in " + x + ", " + y + ", " + z);

                                    // 7 remove from notTried list and add to lastPlaced list (and unselect)
                                    notTried.Remove(tryNext.gameObject);
                                    lastPlaced.Add(tryNext);

                                    // 8 recursive call 1
                                    List<GameObject> tmp = new(notTried);
                                    if (previouslyTried.Count > 0 && unsuccessfulPiece == null)
                                    {
                                        tmp.AddRange(previouslyTried);
                                    }
                                    unsuccessfulPiece = CalcSolution1(tmp, previouslyTried, unsuccessfulPiece);

                                    // if win in recursion: iteratively break out of loops
                                    if (GameManager.userNotAlgo) break;

                                    // 9 recursive call unsuccessful: continue placing tryNext another way
                                    if (unsuccessfulPiece != null)
                                    {
                                        tryNext.PieceSelected();
                                        GameManager.Remove();
                                        notTried.Add(tryNext.gameObject);
                                        lastPlaced.Remove(tryNext);
                                        success = false;
                                    }
                                }

                                // re-select tryNext: unselected in rec. call or in Place() or Remove()
                                if (GameManager.selectedPiece != tryNext) tryNext.PieceSelected();
                                // 5
                                GameManager.TurnPiece('x', 1);
                            }
                            if (GameManager.userNotAlgo) break;

                            // 5
                            if (GameManager.selectedPiece != tryNext) tryNext.PieceSelected();
                            GameManager.TurnPiece('y', 1);
                        }
                        if (GameManager.userNotAlgo) break;

                        // 5
                        if (GameManager.selectedPiece != tryNext) tryNext.PieceSelected();
                        GameManager.TurnPiece('z', 1);
                    }
                    if (GameManager.userNotAlgo) break;

                    // 4
                    if (GameManager.selectedPiece != tryNext) tryNext.PieceSelected();
                    if (x == xMax) break;

                    GameManager.MovePiece('x', 1);
                }
                if (GameManager.userNotAlgo) break;

                // 4
                if (GameManager.selectedPiece != tryNext) tryNext.PieceSelected();
                if (z == zMax) break;

                GameManager.selectedPiece.gridPos.x = 0;
                xMax = 4 - z - y; // = 4 because z (only, needs to calc w/ new one already) counted up after this (new loop it.)

                GameManager.MovePiece('z', 1);
            }
            if (GameManager.userNotAlgo) break;

            // 4 
            if (GameManager.selectedPiece != tryNext) tryNext.PieceSelected();
            if (y == 5) break;

            GameManager.selectedPiece.gridPos.z = 0;
            GameManager.selectedPiece.gridPos.x = 0;
            zMax = 4 - y - x;

            GameManager.MovePiece('y', 1);
        } // piece cannot be placed anywhere on the grid: only reason to go up (apart from win)

        if (!success) 

        // leave function when no placement found or all pieces succesfully placed (won)
        if (GameManager.userNotAlgo)
        {
            Debug.Log("Success or won. Return at end.");
            return null;
        }

        notTried.Remove(tryNext.gameObject);
        return tryNext; // unsuccessful branch
    }


    // TODO delete try 3
    private void CalcSolution2(List<GameObject> placed, HashSet<GameObject> previouslyTried) // returns piece that couldn't be placed, if there is one
    {
        // 1 base case: check if won: all pieces placed successfully
        if (GridFunct.CheckWon()) // placed.Count == GameManager.allPieces.Count
        {
            GameManager.userNotAlgo = true;

            Debug.Log("Solution found");

            return;
        }

        // 2 select random piece of available ones
        List<GameObject> notTried = new(GameManager.allPieces);
        notTried.RemoveAll(obj => previouslyTried.Contains(obj)); // Remove used objects
        Shuffle(notTried); // random order

        foreach (GameObject g in notTried)
        {
            Piece tryNext = g.GetComponent<Piece>();
            tryNext.PieceSelected();
            bool success = false;

            // 4 iterate through each grid position
            int x = 0;
            int xMax = 5, zMax = 5;
            for (int y = 0; y <= 5; y++)
            {
                for (int z = 0; z <= zMax; z++)
                {
                    for (x = 0; x <= xMax; x++)
                    {
                        // 4.5 if grid at x, y, z = disabled: continue with next position;
                        if (!GameManager.gridPoints[GridFunct.CalcGridSpaceToArrayIndex(new Vector3Int(x, y, z))].gameObject.activeSelf)
                        {
                            Debug.Log("Placement skipped.");

                            if (x == xMax) break;

                            GameManager.MovePiece('x', 1);
                            continue;
                        }

                        // 5 iterate through each possible rotation/ orientation
                        for (int j = 0; j < 4; j++) // rotate in z
                        {
                            for (int k = 0; k < 6; k++) // rotate in y
                            {
                                for (int l = 0; l < 4; l++) // rotate in x
                                {
                                    // 6 try placing it
                                    success = GameManager.gameManager.Place();

                                    if (success)
                                    {
                                        // 6.5 check if grid point is isolated: remove and continue trying to place differently 
                                        if (lastPlaced.Count > 4 && GridFunct.CheckIsolatedPoints()) // do only after at least 4 pieces have been placed (if won: returns false bc all inactive) TODO evtl delete?
                                        {
                                            tryNext.PieceSelected(); // Place() removes selectedPiece
                                            GameManager.Remove();
                                            success = false;

                                            // go to next positioning
                                            tryNext.PieceSelected(); // Remove() removes selectedPiece
                                            GameManager.TurnPiece('x', 1);
                                            continue;
                                        }

                                        Debug.Log("Piece successfully placed in " + x + ", " + y + ", " + z);

                                        // 7 remove from notTried list and add to lastPlaced list (and unselect)
                                        placed.Add(tryNext.gameObject);
                                        previouslyTried.Add(tryNext.gameObject);

                                        // 8 recursive call 1
                                        CalcSolution2(placed, previouslyTried);

                                        // if win in recursion: iteratively break out of loops
                                        if (GameManager.userNotAlgo) break;

                                        // 9 backtrack
                                        placed.RemoveAt(placed.Count - 1);
                                        previouslyTried.Remove(g);
                                        tryNext.PieceSelected(); // Place() removes selectedPiece
                                        GameManager.Remove();
                                    }

                                    // re-select tryNext: unselected in rec. call or in Place() or Remove()
                                    if (GameManager.selectedPiece != tryNext) tryNext.PieceSelected();
                                    // 5
                                    GameManager.TurnPiece('x', 1);
                                }
                                if (GameManager.userNotAlgo) break;

                                // 5
                                if (GameManager.selectedPiece != tryNext) tryNext.PieceSelected();
                                GameManager.TurnPiece('y', 1);
                            }
                            if (GameManager.userNotAlgo) break;

                            // 5
                            if (GameManager.selectedPiece != tryNext) tryNext.PieceSelected();
                            GameManager.TurnPiece('z', 1);
                        }
                        if (GameManager.userNotAlgo) break;

                        // 4
                        if (GameManager.selectedPiece != tryNext) tryNext.PieceSelected();
                        if (x == xMax) break;

                        GameManager.MovePiece('x', 1);
                    }
                    if (GameManager.userNotAlgo) break;

                    // 4
                    if (GameManager.selectedPiece != tryNext) tryNext.PieceSelected();
                    if (z == zMax) break;

                    GameManager.selectedPiece.gridPos.x = 0;
                    xMax = 4 - z - y; // = 4 because z (only, needs to calc w/ new one already) counted up after this (new loop it.)

                    GameManager.MovePiece('z', 1);
                }
                if (GameManager.userNotAlgo) break;

                // 4 
                if (GameManager.selectedPiece != tryNext) tryNext.PieceSelected();
                if (y == 5) break;

                GameManager.selectedPiece.gridPos.z = 0;
                GameManager.selectedPiece.gridPos.x = 0;
                zMax = 4 - y - x;

                GameManager.MovePiece('y', 1);
            } // piece cannot be placed anywhere on the grid: only reason to go up (apart from win)
            if (GameManager.userNotAlgo) break;
        }
    }

    void Shuffle(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randIndex = Random.Range(0, i + 1);
            (list[i], list[randIndex]) = (list[randIndex], list[i]);
        }
    }
}

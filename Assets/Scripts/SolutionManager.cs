using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
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

    int i = 0; // TODO for debugging: delete

    private void Start()
    {
        if (difficulty != 0)
        {
            // TODO 1 activate loading screen (if new level in between? -> maybe go back to main menu for that)
            DeActivateLoadingScreen(true);

            // 2 calls solving algorithm
            CalcSolution(GameManager.allPieces.ToList(), new()); // give a new List of allPieces (copy but w/ the same objects)

            // TODO 3 save solution in dicts
            SaveSolution();

            // TODO 4 level setup (first difficulty ones in lastPlaced)
            LevelSetup();

            // TODO 5 reset pieces that are moveable
            GameManager.Restart();

            // TODO 6 de-activate loading screen
            DeActivateLoadingScreen(false);
        }
        else
        {
            GameManager.userNotAlgo = true;
        }
    }

    // solving algorithm TODO (tree-based: recursively)
    Piece CalcSolution(List<GameObject> notTried, List<GameObject> previouslyTried) // returns piece that couldn't be placed, if there is one
    {
        i++; // TODO delete

        // 0 new triedNotPlaced list
        List<GameObject> triedNotPlaced = new();
        Piece unsuccessfulPiece; // TODO delete

        // if piece couldn't get placed in lower tree node: remember position and rot. to go back and try placing same piece again in other positioning before moving on to the next one
        // TODO 
        Vector3Int placedPosition = new(0, 0, 0);
        Vector3Int placedRotation = new(0, 0, 0);

        // 1 base case: check if won: all pieces placed successfully
        if (GridFunct.CheckWon())
        {
            won = true;
            GameManager.userNotAlgo = true;

            Debug.Log("Solution found");

            return null;
        }
        // or: no remaining pieces in notTried list
        if (notTried.Count == 0)
            return null;

        // 2 select random piece of available ones
        int rdm = Random.Range(0, notTried.Count);
        Piece tryNext = notTried[rdm].GetComponent<Piece>();
        bool success = false;

        // 3 save tryNext piece in triedNotPlaced list and remove from notTried
        notTried.RemoveAt(rdm);
        triedNotPlaced.Add(tryNext.gameObject);

        tryNext.PieceSelected();

        while (placedPosition.y != 5 && placedRotation.x != 4)
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
                            placedRotation.x = x;
                            for (int k = placedRotation.y; k < 6; k++) // rotate in y
                            {
                                placedRotation.y = y;
                                for (int l = placedRotation.z; l < 4; l++) // rotate in x
                                {
                                    placedRotation.z = z;
                                    // 6 try placing it
                                    success = gameManager.Place(); // TODO doesn't work

                                    Debug.Log("In call " + i + " piece placement is " + success);

                                    if (success || won)
                                    {
                                        Debug.Log("Piece placed");

                                        // 7 if successful: remove from triedNotPlaced list and add to lastPlaced list
                                        triedNotPlaced.Remove(GameManager.selectedPiece.gameObject);
                                        lastPlaced.Add(GameManager.selectedPiece);

                                        // 9 recursive call 1(.1 and 1.2)
                                        if (notTried.Count > 0)
                                        {
                                            if (previouslyTried.Count > 0)
                                                notTried.AddRange(previouslyTried); // if gone back previously and going down again in another branch: take previouslyTried in again as notTried (for that new branch)
                                            unsuccessfulPiece = CalcSolution(notTried, triedNotPlaced);
                                        }
                                        else if (previouslyTried.Count > 0)
                                            unsuccessfulPiece = CalcSolution(previouslyTried, triedNotPlaced);
                                        else // both lists = empty: all pieces placed = win TODO true???
                                        {
                                            won = true;
                                            GameManager.userNotAlgo = true;
                                        }

                                        break;
                                    }

                                    // 5
                                    GameManager.TurnPiece('x', 1);
                                }
                                if (success || won)
                                {
                                    break; // iteratively break out of loops to try placing the next piece
                                }

                                // 5
                                GameManager.TurnPiece('y', 1);
                                placedRotation.z = 0;
                            }
                            if (success || won)
                            {
                                break;
                            }

                            // 5
                            GameManager.TurnPiece('z', 1);
                            placedRotation.y = 0;
                        }
                        if (success || won)
                        {
                            break;
                        }

                        // 4 
                        GameManager.MovePiece('x', 1);
                        placedRotation.x = 0;
                    }
                    if (success || won)
                    {
                        break;
                    }

                    // 4 
                    GameManager.MovePiece('z', 1);

                    placedPosition.x = 0;
                    xMax = 5 - z - y;
                }
                if (success || won)
                {
                    break;
                }

                // 4 
                GameManager.MovePiece('y', 1);

                placedPosition.z = 0;
                zMax = 5 - y - x;
            }

            Debug.Log("No positioning found or won in fct call " + i);

            // 8 recursive call 2: rec. call 1 doesn't find a solution -> gone back from lower node
            if (!won && success)
            {
                Debug.Log("No positioning found");

                Piece lastTried = lastPlaced.ElementAt(lastPlaced.Count - 1);

                lastPlaced.RemoveAt(lastPlaced.Count - 1); // remove from lastPlaced list

                lastTried.PieceSelected(); // remove piece from grid
                GameManager.Remove();

                triedNotPlaced.Add(lastTried.gameObject); // add back to triedNotPlaced list

                unsuccessfulPiece = CalcSolution(notTried, triedNotPlaced);

                while (unsuccessfulPiece != null) // recursive call unsuccessful (keep in same node, try all possible branches)
                {
                    triedNotPlaced.Add(unsuccessfulPiece.gameObject);
                    notTried.Remove(unsuccessfulPiece.gameObject);

                    // recursive call 3
                    unsuccessfulPiece = CalcSolution(notTried, triedNotPlaced);
                }
            }
        }

        return tryNext; // TODO correct? if (won) return null;
    }

    // save solution in dicts
    void SaveSolution()
    {
        foreach (GameObject p in GameManager.allPieces)
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
        // place next moveable piece in lastPlaced
        Piece p = lastPlaced.ElementAt(difficulty + hintNr);
        p.gameObject.transform.position = solutionPositions[p.gameObject.name];
        p.gameObject.transform.rotation = solutionRotations[p.gameObject.name];
        GameManager.selectedPiece = p;
        gameManager.Place();

        // make piece not moveable
        p.moveable = false;

        // TODO put (flickering) outline around hint piece for 3 sec

    }



    // 2 try each piece TODO delete
    // for (int i = ; i > 0; i--) // TODO not tree based -> doesn't go back just one -> brute-force algo
    // {
}

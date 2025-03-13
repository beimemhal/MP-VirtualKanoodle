using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolutionManager : MonoBehaviour
{
    // int difficulty = 1; // TODO 

    bool won = false;

    // solution storage for each piece store position and rotation
    Dictionary<string, Vector3> solutionPositions = new();
    Dictionary<string, Quaternion> solutionRotations = new();

    // store pieces in the order 
    Stack<Piece> lastPlaced = new Stack<Piece>(12); 

    int i = 0; // TODO for debugging: delete

    private void Start()
    {
        // TODO 1 activate loading screen (if new level in between? -> maybe go back to main menu for that)
        DeActivateLoadingScreen(true);

        // 2 calls solving algorithm
        CalcSolution(GameManager.allPieces.ToList()); // give a new List of allPieces (copy but w/ the same objects)

        // TODO 3 save solution in dicts
        SaveSolution();

        // TODO 4 reset grid and pieces
        RemoveAll();

        // TODO 5 level setup
        LevelSetup();

        // TODO 6 de-activate loading screen
        DeActivateLoadingScreen(false);
    }

    // solving algorithm TODO (tree-based: recursively)
    void CalcSolution(List<GameObject> notTried) // called on scene start TODO question: are pieces copied and appear multiple times in scene? / destroyed after leaving an iteration
    {
        i++; // TODO delete

        // 1 base case: check if won: if all pieces placed succesfully
        if (GridFunct.CheckWon())
        {
            won = true;
            GameManager.userNotAlgo = true;

            Debug.Log("Solution found");

            return;
        }

        // 2 select random piece of available ones
        int rdm = Random.Range(0, notTried.Count);
        Piece tryNext = notTried[rdm].GetComponent<Piece>();
        bool success = false;

        // 3 if just gone back in recursive calls -> previously placed piece: remove & put back in notTried 
        if (notTried.Count > 12 - lastPlaced.Count) 
        {
            Debug.Log("Gone back once");

            i--; // TODO delete

            Piece lastTried = lastPlaced.Pop(); // remove from lastPlaced list
            lastTried.PieceSelected(); // to remove piece from grid
            GameManager.Remove(); 

            notTried.Add(lastTried.gameObject); // add back to notTried list
        }

        tryNext.PieceSelected();
        
        // 4 iterate through each grid position 
        int x = 0, y = 0, z = 0;
        int xMax = 5, zMax = 5;
        for (y = 0; y < 5; y++)
        {
            for (z = 0; z < zMax; z++)
            {
                for (x = 0; x < xMax; x++)
                {
                    // 5 iterate through each possible rotation/ orientation TODO not if current grid point disabled
                    for (int j = 0; j < 4; j++) // rotate in z
                    {
                        for (int k = 0; k < 6; k++) // rotate in y
                        {
                            for (int l = 0; l < 4; l++) // rotate in x
                            {
                                // 6 try placing it
                                success = GameManager.Place(); // TODO doesn't work

                                Debug.Log("In call " + i + " piece placement is " + success);

                                if (success)
                                {
                                    // 7 if successful: remove from notTried list and add to lastPlaced Stack
                                    notTried.RemoveAt(rdm);
                                    lastPlaced.Push(GameManager.selectedPiece);

                                    // 9 recursive call
                                    CalcSolution(notTried);

                                    Debug.Log("Piece placed");

                                    break;
                                }

                                // 5
                                GameManager.TurnPiece('x', 1);
                            }
                            if (success)
                            {
                                break; // iteratively break out of loops to try placing the next piece
                            }

                            // 5
                            GameManager.TurnPiece('y', 1);
                        }
                        if (success)
                        {
                            break;
                        }

                        // 5
                        GameManager.TurnPiece('z', 1);
                    }
                    if (success)
                    {
                        break;
                    }

                    // 4 
                    GameManager.MovePiece('x', 1);


                }
                if (success)
                {
                    break;
                }

                // 4 
                GameManager.MovePiece('z', 1);
                
                xMax = 5 - z - y;
            }
            if (success)
            {
                break;
            }

            // 4 
            GameManager.MovePiece('y', 1);
            
            zMax = 5 - y - x;
        }

        Debug.Log("No positioning found or won in fct call " + i);

        // 8 recursive call doesn't find a solution/ last tried piece couln't be placed -> gone back
        if (!won && success)
        {
            Debug.Log("No positioning found");

            // notTried doesn't contain the piece placed in current fct call -> gets added again in recursive call (in step 3) after picking the next piece randomly (so that currently piece won't be picked again)
            CalcSolution(notTried); 
        }

    }

    // TODO save solution in dicts
    void SaveSolution()
    {

    }

    // TODO reset grid and pieces
    void RemoveAll()
    {

    }

    // placing pre-set pieces according to difficulty var TODO
    void LevelSetup()
    {
        // set pieces.moveable = false; TODO
    }

    // TODO
    void DeActivateLoadingScreen(bool deOrActive) // true if activate, false to deactivate
    {

    }


    // 2 try each piece TODO delete
    // for (int i = ; i > 0; i--) // TODO not tree based -> doesn't go back just one -> brute-force algo
    // {
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolutionManager : MonoBehaviour
{
    int difficulty = 1; // TODO 1 to 5 ?

    bool won = false;

    // solution storage for each piece store position and rotation
    Dictionary<string, Vector3> solutionPositions = new();
    Dictionary<string, Quaternion> solutionRotations = new();

    private void Start()
    {        
        // TODO 1 activate loading screen (if new level in between? -> maybe go back to main menu for that)

        // calls solving algorithm
        // CalcSolution(GameManager.allPieces.ToList()); // give a new List of allPieces (copy but w/ the same objects)

        // TODO save solution in dicts

        // TODO reset grid and pieces

        // TODO level setup
        LevelSetup();

        // TODO de-activate loading screen

    }

    // placing pre-set pieces according to difficulty var TODO
    void LevelSetup()
    {
        // set pieces.moveable = false; TODO
    }

    // solving algorithm TODO (tree-based: recursively)
    void CalcSolution(List<GameObject> notTried) // called on scene start TODO question: are pieces copied and appear multiple times in scene? / destroyed after leaving an iteration
    {
        // 1 base case: check if won: if all pieces placed succesfully
        if (notTried.Count == 0 && GameManager.CheckWon())
        {
            won = true;
            GameManager.userNotAlgo = true;
            return;
        }

        // if not won: goes back one iterative call to 

        // 2 try each piece
        for (int i = notTried.Count; i > 0; i--) // TODO not tree based -> doesn't go back just one -> brute-force algo
        {
            // 3 select random piece of available ones
            int rdm = Random.Range(0, i);
            GameManager.selectedPiece = notTried[rdm].GetComponent<Piece>();
            bool success = false;

            int x = 0, y = 0, z = 0;
            for (y = 0; y < 5 - z - x; y++) // 4 iterate through each grid position 
            {
                z = 0;
                for (z = 0; z < 5 - y - x; z++)
                {
                    x = 0;
                    for (x = 0; x < 5 - z - y; x++)
                    {
                        // 5 iterate through each possible rotation/ orientation
                        for (int j = 0; j < 4; j++) // rotate in z
                        {
                            for (int k = 0; k < 6; k++) // rotate in y
                            {
                                for (int l = 0; l < 4; l++) // rotate in x
                                {
                                    // 6 try placing it
                                    success = GameManager.Place();
                                    if (success)
                                    {
                                        // 7 if successful: remove from notTried list (in a separate copy)
                                        List<GameObject> tmp = notTried.ToList();
                                        tmp.RemoveAt(rdm);

                                        // 8 recursive call
                                        CalcSolution(tmp);

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
                }
                if (success)
                {
                    break;
                }

                // 4 
                GameManager.MovePiece('y', 1);
            }

            // TODO if unsuccessful: remove last placed piece from grid again (for tree)
                // when going back in the tree -> remove last piece from grid again and try placing the another first

            Debug.Log("No positioning found");

        }

        Debug.Log("Outer iteration reached");
    }

    // TODO try recursive
}

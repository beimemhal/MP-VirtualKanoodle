using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionManager : MonoBehaviour
{
    bool won = false; // TODO
    List<GameObject> availablePieces = new(GameManager.allPieces); // copy of allPieces

    int difficulty; // TODO 1 to 5 ?

    // solution storage in GameManager? (2 dictionaries vars) TODO

    // placing pre-set pieces according to difficulty fct TODO
    void LevelSetup()
    {
        // set pieces.moveable = false;
    }

    // solving algorithm TODO (tree-based)
    void CalcSolution() // called on scene start
    {
        // TODO activate loading screen (if new level in between? -> maybe go back to main menu for that)

        while (!won)
        {
            for (int i = 0; i < availablePieces.Count; i++)
            {
                // TODO 1 select random piece of available ones in a loop where it's possible to go back
                Piece rdmPiece;
                bool success = false;

                int x = 0, y = 0, z = 0;
                for (y = 0; y < 5 - z - x; y++) // iterate through each grid position 
                {
                    z = 0;
                    for (z = 0; z < 5 - y - x; z++)
                    {
                        x = 0;
                        for (x = 0; x < 5 - z - y; x++)
                        {
                            // iterate through each possible rotation/ orientation
                            for (int j = 0; j < 6; j++) // rotate in x
                            {
                                for (int k = 0; k < 6; k++) // rotate in y
                                {
                                    for (int l = 0; l < 6; l++) // rotate in z
                                    {
                                        // try placing it, if possible, success = true; break; TODO
                                        // if not, continue;
                                    }
                                    if (success)
                                    {
                                        break;
                                    }
                                }
                                if (success)
                                {
                                    break;
                                }
                            }
                            if (success)
                            {
                                break;
                            }
                        }
                        if (success)
                        {
                            break;
                        }
                    }
                    if (success)
                    {
                        break;
                    }
                }
                // TODO check if availablePieces = empty, if yes won = true;
            }
            if (!won)
                // error 
                Debug.Log("Error while calculating solution! None found.");
        }


            //  make copy of available in each loop and delete directly -> can't choose the same piece twice if going back on step 

            // try to place if ! -> continue;
            // if placeable: remove from List & check if list empty -> won

            // when going back in the tree -> remove last piece from grid again and try placing the other first

            // outermost loop: if no available pieces and !won: error "no solution found"

            // TODO de-activate loading screen

        }
    }

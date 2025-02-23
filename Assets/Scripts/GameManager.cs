using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool won = false; // TODO check function

    GameObject selected;
    // if a piece is selected, calculate how far the furthest sphere is away from sphere 1
    int x_min = 0, x_max = 0, y_min = 0, y_max = 0, z_min = 0, z_max = 0;

    public static List<GameObject> allPieces = new List<GameObject>(); // how to create and put pieces in there effectively?
    List<GameObject> buttons = new List<GameObject>();

    public static void TurnGridY(int i) // TODO
    {
        // don't turn grids ghost spheres, only its attached pieces -> maybe not necessary
        // y + i selected
    }

    public static void MovePieceY(int dir) // x, y, z also as variable (1, 2, 3 and then if
    {
        // check if outside grid
        if (OutsideGrid('y')) { 

            return; 
        }
        // y + () * i TODO
    }

    public static void MovePieceX(int dir)
    {
        // x + () * i TODO
    }

    public static void MovePieceZ(int dir)
    {
        // z + () * i TODO
    }

    public static void TurnPieceY(int i)
    {
        // y + i TODO
    }

    public static void TurnPieceX(int i)
    {
        // y + i TODO
    }

    // TODO turn in z necessary ?

    void Start() // TODO
    {
        // initialise the 12 pieces list -> find
        InitPieces();
        // initialise buttons list
        InitButtons();
    }

    void Update() // TODO
    {
        // if selected != null
        // check for movement
    }

    // restart funct here ? TODO

    // help functions for Start
    void InitPieces()
    {
        // TODO
    }

    void InitButtons()
    {
        // TODO
    }

    public void DisableButtons()
    {
        if (selected == null)
        {
            foreach (GameObject g in buttons) g.SetActive(false);
        }
        // TODO
    }

    static bool OutsideGrid(char dir) // at least one of the pieces spheres has to remain on a grid position
    {
        // if ( == 1) disable Minus Buttons TODO
        // return false; 
    }

    void PieceSelected()
    {
        // search from low to up for free grid position and put piece's sphere 1 there TODO
    }
}

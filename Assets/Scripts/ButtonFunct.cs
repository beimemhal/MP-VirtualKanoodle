using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunct : MonoBehaviour
{
    // turn grid
    public void TurnGridRight() // called by button
    {
        GameManager.TurnGridY(90); // or turn 60 or freehand? -> both too hard
    }

    public void TurnGridLeft() // called by button
    {
        GameManager.TurnGridY(-90); // or turn -60 or freehand? -> both too hard
    }

    // move pieces
    public void MovePieceXPlus()
    {
        GameManager.MovePieceX(1);
    }

    public void MovePieceXMinus()
    {
        GameManager.MovePieceX(-1);
    }

    public void MovePieceYPlus()
    {
        GameManager.MovePieceY(1);
    }

    public void MovePieceYMinus()
    {
        GameManager.MovePieceY(-1);
    }

    public void MovePieceZPlus()
    {
        GameManager.MovePieceZ(1);
    }
    public void MovePieceZMinus()
    {
        GameManager.MovePieceZ(-1);
    }

    // turn pieces
    public void TurnPieceYPlus() 
    {
        GameManager.TurnPieceY(60);
    }

    public void TurnPieceYMinus() 
    {
        GameManager.TurnPieceY(-60);
    }

    public void TurnPieceXPlus() 
    {
        GameManager.TurnPieceX(-60);
    }

    public void TurnPieceXMinus() 
    {
        GameManager.TurnPieceX(-60);
    }

    public void Place() 
    {
        // attach piece to grid
        // disable each ghost sphere that overlaps with a pieces sphere
    }

    public void Remove() 
    { 

    }

    public void Restart() // in GameManager?
    { 
        // put pieces at initial positions (after detaching them from the grid)
        // reset grid spheres
    }

    public void BackToMainMenu() // in GameManager?
    { 
        // TODO
    }
}

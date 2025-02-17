using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFunct : MonoBehaviour
{
    // all need prefab instance = selected GameObject in GameManager
    void TurnRight() { // rotate in y + 60
                       
    }

    void TurnLeft() { 
        // 5 times TurnRight(); 
    }

    // void Flip() { // not nececarry cause turnable in all axes -> turn 3 times = flipped
        // rotate in x + 180
        // }

    void TurnXPlus() {
        // rotate x + 60
    }

    void TurnXMinus() { 
        // 5 times TurnXPlus(); 
    }

    void Place() {
    }

    void Remove() { 
    }

    void Restart() { // in GameManager?
    }

    void BackToMainMenu() { // in GameManager?
    }
}

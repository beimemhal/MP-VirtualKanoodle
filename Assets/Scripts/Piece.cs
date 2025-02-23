using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sphere1 is always the reference for positioning TODO true?
public class Piece : MonoBehaviour
{
    public GameObject prefab;
    bool placed = false;
    public Vector3 initialPosition; // or just work with Transform.Position
    bool moveable = true; // if piece gets set in the level setup, it cannot be moved

    // position in grid if placed (x, y, z for each sphere?) TODO

    // positions of the spheres: FindAllChildren<3D Object: Sphere> TODO

    // store initial position in initialPosition variable
    void Start()
    {
        initialPosition = prefab.transform.position;
        GameManager.allPieces.Add(gameObject);
    }

    // if clicked on, becomes selected piece in GameManager 
    private void Update()
    {
        // TODO
        // check if selected != null : put selected to initial position and reset orientation
        // enable buttons
    }

    // if selected or placed: attach to grid so that it moves together
}

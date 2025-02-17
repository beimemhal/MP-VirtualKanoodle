using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFunct : MonoBehaviour 
{
    // Grid variable? and script attached to empty object?
    List<GameObject> gridPoints = new List<GameObject>();

    // if a piece is placed, make it child obj -> so that it moves together with grid?

    void Start()
    {
        // gridPoints = find all children (spheres)
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // when a piece is placed on the grid, the ghost sphere visualising the grid point is disabled -> no need for extra methods
    void DisableGhostSphere() { 
    } // recieves reference to sphere (position of the grid)

    // re-enables ghost sphere on, when a piece is removed from the grid
    void EnableGhostSphere() { 
    } // recieves reference to sphere (position of the grid)

}

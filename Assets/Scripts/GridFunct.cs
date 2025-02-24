using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFunct : MonoBehaviour 
{
    // Grid variable? and script attached to gridObj
    List<GameObject> gridPoints = new List<GameObject>();

    static float gridHeightY = 0.8164966F, gridHeightZ = 0.8660254F; // gridWidthX = 1

    // if a piece is placed, make it child obj -> so that it moves together with grid or joint TODO

    void Start()
    {
        // gridPoints = find all children (spheres) TODO
    }

    // when a piece is placed on the grid, the ghost sphere visualising the grid point is disabled -> no need for extra methods
    void DisableGhostSphere() // recieves reference to sphere (position of the grid)
    { 
        // TODO
    } 

    // re-enables ghost sphere on, when a piece is removed from the grid
    void EnableGhostSphere() // recieves reference to sphere (position of the grid)
    {
        // TODO

    }

    // see Unitys class Grid for useful functions TODO

    // CalcGlobalToLocalSpace TODO

    public static Vector3 CalcGridToGlobalSpace(Vector3Int gridPos)
    {
        float x = (float) gridPos.x;
        float y = (float) gridPos.y;
        float z = (float) gridPos.z;
        
        return new Vector3(x - 8F + 0.5F*(y + z), gridHeightY*y + 0.5F, gridHeightZ*(z + y/2F));
    }
}

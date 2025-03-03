using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFunct : MonoBehaviour 
{
    // Grid variable? and script attached to gridObj
    List<GameObject> gridPoints = new List<GameObject>();

    static float gridHeightY = 0.8164966F, gridHeightZ = 0.8660254F; // gridWidthX = 1

    void Start()
    {
        // gridPoints = find all children (spheres) TODO
    }

    // see Unitys class Grid for useful functions TODO

    // CalcGlobalToLocalSpace TODO

    public static Vector3 CalcGridToGlobalSpace(Vector3Int gridPos) // still working if grid gets turned? TODO
    {
        float x = (float) gridPos.x;
        float y = (float) gridPos.y;
        float z = (float) gridPos.z;
        
        return new Vector3(x - 8F + 0.5F*(y + z), gridHeightY*y + 0.5F, gridHeightZ*(z + y/3F));
    }
}

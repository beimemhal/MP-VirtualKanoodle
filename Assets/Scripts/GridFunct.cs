using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridFunct 
{
    // Grid variable? and script attached to gridObj TODO delete
    public static SphereCollider[] gridPoints = GameManager.gridParent.transform.GetComponentsInChildren<SphereCollider>();

    static float gridHeightY = 0.8164966F, gridHeightZ = 0.8660254F; // gridWidthX = 1

    // see Unitys class Grid for useful functions TODO delete

    // CalcGlobalToLocalSpace TODO delete

    public static Vector3 CalcGridToGlobalSpace(Vector3Int gridPos)
    {
        float x = (float) gridPos.x;
        float y = (float) gridPos.y;
        float z = (float) gridPos.z;
        
        return new Vector3(x - 8F + 0.5F*(y + z), gridHeightY*y + 0.5F, gridHeightZ*(z + y/3F));
    }
}

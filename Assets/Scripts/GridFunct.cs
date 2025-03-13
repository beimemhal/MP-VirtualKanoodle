using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridFunct 
{
    public static SphereCollider[] gridPoints = GameManager.gridParent.transform.GetComponentsInChildren<SphereCollider>();

    static float gridHeightY = 0.8164966F, gridHeightZ = 0.8660254F; // gridWidthX = 1

    public static Vector3 CalcGridToGlobalSpace(Vector3Int gridPos)
    {
        float x = (float) gridPos.x;
        float y = (float) gridPos.y;
        float z = (float) gridPos.z;
        
        return new Vector3(x - 8F + 0.5F*(y + z), gridHeightY*y + 0.5F, gridHeightZ*(z + y/3F));
    }

    public static bool CheckWon()
    {
        // Debug.Log("Check won called on " + gridPoints);

        // if all grid spheres are disabled = solution found
        foreach (SphereCollider sphere in gridPoints)
            if (sphere.gameObject.activeSelf)
                return false;

        // Debug.Log("Game won");

        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        
        return new Vector3(x + 0.5F*(y + z) + GameManager.gridParent.transform.position.x, 
            gridHeightY*y + GameManager.gridParent.transform.position.y, 
            gridHeightZ*(z + y/3F) + GameManager.gridParent.transform.position.z);
    }

    // calculate gridSpace to array index TODO correct?
    public static int CalcGridSpaceToArrayIndex(Vector3Int gridPos)
    {
        int index = 0;

        for (int y = 0; y < gridPos.y; y++)
        {
            for (int z = 0; z < 6 - y; z++)
            {
                index += 6 - y - z; // = xMax
            }
        }

        for (int z = 0; z < gridPos.z; z++)
        {
            index += 6 - gridPos.y - z;
        }

        index += gridPos.x;

        return index;
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

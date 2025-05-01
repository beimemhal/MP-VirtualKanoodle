using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GridFunct : ScriptableObject
{
    static readonly float gridHeightY = 0.8164966F, gridHeightZ = 0.8660254F; // gridWidthX = 1

    public static int gridTurns = 0;

    static Vector3 gridRoot = new Vector3(-8F, 0.5F, 0F);
    
    public static Vector3 CalcGridToGlobalSpace(Vector3Int gridPos)
    {
        float x = (float) gridPos.x;
        float y = (float) gridPos.y;
        float z = (float) gridPos.z;

        return new Vector3(x + 0.5F * (y + z) + gridRoot.x, // GameManager.gridParent.transform.position.x, 
            gridHeightY * y + gridRoot.y, // GameManager.gridParent.transform.position.y, 
            gridHeightZ * (z + y / 3F) + gridRoot.z); // GameManager.gridParent.transform.position.z);
    }

    // calculate gridSpace to array index of gridPoints in GameManager
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
        // if all grid spheres are disabled = solution found
        foreach (SphereCollider sphere in GameManager.gridPoints)
            if (sphere.gameObject.activeSelf)
                return false;

        // Debug.Log("Game won");

        // HardCodedLvl.SaveSolutions(0); // used for implementation of hard-coded levels

        return true;
    }

    // piece sphere 1 has to remain on a grid position
    public static bool OutsideGrid(Vector3Int newPos)
    {
        if (newPos.x < 0 || newPos.x > 5 - newPos.y - newPos.z ||
            newPos.y < 0 || newPos.y > 5 - newPos.x - newPos.z ||
            newPos.z < 0 || newPos.z > 5 - newPos.y - newPos.x)
            return true;
        return false;
    }

    // to make solver quicker; true if a point isolated/ no available grid points neighbouring, false if not
    public static bool CheckIsolatedPoints()
    {
        foreach (SphereCollider coll in GameManager.gridPoints)
        {
            GameObject g = coll.gameObject;
            int counter = 12; // = number of neighboring spheres

            if (g.activeSelf)
            {
                Vector3Int gridPos = new(int.Parse(g.name[8].ToString()) - 1, int.Parse(g.name[6].ToString()) - 1, int.Parse(g.name[7].ToString()) - 1); // extracts grid coordinates out of grid points names

                // check each neighbor in the grid  
                // six on same level/ y
                gridPos.x--;
                counter = CheckActiveNeighbor(counter, gridPos);
                gridPos.z++;
                counter = CheckActiveNeighbor(counter, gridPos);
                gridPos.x++;
                counter = CheckActiveNeighbor(counter, gridPos);
                gridPos.z -= 2;
                counter = CheckActiveNeighbor(counter, gridPos);
                gridPos.x++;
                counter = CheckActiveNeighbor(counter, gridPos);
                gridPos.z++;
                counter = CheckActiveNeighbor(counter, gridPos);

                // three below
                gridPos.y--;
                counter = CheckActiveNeighbor(counter, gridPos);
                gridPos.x--;
                counter = CheckActiveNeighbor(counter, gridPos);
                gridPos.z++;
                counter = CheckActiveNeighbor(counter, gridPos);

                // three above
                gridPos.y += 2;
                gridPos.z--;
                counter = CheckActiveNeighbor(counter, gridPos);
                gridPos.z--;
                counter = CheckActiveNeighbor(counter, gridPos);
                gridPos.z++;
                gridPos.x--;
                counter = CheckActiveNeighbor(counter, gridPos);
            }

            if (counter == 0) // no neighbours active
                return true;
        }

        return false; // no grid points isolated
    }

    // counts down by one if given position is outside of the grid or occupied by a piece
    static int CheckActiveNeighbor(int counter, Vector3Int gridPos)
    {
        if (!OutsideGrid(gridPos))
        {
            if (!GameManager.gridPoints[CalcGridSpaceToArrayIndex(gridPos)].gameObject.activeSelf)
                counter--;
        }
        else
            counter--; // neighbour doesn't exist/ outside border point

        return counter;
    }
}

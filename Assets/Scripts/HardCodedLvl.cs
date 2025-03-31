using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class HardCodedLvl : MonoBehaviour
{
    void Start()
    {
        
    }

    public static void SaveSolution()
    {
        foreach (GameObject p in GameManager.allPieces)
        {
            Debug.Log("SolutionManager.solutionPositions.Add(\"" + p.name + "\", new(" + p.transform.position.x + "F, " + p.transform.position.y + "F, " + p.transform.position.z  + "F));");
            Debug.Log("SolutionManager.solutionRotations.Add(\"" + p.name + "\", new(" + p.transform.rotation.x + "F, " + p.transform.rotation.y + "F, " + p.transform.rotation.z + "F, " + p.transform.rotation.w + "F));");


        }

    }

    void InitialisePieces()
    {
        // TODO
        SolutionManager.solutionRotations.Add("grey", new(-0.2886753F, 0.7071068F, -0.5F, 0.4082481F));
        SolutionManager.solutionPositions.Add("grey", new(-5.5F, 4.582483F, 1.443374F));
        SolutionManager.solutionRotations.Add("red", new(0F, -0.8660254F, 0F, 0.5F));
        SolutionManager.solutionPositions.Add("red", new(-3.000001F, 0.5F, -8.940697E-07F));
        SolutionManager.solutionRotations.Add("darkBlue", new(0F, -0.8660254F, 0F, 0.5F));
        SolutionManager.solutionPositions.Add("darkBlue", new(-4.500001F, 0.5F, 2.598075F));
        SolutionManager.solutionRotations.Add("orange", new(0F, -1F, 0F, 3.992753E-09F));
        SolutionManager.solutionPositions.Add("orange", new(-4.000001F, 1.316497F, 1.1547F));
        SolutionManager.solutionRotations.Add("beige", new(0.5F, -2.620836E-09F, -0.8660254F, 3.895218E-08F));
        SolutionManager.solutionPositions.Add("beige", new(-5.500001F, 0.5F, 2.598075F));
        SolutionManager.solutionRotations.Add("purple", new(-0.2886751F, 0.7071068F, -0.5000001F, 0.4082483F));
        SolutionManager.solutionPositions.Add("purple", new(-5.500001F, 3.765986F, 2.020725F));
        SolutionManager.solutionRotations.Add("lightBlue", new(0F, 2.980232E-08F, 0F, 1F));
        SolutionManager.solutionPositions.Add("lightBlue", new(-7.500001F, 1.316497F, 0.2886745F));
        SolutionManager.solutionRotations.Add("darkGreen", new(-0.5773503F, 2.43335E-08F, 1.720638E-08F, 0.8164966F));
        SolutionManager.solutionPositions.Add("darkGreen", new(-7.000001F, 2.132993F, 0.5773496F));
        SolutionManager.solutionRotations.Add("lightGreen", new(-1.441361E-07F, 0.2886752F, -0.8164966F, 0.5F));
        SolutionManager.solutionPositions.Add("lightGreen", new(-5.500001F, 2.132993F, 3.175426F));
        SolutionManager.solutionRotations.Add("white", new(0F, -0.8660254F, 0F, 0.5F));
        SolutionManager.solutionPositions.Add("white", new(-4.500001F, 1.316497F, 2.020725F));
        SolutionManager.solutionRotations.Add("pink", new(0F, 2.980232E-08F, 0F, 1F));
        SolutionManager.solutionPositions.Add("pink", new(-8.000001F, 0.5F, -5.960464E-07F));
        SolutionManager.solutionRotations.Add("yellow", new(0.8660253F, 1.720638E-08F, -0.5000002F, -2.980232E-08F));
        SolutionManager.solutionPositions.Add("yellow", new(-5F, 2.132993F, 2.309399F));







    }
}

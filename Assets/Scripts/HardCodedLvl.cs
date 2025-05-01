using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class HardCodedLvl : MonoBehaviour
{
    public static List<string> lastPlaced;

    void Start() // TODO disable/ delete if solver algo works
    {
        lastPlaced = new();

        // chose random solution 
        int rdm = Random.Range(1, 6);
        SaveSolutions(rdm);

        // initialise pre-set pieces according to difficulty
        InitialisePieces();

        // set user true
        GameManager.userNotAlgo = true;
    }

    public static void SaveSolutions(int rdm)
    {
        /*
        // output of solutions, used to output the lines for InitialisePieces()
        foreach (GameObject p in GameManager.allPieces)
        {
            Debug.Log("SolutionManager.solutionPositions.Add(\"" + p.name + "\", new(" + p.transform.position.x + "F, " + p.transform.position.y + "F, " + p.transform.position.z  + "F));");
            Debug.Log("SolutionManager.solutionRotations.Add(\"" + p.name + "\", new(" + p.transform.rotation.x + "F, " + p.transform.rotation.y + "F, " + p.transform.rotation.z + "F, " + p.transform.rotation.w + "F));");
        }
        */

        // solution 1
        if (rdm == 1)
        {
            SolutionManager.solutionRotations.Add("beige", new(0.5F, -2.620836E-09F, -0.8660254F, 3.895218E-08F));
            SolutionManager.solutionPositions.Add("beige", new(-5.5F, 0.5F, 2.598075F));
            lastPlaced.Add("beige");
            SolutionManager.solutionRotations.Add("pink", new(0F, 2.980232E-08F, 0F, 1F));
            SolutionManager.solutionPositions.Add("pink", new(-8F, 0.5F, -5.960464E-07F));
            lastPlaced.Add("pink");
            SolutionManager.solutionRotations.Add("darkBlue", new(0F, -0.8660254F, 0F, 0.5F));
            SolutionManager.solutionPositions.Add("darkBlue", new(-4.5F, 0.5F, 2.598075F));
            lastPlaced.Add("darkBlue");
            SolutionManager.solutionRotations.Add("red", new(0F, -0.8660254F, 0F, 0.5F));
            SolutionManager.solutionPositions.Add("red", new(-3F, 0.5F, -8.940697E-07F));
            lastPlaced.Add("red");
            SolutionManager.solutionRotations.Add("white", new(0F, -0.8660254F, 0F, 0.5F));
            SolutionManager.solutionPositions.Add("white", new(-4.5F, 1.316497F, 2.020725F));
            lastPlaced.Add("white");
            SolutionManager.solutionRotations.Add("orange", new(0F, -1F, 0F, 3.992753E-09F));
            SolutionManager.solutionPositions.Add("orange", new(-4F, 1.316497F, 1.1547F));
            lastPlaced.Add("orange");
            SolutionManager.solutionRotations.Add("lightBlue", new(0F, 2.980232E-08F, 0F, 1F));
            SolutionManager.solutionPositions.Add("lightBlue", new(-7.5F, 1.316497F, 0.2886745F));
            lastPlaced.Add("lightBlue");
            SolutionManager.solutionRotations.Add("lightGreen", new(-1.441361E-07F, 0.2886752F, -0.8164966F, 0.5F));
            SolutionManager.solutionPositions.Add("lightGreen", new(-5.5F, 2.132993F, 3.175426F));
            lastPlaced.Add("lightGreen");
            SolutionManager.solutionRotations.Add("yellow", new(0.8660253F, 1.720638E-08F, -0.5F, -2.980232E-08F));
            SolutionManager.solutionPositions.Add("yellow", new(-5F, 2.132993F, 2.309399F));
            lastPlaced.Add("yellow");
            SolutionManager.solutionRotations.Add("purple", new(-0.2886751F, 0.7071068F, -0.5F, 0.4082483F));
            SolutionManager.solutionPositions.Add("purple", new(-5.500001F, 3.765986F, 2.020725F));
            lastPlaced.Add("purple");
            SolutionManager.solutionRotations.Add("darkGreen", new(-0.5773503F, 2.43335E-08F, 1.720638E-08F, 0.8164966F));
            SolutionManager.solutionPositions.Add("darkGreen", new(-7F, 2.132993F, 0.5773496F));
            lastPlaced.Add("darkGreen");
            SolutionManager.solutionRotations.Add("grey", new(-0.2886753F, 0.7071068F, -0.5F, 0.4082481F));
            SolutionManager.solutionPositions.Add("grey", new(-5.5F, 4.582483F, 1.443374F));
            lastPlaced.Add("grey");
        }
        else if (rdm == 2)
        {
            // solution 2
            SolutionManager.solutionPositions.Add("darkGreen", new(-5.5F, 0.5F, 4.330127F));
            SolutionManager.solutionRotations.Add("darkGreen", new(-0.8660254F, -1.720638E-08F, 0.5F, 2.980232E-08F));
            lastPlaced.Add("darkGreen");
            SolutionManager.solutionPositions.Add("lightGreen", new(-5.5F, 0.5F, 0.8660254F));
            SolutionManager.solutionRotations.Add("lightGreen", new(0.5F, -2.980232E-08F, 0.8660254F, -1.720638E-08F));
            lastPlaced.Add("lightGreen");
            SolutionManager.solutionPositions.Add("red", new(-8F, 0.5F, 0F));
            SolutionManager.solutionRotations.Add("red", new(-0.8660254F, 1.720638E-08F, -0.5F, 2.980232E-08F));
            lastPlaced.Add("red");
            SolutionManager.solutionPositions.Add("white", new(-5F, 0.5F, 0F));
            SolutionManager.solutionRotations.Add("white", new(0F, 0F, 0F, 1F));
            lastPlaced.Add("white");
            SolutionManager.solutionPositions.Add("purple", new(-5.5F, 1.316497F, 3.752777F));
            SolutionManager.solutionRotations.Add("purple", new(-0.8660254F, -1.720638E-08F, 0.5F, 2.980232E-08F));
            lastPlaced.Add("purple");
            SolutionManager.solutionPositions.Add("beige", new(-7.5F, 1.316497F, 0.2886751F));
            SolutionManager.solutionRotations.Add("beige", new(-0.8660254F, -2.43335E-08F, -0.5F, -5.241674E-09F));
            lastPlaced.Add("beige");
            SolutionManager.solutionPositions.Add("pink", new(-6.5F, 1.316497F, 0.2886751F));
            SolutionManager.solutionRotations.Add("pink", new(0F, 0F, 0F, 1F));
            lastPlaced.Add("pink"); 
            SolutionManager.solutionPositions.Add("orange", new(-5.5F, 2.94949F, 2.598076F));
            SolutionManager.solutionRotations.Add("orange", new(0.5F, -0.8164966F, -0.2886752F, -1.720638E-08F));
            lastPlaced.Add("orange");
            SolutionManager.solutionPositions.Add("darkBlue", new(-4.5F, 2.132993F, 1.443376F));
            SolutionManager.solutionRotations.Add("darkBlue", new(-0.2886751F, -1.416194E-07F, 0.5F, -0.8164967F));
            lastPlaced.Add("darkBlue");
            SolutionManager.solutionPositions.Add("yellow", new(-6F, 2.132993F, 0.5773503F));
            SolutionManager.solutionRotations.Add("yellow", new(0F, 0F, 0F, 1F));
            lastPlaced.Add("yellow");
            SolutionManager.solutionPositions.Add("grey", new(-4.5F, 2.94949F, 0.8660254F));
            SolutionManager.solutionRotations.Add("grey", new(0F, -0.8660254F, 0F, 0.5F));
            lastPlaced.Add("grey");
            SolutionManager.solutionPositions.Add("lightBlue", new(-5.5F, 4.582483F, 1.443376F));
            SolutionManager.solutionRotations.Add("lightBlue", new(-0.2886752F, 0.7071068F, -0.5F, 0.4082482F));
            lastPlaced.Add("lightBlue");
        }
        else if (rdm == 3)
        {
            // solution 3
            SolutionManager.solutionPositions.Add("grey", new(-5.5F, 0.5F, 2.598076F));
            SolutionManager.solutionRotations.Add("grey", new(0F, -0.5F, 0F, 0.8660254F));
            lastPlaced.Add("grey");
            SolutionManager.solutionPositions.Add("lightGreen", new(-6F, 0.5F, 1.732051F)); 
            SolutionManager.solutionRotations.Add("lightGreen", new(0F, -0.8660254F, 0F, -0.5F));
            lastPlaced.Add("lightGreen");
            SolutionManager.solutionPositions.Add("yellow", new(-5.5F, 0.5F, 0.8660254F));
            SolutionManager.solutionRotations.Add("yellow", new(-0.8660254F, -2.43335E-08F, -0.5F, -5.241674E-09F));
            lastPlaced.Add("yellow");
            SolutionManager.solutionPositions.Add("darkGreen", new(-5.5F, 1.316497F, 3.752777F));
            SolutionManager.solutionRotations.Add("darkGreen", new(-0.8660254F, -1.720638E-08F, 0.5F, 2.980232E-08F));
            lastPlaced.Add("darkGreen");
            SolutionManager.solutionPositions.Add("darkBlue", new(-7F, 0.5F, 1.732051F));
            SolutionManager.solutionRotations.Add("darkBlue", new(-0.2886752F, 0.7071068F, 0.5F, 0.4082483F));
            lastPlaced.Add("darkBlue");
            SolutionManager.solutionPositions.Add("purple", new(-3F, 0.5F, 0F));
            SolutionManager.solutionRotations.Add("purple", new(1.720638E-08F, -0.8660254F, 6.312391E-10F, 0.5F));
            lastPlaced.Add("purple");
            SolutionManager.solutionPositions.Add("beige", new(-7.5F, 1.316497F, 0.2886751F));
            SolutionManager.solutionRotations.Add("beige", new(-0.8660254F, -2.43335E-08F, -0.5F, -5.241674E-09F));
            lastPlaced.Add("beige");
            SolutionManager.solutionPositions.Add("orange", new(-4.5F, 1.316497F, 0.2886751F));
            SolutionManager.solutionRotations.Add("orange", new(2.580957E-08F, -4.866698E-08F, -1F, -1.294175E-15F));
            lastPlaced.Add("orange");
            SolutionManager.solutionPositions.Add("white", new(-5.5F, 3.765986F, 2.020726F));
            SolutionManager.solutionRotations.Add("white", new(-4.153987E-08F, -0.2886751F, 0.8164966F, -0.5F));
            lastPlaced.Add("white");
            SolutionManager.solutionPositions.Add("red", new(-6F, 2.132993F, 0.5773503F));
            SolutionManager.solutionRotations.Add("red", new(-0.8660254F, 1.720638E-08F, -0.5F, 2.980232E-08F));
            lastPlaced.Add("red");
            SolutionManager.solutionPositions.Add("lightBlue", new(-5F, 3.765986F, 1.154701F)); 
            SolutionManager.solutionRotations.Add("lightBlue", new(-0.2886751F, -3.442493E-08F, 0.5F, -0.8164966F));
            lastPlaced.Add("lightBlue");
            SolutionManager.solutionPositions.Add("pink", new(-5.5F, 4.582483F, 1.443376F));
            SolutionManager.solutionRotations.Add("pink", new(0.2886751F, -0.7071068F, 0.5F, -0.4082483F));
            lastPlaced.Add("pink");
        }
        else if (rdm == 4)
        {
            // solution 4
            SolutionManager.solutionPositions.Add("beige", new(-5.5F, 0.5F, 4.330127F));
            SolutionManager.solutionRotations.Add("beige", new(-0.8660254F, -1.720638E-08F, 0.5F, 2.980232E-08F));
            lastPlaced.Add("beige");
            SolutionManager.solutionPositions.Add("lightGreen", new(-6.5F, 0.5F, 2.598076F));
            SolutionManager.solutionRotations.Add("lightGreen", new(-0.8660254F, -1.720638E-08F, 0.5F, 2.980232E-08F));
            lastPlaced.Add("lightGreen");
            SolutionManager.solutionPositions.Add("orange", new(-5F, 0.5F, 0F));
            SolutionManager.solutionRotations.Add("orange", new(2.580957E-08F, -3.441276E-08F, 1F, -1.185091E-15F));
            lastPlaced.Add("orange");
            SolutionManager.solutionPositions.Add("lightBlue", new(-3F, 0.5F, 0F));
            SolutionManager.solutionRotations.Add("lightBlue", new(1.720638E-08F, -0.8660254F, 6.312391E-10F, 0.5F));
            lastPlaced.Add("lightBlue");
            SolutionManager.solutionPositions.Add("darkBlue", new(-7F, 0.5F, 1.732051F));
            SolutionManager.solutionRotations.Add("darkBlue", new(-0.8164967F, -0.5F, 1.007926E-08F, 0.2886752F));
            lastPlaced.Add("darkBlue");
            SolutionManager.solutionPositions.Add("yellow", new(-6F, 1.316497F, 2.886751F));
            SolutionManager.solutionRotations.Add("yellow", new(0F, 0.5F, 0F, 0.8660254F));
            lastPlaced.Add("yellow");
            SolutionManager.solutionPositions.Add("grey", new(-7.5F, 1.316497F, 0.2886751F));
            SolutionManager.solutionRotations.Add("grey", new(0F, 0F, 0F, 1F));
            lastPlaced.Add("grey");
            SolutionManager.solutionPositions.Add("white", new(-3.5F, 1.316497F, 0.2886751F));
            SolutionManager.solutionRotations.Add("white", new(2.580957E-08F, -3.441276E-08F, 1F, -1.185091E-15F));
            lastPlaced.Add("white");
            SolutionManager.solutionPositions.Add("darkGreen", new(-5.5F, 2.132993F, 3.175426F));
            SolutionManager.solutionRotations.Add("darkGreen", new(-0.7071067F, 0.288675F, 0.4082484F, 0.5F));
            lastPlaced.Add("darkGreen");
            SolutionManager.solutionPositions.Add("purple", new(-5.5F, 3.765986F, 2.020726F));
            SolutionManager.solutionRotations.Add("purple", new(0.2886752F, -0.7071068F, 0.5F, -0.4082483F));
            lastPlaced.Add("purple");
            SolutionManager.solutionPositions.Add("pink", new(-7F, 2.132993F, 0.5773503F));
            SolutionManager.solutionRotations.Add("pink", new(0F, 0F, 0F, 1F));
            lastPlaced.Add("pink");
            SolutionManager.solutionPositions.Add("red", new(-5.5F, 4.582483F, 1.443376F));
            SolutionManager.solutionRotations.Add("red", new(0.2886751F, -0.7071068F, 0.5F, -0.4082483F));
            lastPlaced.Add("red");
        }
        else if (rdm == 5)
        {
            // solution 5
            SolutionManager.solutionPositions.Add("pink", new(-5.5F, 0.5F, 4.330127F));
            SolutionManager.solutionRotations.Add("pink", new(-0.8660254F, -1.720638E-08F, 0.5F, 2.980232E-08F));
            lastPlaced.Add("pink");
            SolutionManager.solutionPositions.Add("lightGreen", new(-5.5F, 2.132993F, 3.175426F));
            SolutionManager.solutionRotations.Add("lightGreen", new(-1.323798E-08F, 0.2886751F, -0.8164966F, 0.5F));
            lastPlaced.Add("lightGreen");
            SolutionManager.solutionPositions.Add("darkBlue", new(-7F, 0.5F, 1.732051F));
            SolutionManager.solutionRotations.Add("darkBlue", new(0F, -0.8660254F, 0F, -0.5F));
            lastPlaced.Add("darkBlue");
            SolutionManager.solutionPositions.Add("orange", new(-6.5F, 0.5F, 0.8660254F));
            SolutionManager.solutionRotations.Add("orange", new(1F, 0F, 0F, 4.866698E-08F));
            lastPlaced.Add("orange");
            SolutionManager.solutionPositions.Add("lightBlue", new(-3F, 0.5F, 0F));
            SolutionManager.solutionRotations.Add("lightBlue", new(2.580957E-08F, -4.866698E-08F, -1F, -1.294175E-15F));
            lastPlaced.Add("lightBlue");
            SolutionManager.solutionPositions.Add("yellow", new(-4.5F, 1.316497F, 2.020726F));
            SolutionManager.solutionRotations.Add("yellow", new(1.720638E-08F, -0.8660254F, 6.312391E-10F, 0.5F));
            lastPlaced.Add("yellow");
            SolutionManager.solutionPositions.Add("grey", new(-6.5F, 1.316497F, 0.2886751F));
            SolutionManager.solutionRotations.Add("grey", new(0F, -0.5F, 0F, 0.8660254F));
            lastPlaced.Add("grey");
            SolutionManager.solutionPositions.Add("purple", new(-3.5F, 1.316497F, 0.2886751F));
            SolutionManager.solutionRotations.Add("purple", new(2.580957E-08F, -4.866698E-08F, -1F, -1.294175E-15F));
            lastPlaced.Add("purple");
            SolutionManager.solutionPositions.Add("darkGreen", new(-4F, 2.132993F, 0.5773503F));
            SolutionManager.solutionRotations.Add("darkGreen", new(0F, -0.8660254F, 0F, 0.5F));
            lastPlaced.Add("darkGreen");
            SolutionManager.solutionPositions.Add("red", new(-5.5F, 3.765986F, 2.020726F));
            SolutionManager.solutionRotations.Add("red", new(-0.2886752F, 0.7071068F, -0.5F, 0.4082483F));
            lastPlaced.Add("red");
            SolutionManager.solutionPositions.Add("beige", new(-7.5F, 1.316497F, 0.2886751F));
            SolutionManager.solutionRotations.Add("beige", new(-0.7071068F, -0.2886752F, -0.4082483F, -0.5F));
            lastPlaced.Add("beige");
            SolutionManager.solutionPositions.Add("white", new(-5.5F, 4.582483F, 1.443376F));
            SolutionManager.solutionRotations.Add("white", new(-0.7071068F, 0.2886751F, 0.4082483F, -0.5F));
            lastPlaced.Add("white");
        } 
        else if (rdm == 6)
        {
            // solution 6
            SolutionManager.solutionPositions.Add("beige", new(-5F, 0.5F, 1.732052F));
            SolutionManager.solutionRotations.Add("beige", new(-0.5F, -3.504402E-08F, -0.8660261F, 1.720633E-08F));
            lastPlaced.Add("beige");
            SolutionManager.solutionPositions.Add("lightGreen", new(-6.5F, 0.5F, 2.598073F));
            SolutionManager.solutionRotations.Add("lightGreen", new(-0.8660259F, 4.676435E-08F, 0.5F, -8.982987E-10F));
            lastPlaced.Add("lightGreen");
            SolutionManager.solutionPositions.Add("darkBlue", new(-7F, 0.5F, 1.732046F));
            SolutionManager.solutionRotations.Add("darkBlue", new(0.8164966F, 0.5F, 8.620694E-07F, -0.2886747F));
            lastPlaced.Add("darkBlue");
            SolutionManager.solutionPositions.Add("lightBlue", new(-3F, 0.5F, 3.516674E-06F));
            SolutionManager.solutionRotations.Add("lightBlue", new(2.901498E-08F, -0.8660259F, -5.273016E-09F, 0.5F));
            lastPlaced.Add("lightBlue");
            SolutionManager.solutionPositions.Add("orange", new(-5F, 0.5F, -4.172325E-07F));
            SolutionManager.solutionRotations.Add("orange", new(-9.499682E-07F, 0F, 1F, 0F));
            lastPlaced.Add("orange");
            SolutionManager.solutionPositions.Add("yellow", new(-5F, 1.316497F, 2.886751F));
            SolutionManager.solutionRotations.Add("yellow", new(0.5F, -4.094827E-08F, -0.866025F, -2.260422E-08F));
            lastPlaced.Add("yellow");
            SolutionManager.solutionPositions.Add("purple", new(-3.5F, 1.316497F, 0.2886777F));
            SolutionManager.solutionRotations.Add("purple", new(2.901498E-08F, -0.8660259F, -5.273016E-09F, 0.5F));
            lastPlaced.Add("purple");
            SolutionManager.solutionPositions.Add("darkGreen", new(-7.5F, 1.316497F, 0.2886698F));
            SolutionManager.solutionRotations.Add("darkGreen", new(0F, -9.834766E-07F, 0F, 1F));
            lastPlaced.Add("darkGreen");
            SolutionManager.solutionPositions.Add("pink", new(-4F, 2.132993F, 0.5773518F));
            SolutionManager.solutionRotations.Add("pink", new(-0.2886747F, -0.7071072F, -0.5F, 0.4082476F));
            lastPlaced.Add("pink");
            SolutionManager.solutionPositions.Add("red", new(-7F, 2.132993F, 0.5773459F));
            SolutionManager.solutionRotations.Add("red", new(0F, -9.834766E-07F, 0F, 1F));
            lastPlaced.Add("red");
            SolutionManager.solutionPositions.Add("white", new(-5.5F, 2.94949F, 2.59807F));
            SolutionManager.solutionRotations.Add("white", new(0.7071069F, 0.5773503F, -0.4082482F, 7.971101E-08F));
            lastPlaced.Add("white");
            SolutionManager.solutionPositions.Add("grey", new(-6F, 3.765986F, 1.154696F));
            SolutionManager.solutionRotations.Add("grey", new(-0.8164966F, 2.90122E-07F, -3.76867E-07F, -0.5773503F));
            lastPlaced.Add("grey");
        }
    }

    void InitialisePieces()
    {
        // Debug.Log("pieces initialised.");

        // put difficulty first pieces in solution dict on the grid (select and place)
        for (int i = 0; i < SolutionManager.difficulty; i++)
        {
            Piece p = GameObject.Find(lastPlaced[i]).GetComponent<Piece>();
            p.PieceSelected();

            // put in position saved in solution
            p.gameObject.transform.SetPositionAndRotation(SolutionManager.solutionPositions[lastPlaced[i]], SolutionManager.solutionRotations[lastPlaced[i]]);
            GameManager.gameManager.Place();

            // set not moveable
            p.moveable = false;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool won = false;

    GameObject selected;

    List<Piece> allPieces = new List<Piece>(); // how to create and put pieces in there effectively?

    // void Restart() { } & BackToMainMenu() { } here?

    // Start is called before the first frame update
    void Start()
    {
        // initialise the 12 pieces by hand? -> needs prefabs and positions
        // initialise grid prefab
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

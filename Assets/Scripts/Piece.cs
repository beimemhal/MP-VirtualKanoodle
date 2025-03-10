using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject prefab;

    public bool placed = false;
    public bool moveable = true; // if piece gets set in the level setup, it cannot be moved => cannot be selected

    public Vector3 initialPosition;

    // when piece is placed on the grid, saves the sphere which was used last to position the piece
    public Vector3Int gridPos = new Vector3Int(-1, -1, -1); // position in grid if placed or selected
    public Vector3Int rotationNrs = new Vector3Int(0, 0, 0); // saves amount of rotations for x & z direction for turning the piece

    public int sphereNr;
    public GameObject[] overlapsGridSpheres;

    // store initial position in initialPosition variable
    void Start()
    {
        initialPosition = prefab.transform.position;

        GameManager.allPieces.Add(gameObject);

        overlapsGridSpheres = new GameObject[sphereNr];
    }

    // if clicked on, becomes selected piece in GameManager 
    private void Update()
    {
        if (moveable)
        {
            // if mouse button down call one of the functions, source for raycast functionality: ChatGPT (slightly altered)
            if (Input.GetMouseButtonDown(0)) // Left-click
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Transform clickedObject = hit.transform;

                    // Check if the clicked object is the parent or one of its children
                    if (clickedObject.IsChildOf(transform))
                    {
                        PieceSelected();
                    }
                }
            }
        }
    }

    public void PieceSelected()
    {
        Debug.Log("Piece " + prefab.name + " selected."); // TODO later delete
        
        // check if piece that's being tried to be selected = moveable
        if (!moveable)
        {
            // TODO pop-up message that piece can't be selected
            Debug.Log("The " + name + " piece can't be removed because it is pre-set.");
            return;
        }

        // 1 if other piece is currently selected & != placed: reset it (to initial position) 
        if (GameManager.selectedPiece != null && !GameManager.selectedPiece.placed)
            GameManager.PieceUnselected();

        // better than below?: search from low to up for free grid position and put piece's sphere 1 there TODO later
        // 2 set selectedPiece in GameManager 
        if (!placed)
        {
            GameManager.selectedPiece = this;

            // 3 put at top of grid & attach to grid so that it moves together
            gridPos = new Vector3Int(0, 5, 0);
            gameObject.transform.position = GridFunct.CalcGridToGlobalSpace(gridPos);

            gameObject.transform.SetParent(GameManager.gridParent.transform);
        }
        else // 2
        {
            GameManager.selectedPiece = this;
        }

        // 4 dis- and enabling buttons
        int[] placeRemoveB = new int[6];
        // disable place or remove
        if (placed)
        {
            placeRemoveB[0] = 15; // placeB
        }
        else
            placeRemoveB[0] = 16; // removeB
        GameManager.DisOrEnableButtons(placeRemoveB, false);

        // enable place or remove
        if (placed) placeRemoveB[0] = 16; // removeB
        else
            placeRemoveB[0] = 15; // placeB
        GameManager.DisOrEnableButtons(placeRemoveB, true);

        if (!placed)
        {
            // movement buttons
            GameManager.DynamicButtonCheck(); 

            //  rotation buttons enabled
            for (int i = 0; i < placeRemoveB.Length; i++)
            {
                placeRemoveB[i] = i + 9;
            }
            GameManager.DisOrEnableButtons(placeRemoveB, true);
        }
        // 6 add outline script (TODO maybe set parameters)
        gameObject.AddComponent<Outline>();
    }

    public void CalcNewGridCoords(int negPos)
    {
        Debug.Log("Old coords: " + gridPos.x + ", " + gridPos.y + ", " + gridPos.z);

        if (negPos == 1)
        {
            int xOld = gridPos.x;
            gridPos.x = gridPos.z;
            gridPos.z = 5 - xOld - gridPos.y - gridPos.z;
        }
        else
        {
            int xOld = gridPos.x;
            gridPos.x = 5 - xOld - gridPos.y - gridPos.z;
            gridPos.z = xOld;
        }

        Debug.Log("New coords: " + gridPos.x + ", " + gridPos.y + ", " + gridPos.z);
    }

}

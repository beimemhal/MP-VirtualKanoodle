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
    public Vector3Int gridPos = new(-1, -1, -1); // position in grid if placed or selected
    public Vector3Int rotationNrs = new(0, 0, 0); // saves amount of rotations for x & z direction for turning the piece

    public int sphereNr;
    public GameObject[] overlapsGridSpheres;

    // store initial position at start of the scene
    void Start()
    {
        placed = false;
        moveable = true;

        initialPosition = prefab.transform.position;

        GameManager.allPieces.Add(gameObject);

        overlapsGridSpheres = new GameObject[sphereNr];
    }

    // if clicked on, becomes selected piece in GameManager 
    private void Update()
    {
        if (moveable && this != GameManager.selectedPiece) // cannot be selected if it already is
        {
            // if mouse button down call one of the functions, source for raycast functionality: ChatGPT (slightly altered)
            if (Input.GetMouseButtonDown(0)) // left-click
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Transform clickedObject = hit.transform;

                    // check if the clicked object is the parent or one of its children
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
        Debug.Log("Piece " + prefab.name + " selected.");
        
        // check if piece that's being tried to be selected = moveable
        if (!moveable)
        {
            // pop-up message that piece can't be selected
            if (!GameManager.userNotAlgo)
            {
                StartCoroutine(GameManager.gameManager.popUpCanvas.GetComponent<PopUpManager>().ShowNotification("Piece can't be selected because it's pre-set!"));
            }
            Debug.Log("The " + name + " piece can't be removed because it is pre-set.");
            return;
        }

        // 1 if other piece is currently selected & != placed: reset it (to initial position) 
        if (GameManager.selectedPiece != null && !GameManager.selectedPiece.placed)
            GameManager.PieceUnselected();
        else if (GameManager.selectedPiece != null && GameManager.selectedPiece.placed) // other piece selected which = placed: just unselect w/o reset
        {
            Destroy(GameManager.selectedPiece.gameObject.GetComponent<Outline>());
            GameManager.selectedPiece = null;
        }

        // 2 set selectedPiece in GameManager 
        if (!placed)
        {
            GameManager.selectedPiece = this;

            // 3 put at top of grid & attach to grid so that it moves together
            if (GameManager.userNotAlgo)
                gridPos = new Vector3Int(0, 5, 0);
            else
                gridPos = new Vector3Int(0, 0, 0); // solver algo tries placing the pieces from the bottom

            gameObject.transform.position = GridFunct.CalcGridToGlobalSpace(gridPos);

            gameObject.transform.SetParent(GameManager.gridParent.transform);
        }
        else // 2
        {
            GameManager.selectedPiece = this;
        }

        // buttons and outline unnecessary for solving algo (not doing it: time efficient)
        if (GameManager.userNotAlgo)
        {
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

            // 6 add outline script
            gameObject.AddComponent<Outline>();
        }
    }

    // when grid is rotated, piece turn with it -> grid zero is always considered lowest, front, left => pieces coordinates in grid change with changed position
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

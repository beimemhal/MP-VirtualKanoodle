using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sphere1 is always the reference for positioning (= 0,0,0 local space) TODO true?
public class Piece : MonoBehaviour
{
    public GameObject prefab;

    public bool placed = false; // TODO use
    bool moveable = true; // if piece gets set in the level setup, it cannot be moved => cannot be selected

    public Vector3 initialPosition; // or just work with Transform.Position

    // when piece is placed on the grid, saves the sphere which was used last to position the piece
    public GameObject referenceSphere;
    public Vector3Int gridPos = new Vector3Int(-1, -1, -1); // position in grid if placed or selected

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

        // 1 if other piece is currently selected: reset it (to initial position)
        if (GameManager.selectedPiece != null) 
            PieceUnselected();
        else // 2 enable movement buttons
            GameManager.DisOrEnableMovement(true);

        // 3 disable sphere colliders, so that piece can move freely in the grid
        SphereCollider[] components = gameObject.GetComponentsInChildren<SphereCollider>(true); // (true = also inactive objects) TODO later: source ChatGPT
        foreach (SphereCollider component in components)
        {
            component.enabled = false; // disable colliders
        }

        // better than below?: search from low to up for free grid position and put piece's sphere 1 there TODO later
        // 4 set selectedPiece in GameManager and put at top of grid
        GameManager.selectedPiece = this;
        gridPos = new Vector3Int(0, 5, 0);
        gameObject.transform.position = GridFunct.CalcGridToGlobalSpace(gridPos);

        // 5 attach to grid so that it moves together
        gameObject.transform.SetParent(GameManager.gridParent.transform);
    }

    // when piece is no longer selected but also not placed on grid => put back at initial position
    public void PieceUnselected()
    {
        // 1 & 2 put selected to initial position & reset orientation
        gameObject.transform.SetPositionAndRotation(initialPosition, Quaternion.identity);

        // 3 set sphereColliders active TODO later: necessary? (also in selected)
        SphereCollider[] components = gameObject.GetComponentsInChildren<SphereCollider>(true); // (true = also inactive objects)
        foreach (SphereCollider component in components)
        {
            component.enabled = true; // enable sphere collidors
        }

        // 4 selected = null
        GameManager.selectedPiece = null;

        // 5 detach from grid (no child of grid anymore) TODO
        gameObject.transform.SetParent(null);
    }
}

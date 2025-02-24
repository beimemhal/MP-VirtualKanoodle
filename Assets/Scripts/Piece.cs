using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sphere1 is always the reference for positioning (= 0,0,0 local space) TODO true?
public class Piece : MonoBehaviour
{
    public GameObject prefab;
    bool placed = false; // TODO use
    public Vector3 initialPosition; // or just work with Transform.Position
    bool moveable = true; // if piece gets set in the level setup, it cannot be moved => cannot be selected

    // when piece is placed on the grid, saves the sphere which was used last to position the piece
    public GameObject referenceSphere;
    public Vector3Int gridPos = new Vector3Int(-1, -1, -1);

    // position in grid if placed TODO

    // store initial position in initialPosition variable
    void Start()
    {
        initialPosition = prefab.transform.position;
        GameManager.allPieces.Add(gameObject);
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

    // if selected: attach to grid so that it moves together TODO (= 
    public void PieceSelected()
    {
        Debug.Log("Piece " + prefab.name + " selected.");
        // if other piece is currently selected: reset it (to initial position)
        if (GameManager.selectedPiece != null) PieceUnselected();

        // enable buttons TODO

        // search from low to up for free grid position and put piece's sphere 1 there TODO

        // set selectedPiece in GameManager and put at top of grid TODO weg?
        GameManager.selectedPiece = this;
        gridPos = new Vector3Int(0, 5, 0);
        gameObject.transform.position = GridFunct.CalcGridToGlobalSpace(gridPos);

        // disable sphere colliders, so that piece can move freely in the grid
        SphereCollider[] components = gameObject.GetComponentsInChildren<SphereCollider>(true); // (true = also inactive objects) TODO source ChatGPT
        foreach (SphereCollider component in components)
        {
            component.enabled = false; // Disable the component
        }
    }

    public void PieceUnselected()
    {
        // put selected to initial position TODO

        // reset orientation TODO

        // set sphereColliders active TODO

        // selected = null TODO
    }
}

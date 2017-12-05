using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BGEditor.NodeSystem;

public class PrototypeInteractionController : MonoBehaviour {

    bool isSelected = false;
    List<Cell> possibleCellOfMovement = new List<Cell>();
    public GameObject prefab;


    Ray mouseProjection;
    Plane gridLevel;
    Vector3 mousePosition;



    private void OnMouseDown()
    {
        // Inizia draggaggio della pedina
        possibleCellOfMovement = GetComponent<MovementController>().EvaluateMovementPown();
        isSelected = true;
        ShowPossibleMovement();
    }

    /// <summary>
    /// Mostra le celle in cui è possibile arrivare col movimento
    /// </summary>
    public void ShowPossibleMovement()
    {
        foreach (Cell cell in possibleCellOfMovement)
        {
            Instantiate(prefab, cell.GetPosition(), Quaternion.identity);
        }
    }

    // Use this for initialization
    void Start () {
        gridLevel = new Plane(Vector3.up,0);
    }

    // Update is called once per frame
    void Update () {
        if (isSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                mouseProjection = Camera.main.ScreenPointToRay(Input.mousePosition);
                float distance;
                if (gridLevel.Raycast(mouseProjection, out distance))
                {
                    mousePosition = mouseProjection.GetPoint(distance);
                    Debug.Log(mousePosition);
                }

                // determina la cella più vicina analizzando la distanza dal centro di ognuna (?)
            }

        }
	}
}

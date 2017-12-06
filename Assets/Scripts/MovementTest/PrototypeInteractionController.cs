using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BGEditor.NodeSystem;

public class PrototypeInteractionController : MonoBehaviour {

    private bool _isSelected;

    public bool isSelected
    {
        get { return _isSelected; }
        set {
            _isSelected = value;
            if (value == false)
                clickCounter = 0;
        }
    }


    List<Cell> possibleCellOfMovement = new List<Cell>();
    public GameObject prefab;
    List<GameObject> cubeInstantiated = new List<GameObject>();

    Ray mouseProjection;
    Plane gridLevel;
    Vector3 mousePosition;
    Vector3 pownStartPosition;

    MovementController movementCtrl;

    public MovementMode CurrentMovementMode;

    int clickCounter = 0;

    private void OnMouseDown()
    {
        if (movementCtrl == null)
            movementCtrl = GetComponent<MovementController>();
        possibleCellOfMovement = movementCtrl.EvaluateMovementPown();
        isSelected = true;
        ShowPossibleMovement();

        if (CurrentMovementMode == MovementMode.Click)
            clickCounter++;
        else if (CurrentMovementMode == MovementMode.DragAndDrop)
            pownStartPosition = transform.position;
    }

    // Use this for initialization
    void Start () {
        gridLevel = new Plane(Vector3.up,0);
    }

    // Update is called once per frame
    void Update () {
        if (isSelected)
        {
            FindMousePositionOnGridPlane();

            if (CurrentMovementMode == MovementMode.Click)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    if (CurrentMovementMode == MovementMode.Click)
                    {
                        /// Al click sull'oggetto il counter viene aumentato ad uno, quindi viene aumentato a due per impedire che rientri nello stesso if
                        /// ed esca dall'Update. Se viene ri clickato l'oggetto il counter passa a 3 entrando nel secondo if dove viene deselezionato l'oggetto
                        /// cancellato i cubi, e riazzerato il counter.

                        switch (clickCounter)
                        {
                            case 1:
                                clickCounter++;
                                return;
                            case 3:
                                isSelected = false;
                                HidePossibleMovement();
                                return;
                        }

                        // determina la cella più vicina analizzando la distanza dal centro di ognuna (le celle facenti parte della lsita di possibili celle di movimento)
                        FindCloseCellToMousePosition();

                        isSelected = false;
                        HidePossibleMovement();
                    }

                    if (CurrentMovementMode == MovementMode.DragAndDrop)
                    {
                        transform.position = mousePosition;
                    }
                } 
            }
            else if (CurrentMovementMode == MovementMode.DragAndDrop)
            {
                if (Input.GetMouseButton(0))
                {
                    transform.position = mousePosition;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    // determina la cella più vicina analizzando la distanza dal centro di ognuna (le celle facenti parte della lsita di possibili celle di movimento)
                    FindCloseCellToMousePosition();
                }

            }

        }
	}

    void FindCloseCellToMousePosition()
    {
        foreach (Cell cell in possibleCellOfMovement)
        {
            if (Vector3.Distance(cell.GetPosition(), mousePosition) < movementCtrl.Tester.SectorData.Radius / 2)
            {
                transform.position = cell.GetPosition();
                HidePossibleMovement();
                isSelected = false;
                return;
            }
            else
            {
                transform.position = pownStartPosition;
                HidePossibleMovement();
                isSelected = false;
            }
        }
    }

    void FindMousePositionOnGridPlane()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseProjection = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (gridLevel.Raycast(mouseProjection, out distance))
            mousePosition = mouseProjection.GetPoint(distance);
    }

    void HidePossibleMovement()
    {
        for (int i = 0; i < cubeInstantiated.Count; i++)
        {
            Destroy(cubeInstantiated[i]);
        }
    }

    /// <summary>
    /// Mostra le celle in cui è possibile arrivare col movimento
    /// </summary>
    public void ShowPossibleMovement()
    {
        foreach (Cell cell in possibleCellOfMovement)
        {
            GameObject tempObj = Instantiate(prefab, cell.GetPosition(), Quaternion.identity);
            cubeInstantiated.Add(tempObj);
        }
    }

    public enum MovementMode
    {
        DragAndDrop,
        Click
    }
}

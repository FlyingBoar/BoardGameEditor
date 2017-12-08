using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BGEditor.NodeSystem;

public class PrototypeInteractionController : MonoBehaviour
{

    private bool _isSelected;
    public bool isSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            if (!_isSelected)
                OnMouseUnselect();
            else
                OnMouseSelection();
        }
    }

    int clickCounter = 0;
    public MovementMode CurrentMovementMode;
    MovementController movementCtrl;

    //------ViewLayer
    List<Cell> possibleCellOfMovement = new List<Cell>();
    public GameObject prefabViewGraphic;
    List<GameObject> cubeInstantiated = new List<GameObject>();
    /// <summary>
    /// Mostra le celle in cui è possibile arrivare col movimento
    /// </summary>
    public void ShowPossibleMovement()
    {
        foreach (Cell cell in possibleCellOfMovement)
        {
            GameObject tempObj = Instantiate(prefabViewGraphic, cell.GetPosition(), Quaternion.identity);
            cubeInstantiated.Add(tempObj);
        }
    }
    void HidePossibleMovement()
    {
        for (int i = 0; i < cubeInstantiated.Count; i++)
        {
            Destroy(cubeInstantiated[i]);
        }
    }
    //------InputLayer
    Ray mouseProjection;
    Plane gridLevel;
    Vector3 mousePosition { get { return Test_FindMousePositionOnGridPlane(); } }
    Vector3 Test_FindMousePositionOnGridPlane()
    {
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseProjection = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (gridLevel.Raycast(mouseProjection, out distance))
            currentMousePosition = mouseProjection.GetPoint(distance);

        return currentMousePosition;
    }

    Vector3 pownStartPosition;
    //------------------

    private void OnMouseDown()
    {
        isSelected = true;
    }
    /// <summary>
    /// Instruction called on selection of this Obj.
    /// </summary>
    void OnMouseSelection()
    {
        possibleCellOfMovement = movementCtrl.EvaluateMovementPown();
        ShowPossibleMovement();

        if (CurrentMovementMode == MovementMode.Click)
            clickCounter++;
        else if (CurrentMovementMode == MovementMode.DragAndDrop)
            pownStartPosition = transform.position;
    }
    /// <summary>
    /// Instruction called when this Obj is no more seleced
    /// </summary>
    void OnMouseUnselect()
    {
        clickCounter = 0;
        HidePossibleMovement();
    }

    // Use this for initialization
    void Start()
    {
        gridLevel = new Plane(Vector3.up, 0);
        movementCtrl = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
            UpdateSelectedStatus();
    }

    void UpdateSelectedStatus()
    {
        if (CurrentMovementMode == MovementMode.Click)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (clickCounter > 1)
                {
                    DropAttempt(Tester.ReturnCellFromPosition(mousePosition));
                    isSelected = false;
                    return;
                }
                clickCounter++;
            }
        }
        else if (CurrentMovementMode == MovementMode.DragAndDrop)
        {
            if (Input.GetMouseButton(0))
            {
                movementCtrl.Drag(mousePosition);
            }
            if (Input.GetMouseButtonUp(0))
            {
                // determina la cella più vicina analizzando la distanza dal centro di ognuna (le celle facenti parte della lsita di possibili celle di movimento)
                DropAttempt(Tester.ReturnCellFromPosition(mousePosition));
                isSelected = false;
                return;
            }
        }
    }

    /// <summary>
    /// It handle the drop attempt
    /// </summary>
    /// <param name="_cell"></param>
    void DropAttempt(Cell _cell)
    {
        if (possibleCellOfMovement.Contains(_cell))
            movementCtrl.Drop(_cell.GetCenter());
        else
            movementCtrl.Drop(pownStartPosition);
    }

    public enum MovementMode
    {
        DragAndDrop,
        Click
    }
}

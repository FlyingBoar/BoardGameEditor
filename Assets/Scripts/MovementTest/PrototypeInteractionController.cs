using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;

public class PrototypeInteractionController : MonoBehaviour
{
    public GridController TESTGridController;
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
    public ViewController_test ViewController;
    MovementController movementCtrl;

    List<Cell> possibleCellOfMovement = new List<Cell>();

    Vector3 pownStartPosition;
    //------------------

    void Start()
    {
        movementCtrl = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
            UpdateSelectedStatus();
    }

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
        ViewController.ShowPossibleMovement(possibleCellOfMovement);

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
        ViewController.HidePossibleMovement();
    }

    void UpdateSelectedStatus()
    {
        if (CurrentMovementMode == MovementMode.Click)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (clickCounter > 1)
                {
                    DropAttempt(TESTGridController.GetCellFromPosition(InputAdapter_Tester.I.PointerPosition));
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
                movementCtrl.Drag(InputAdapter_Tester.I.PointerPosition);
            }
            if (Input.GetMouseButtonUp(0))
            {
                // determina la cella più vicina analizzando la distanza dal centro di ognuna (le celle facenti parte della lsita di possibili celle di movimento)
                DropAttempt(TESTGridController.GetCellFromPosition(InputAdapter_Tester.I.PointerPosition));
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
        {
            if (CurrentMovementMode == MovementMode.Click)
                isSelected = false;
            else
                movementCtrl.Drop(pownStartPosition);
        }
    }

    public enum MovementMode
    {
        DragAndDrop,
        Click
    }
}

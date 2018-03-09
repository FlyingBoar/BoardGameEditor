using UnityEngine;
using Grid;

[ExecuteInEditMode]
public class DirectionTester : MonoBehaviour
{
    public bool GridSpace;
    bool _gridSpace;
    // Use this for initialization
    void Update()
    {

        if (!GridSpace && _gridSpace != GridSpace)
        {
            _gridSpace = GridSpace;

            transform.rotation = MasterGrid.gridCtrl.RotationToGridSpace;
        }

        if (GridSpace && _gridSpace != GridSpace)
        {
            _gridSpace = GridSpace;
            transform.rotation = Quaternion.identity;
        }
    }
}

using System.Collections;
using BGEditor.NodeSystem;
using UnityEngine;


[ExecuteInEditMode]
public class Tester : MonoBehaviour
{
    public GridController GridCtrl;

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GridCtrl.CreateNewGrid();
        }
        if (Input.GetKeyDown(KeyCode.D))
            GridCtrl.ClearGrid();
        if (Input.GetKeyDown(KeyCode.S))
            GridCtrl.SaveCurrent();
        if (Input.GetKeyDown(KeyCode.L))
            GridCtrl.Load();
    }
}

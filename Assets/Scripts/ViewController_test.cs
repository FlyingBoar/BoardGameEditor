using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;


public class ViewController_test : MonoBehaviour {

    public GameObject prefabViewGraphic;
    List<GameObject> cubeInstantiated = new List<GameObject>();

    /// <summary>
    /// Crea un cubo sul centro di ogni cella presente nella lista passata come parametro
    /// </summary>
    /// <param name="_cellToShow">La cella su cui creare i cubi</param>
    public void ShowPossibleMovement(List<Cell> _cellToShow)
    {
        foreach (Cell cell in _cellToShow)
        {
            GameObject tempObj = Instantiate(prefabViewGraphic, cell.GetPosition(), Quaternion.identity);
            cubeInstantiated.Add(tempObj);
        }
    }

    /// <summary>
    /// Distrugge i cubi creati in precedenza dalla funzione ShowPossibleMovement
    /// </summary>
    public void HidePossibleMovement()
    {
        for (int i = 0; i < cubeInstantiated.Count; i++)
        {
            Destroy(cubeInstantiated[i]);
        }
    }
}

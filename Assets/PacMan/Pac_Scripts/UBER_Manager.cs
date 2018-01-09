using Grid;
using UnityEngine;

public class UBER_Manager : MonoBehaviour {

    public GridData Datas;
    GridController gridCtrl;
    GridLayerController gridLayerCtrl;
    public GameObject PacMan;
    Cell pacmanCell { get { return gridCtrl.GetCellFromPosition(PacMan.transform.position); } }

    private void Start()
    {
        gridCtrl = new GridController();
        gridLayerCtrl = new GridLayerController(gridCtrl);
        gridCtrl.CreateNewGrid(Datas);

    }

    private void Update()
    {
        Vector3Int coordinatesOfNext = pacmanCell.GridCoordinates;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            coordinatesOfNext += Vector3Int.right;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            coordinatesOfNext -= Vector3Int.right;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            coordinatesOfNext += Vector3Int.up;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            coordinatesOfNext -= Vector3Int.up;
        }

        // TODO : controllare
        //if (pacmanCell.GetNeighbourgs(gridLayerCtrl.Layers[0]).Contains(gridCtrl.GetCellByCoordinates(coordinatesOfNext.x, coordinatesOfNext.y, coordinatesOfNext.z)))
        //    Snap(gridCtrl.GetPositionByCoordinates(coordinatesOfNext.x, coordinatesOfNext.y, coordinatesOfNext.z));
    }

    void Snap(Vector3 _target)
    {
        PacMan.transform.position = Vector3.Lerp(PacMan.transform.position, _target, Time.deltaTime);
    }

}

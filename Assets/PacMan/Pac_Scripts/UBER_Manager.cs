using Grid;
using UnityEngine;

public class UBER_Manager : MonoBehaviour {

    public GridData Datas;
    GridController gridCtrl;
    LayerController gridLayerCtrl;
    public GameObject PacMan;
    Cell pacmanCell { get { return gridCtrl.GetCellFromPosition(PacMan.transform.position); } }

    private void Start()
    {
        gridCtrl = new GridController();
        gridLayerCtrl = new LayerController(gridCtrl);

        gridLayerCtrl.Layers = Datas.Layers;
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

        if (pacmanCell.GetNeighbourgs(gridLayerCtrl.Layers[0]).Contains(gridCtrl.GetCellByCoordinates(coordinatesOfNext).GridCoordinates))
            Snap(gridCtrl.GetPositionByCoordinates(coordinatesOfNext));
    }

    void Snap(Vector3 _target)
    {
        PacMan.transform.position = Vector3.Lerp(PacMan.transform.position, _target, Time.deltaTime);
    }

}

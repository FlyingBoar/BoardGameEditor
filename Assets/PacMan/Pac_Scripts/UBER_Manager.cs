using Grid;
using UnityEngine;

public class UBER_Manager : MonoBehaviour {

    public GridData Datas;
    GridController gridCtrl;
    GridLayerController gridLayerCtrl;
    public GameObject PacMan;
    public string MapName;
    Cell pacmanCell { get { return gridCtrl.GetCellFromPosition(PacMan.transform.position); } }

    private void Start()
    {
        gridCtrl = new GridController();
        gridLayerCtrl = new GridLayerController(gridCtrl);
        gridCtrl.LayerCtrl = gridLayerCtrl;
        MapName = "Assets/GridData/" + MapName + ".json";
        gridCtrl.Load(MapName);
        Datas = gridCtrl.GridData;
        //gridLayerCtrl.Layers = Datas.Layers;
    }

    private void Update()
    {
        Vector3Int coordinatesOfNext = pacmanCell.GridCoordinates;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            coordinatesOfNext += new Vector3Int(0, 0, 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            coordinatesOfNext += new Vector3Int(0, 0, -1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            coordinatesOfNext += Vector3Int.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            coordinatesOfNext += Vector3Int.right;
        }

        if (pacmanCell.GetNeighbourgs(gridLayerCtrl.Layers[0]).Contains(gridCtrl.GetCellByCoordinates(coordinatesOfNext).GridCoordinates))
            Snap(gridCtrl.GetPositionByCoordinates(coordinatesOfNext));
    }

    void Snap(Vector3 _target)
    {
        //PacMan.transform.position = Vector3.Lerp(PacMan.transform.position, _target, 1);
        PacMan.transform.position = _target;
    }

}

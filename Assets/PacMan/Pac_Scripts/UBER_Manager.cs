using Grid;
using UnityEngine;

public class UBER_Manager : MonoBehaviour {

    public GridData Datas;
    GridController gridCtrl;
    GridLayerController gridLayerCtrl;
    public GameObject PacMan;

    public TextAsset GridToLoad;
    Cell pacmanCell { get { return gridCtrl.GetCellFromPosition(PacMan.transform.position); } }

    private void Start()
    {
        gridCtrl = new GridController();
        gridLayerCtrl = new GridLayerController(gridCtrl);
        gridCtrl.LayerCtrl = gridLayerCtrl;
        LoadGrid();
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

    /// <summary>
    /// Cerca il path della griglie e lo carica
    /// </summary>
    void LoadGrid()
    {
        if(GridToLoad != null)
        {
            string assetPath = DataManager.GetAssetPath(GridToLoad);
            DataManager.LoadDataFromJson(assetPath);
            gridCtrl.ReInitVariables();
        }
    }

    void Snap(Vector3 _target)
    {
        //PacMan.transform.position = Vector3.Lerp(PacMan.transform.position, _target, 1);
        PacMan.transform.position = _target;
    }

}

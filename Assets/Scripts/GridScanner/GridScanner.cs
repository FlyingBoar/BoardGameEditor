using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridScanner
    {
        SectorData SectorData;
        List<INode> GridCells;

        ScanCollider scanCollider;
        List<INode> GridObstacleCells = new List<INode>();

        // TODO : da rivedere il tipo con cui viene passata la lista di celle
        public GridScanner(List<INode> _gridCells, SectorData _sectorData)
        {
            SectorData = _sectorData;
            GridCells = _gridCells;
        }

        public void ScanGrid()
        {
            if (scanCollider == null)
                CreateScannerCollider();

            RaycastHit hit = new RaycastHit();

            foreach (INode cell in GridCells)
            {
                scanCollider.transform.position = cell.GetPosition();
                Physics.Raycast((cell.GetPosition() + new Vector3(0, 1000, 0)), -Vector3.up, out hit);
                if(hit.transform.gameObject.GetComponent<ScanCollider>() != null)
                {
                    if (hit.transform.gameObject.GetComponent<ScanCollider>().ObjType == ObjectType.Obstacle)
                    {
                        GridObstacleCells.Add(cell);
                        UnlinkCell(cell);
                    }
                }
            }
            GameObject.Destroy(scanCollider.gameObject);
        }

        void UnlinkCell(INode _cell)
        {
            foreach (INode link in (_cell as Cell).GetCellData().LinkData.LinkedNodes)
            {
                (link as ILink).GetNeighbourgs().Remove(_cell);
            }
            (_cell as Cell).GetCellData().LinkData.LinkedNodes.Clear();
        }

        void CreateScannerCollider()
        {
            GameObject scannerObj = new GameObject("ScannerCollider");
            scanCollider = scannerObj.AddComponent<ScanCollider>();
            scanCollider.Init(SectorData, ObjectType.Cell, new Vector3(SectorData.Radius.x * 2, SectorData.Radius.y * 2, SectorData.Radius.z * 2));
        }
    }

    public enum ObjectType { Cell, Ignorable, Obstacle }
}
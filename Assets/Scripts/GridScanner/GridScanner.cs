using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridScanner
    {
        SectorData SectorData;
        List<Cell> GridCells;

        ScanCollider scanCollider;
        List<Cell> GridObstacleCells = new List<Cell>();

        // TODO : da rivedere il tipo con cui viene passata la lista di celle
        public GridScanner(List<Cell> _gridCells, SectorData _sectorData)
        {
            SectorData = _sectorData;
            GridCells = _gridCells;
        }

        public void ScanGrid()
        {
            if (scanCollider == null)
                CreateScannerCollider();

            RaycastHit hit = new RaycastHit();

            foreach (Cell cell in GridCells)
            {
                scanCollider.transform.position = cell.GetPosition();

                Physics.Raycast((cell.GetPosition() + new Vector3(0, 1000, 0)), -Vector3.up, out hit);
                ScanCollider hitCollider = hit.transform.gameObject.GetComponent<ScanCollider>();

                if (hitCollider != null && hitCollider.ObjType == ObjectType.Obstacle)
                {
                    GridObstacleCells.Add(cell);
                    cell.UnLinkAll();
                }
            }
            GameObject.Destroy(scanCollider.gameObject);
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
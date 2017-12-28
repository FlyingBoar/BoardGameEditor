using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [ExecuteInEditMode]
    public class GridScanner : MonoBehaviour
    {
        ScanCollider scanCollider;
        List<Cell> GridObstacleCells = new List<Cell>();

        public void ScanGrid(List<Cell> _gridCells, SectorData _sectorData)
        {
            if (scanCollider == null)
                CreateScannerCollider(_sectorData);

            RaycastHit hit = new RaycastHit();

            foreach (Cell cell in _gridCells)
            {
                scanCollider.transform.position = cell.GetPosition();

                Physics.Raycast((cell.GetPosition() + new Vector3(0, 1000, 0)), -Vector3.up, out hit);
                ScanCollider hitCollider = hit.transform.gameObject.GetComponent<ScanCollider>();

                if (hitCollider != null && hitCollider.ObjType == ObjectType.Obstacle)
                {
                    GridObstacleCells.Add(cell);
                    cell.UnLinkAll(hitCollider.Layer);
                }
            }
            GameObject.DestroyImmediate(scanCollider.gameObject);
        }

        void CreateScannerCollider(SectorData _sectorData)
        {
            GameObject scannerObj = new GameObject("ScannerCollider");
            scanCollider = scannerObj.AddComponent<ScanCollider>();
            scanCollider.Init(_sectorData, ObjectType.Cell, new Vector3(_sectorData.Radius.x * 2, _sectorData.Radius.y * 2, _sectorData.Radius.z * 2));
        }
    }

    public enum ObjectType { Cell, Ignorable, Obstacle }
}
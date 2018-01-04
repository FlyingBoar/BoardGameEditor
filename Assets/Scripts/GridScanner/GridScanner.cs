using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridScanner
    {
        ScannerCollider scanCollider;

        public GridScanner() { }

        public void ScanGrid(List<Cell> _gridCells, CellData.SectorData _sectorData)
        {
            if (scanCollider == null)
                CreateScannerCollider(_sectorData);

            RaycastHit hit = new RaycastHit();

            foreach (Cell cell in _gridCells)
            {
                scanCollider.transform.position = cell.GetPosition();

                Physics.Raycast((cell.GetPosition() + new Vector3(0, 1000, 0)), -Vector3.up, out hit);
                ScannerCollider hitCollider = hit.transform.gameObject.GetComponent<ScannerCollider>();

                if (hitCollider != null && hitCollider.ObjType == ObjectType.Obstacle)
                {
                    //for (int i = 0; i < hitCollider.Layers.Count; i++)
                    //{
                    //    cell.UnLinkAll();                   
                    //}
                }
            }
            GameObject.DestroyImmediate(scanCollider.gameObject);
        }

        void CreateScannerCollider(CellData.SectorData _sectorData)
        {
            GameObject scannerObj = new GameObject("ScannerCollider");
            scanCollider = scannerObj.AddComponent<ScannerCollider>();
            scanCollider.Init(_sectorData, ObjectType.Cell, new Vector3(_sectorData.Radius.x * 2, _sectorData.Radius.y * 2, _sectorData.Radius.z * 2));
        }
    }

    public enum ObjectType { Cell, Ignorable, Obstacle }
}
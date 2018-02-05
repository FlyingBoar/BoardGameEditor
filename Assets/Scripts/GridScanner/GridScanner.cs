using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Grid
{
    public class GridScanner
    {
        public GridScanner() { }

        public void ScanGrid(GridController _gridCtrl)
        {
            List<GridTags> gridTagsList = GameObject.FindObjectsOfType<GridTags>().ToList();

            foreach (GridTags tag in gridTagsList)
            {
                Cell cell = MasterGrid.GetCellFromPosition(tag.transform.position, true);
                foreach (ScannerLayer layer in tag.ScannerLayers)
                {
                    if(layer.Active)
                        cell.UnLinkAll(layer.Layer);
                }             
            }
        }
    }
}
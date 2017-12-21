using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid {
    public static class GridControllerExtension
    {
        /// <summary>
        /// data una posizione restituisce la cella corrispondente
        /// </summary>
        /// <param name="_position">la posizione da controllare</param>
        /// <returns>la cella che si trova in quella posizione</returns>
        public static Cell GetCellFromPosition(this GridController _gridCtrl, Vector3 _position)
        {
            Cell resultant;
            int[] indexes = _gridCtrl.GetCoordinatesByPosition(_position);
            resultant = _gridCtrl.GetCellsMatrix()[indexes[0],indexes[1],indexes[2]];
            return resultant;
        }
        /// <summary>
        /// Return a specific cell based on coordinates
        /// </summary>
        /// <param name="_gridCtrl"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Cell GetCellByCoordinates(this GridController _gridCtrl, int i, int j, int k)
        {
            Cell resultant;
            resultant = _gridCtrl.GetCellsMatrix()[i, j, k];
            return resultant;
        }
        /// <summary>
        /// Return the center postion of the cell
        /// </summary>
        /// <param name="_gridCtrl"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Vector3 GetPositionByCoordinates(this GridController _gridCtrl, int i, int j, int k)
        {

            Vector3 spacePos = new Vector3(i * _gridCtrl.SectorData.Diameter.x,j * _gridCtrl.SectorData.Diameter.y, k * _gridCtrl.SectorData.Diameter.z);
            spacePos += _gridCtrl.SectorData.Radius;
            //spacePos -= _gridCtrl.GetOffset();

            return spacePos;
        }
        //Associate the coordinate of clostest Cell center (or cell array index)
        //to the position vector
        //By Math: this is actually the metric conversion of the Grid Sub Vectorial Space, but
        //the int cast on the normalized position
        public static int[] GetCoordinatesByPosition(this GridController _gridCtrl, Vector3 _position)
        {
            Vector3 spacePos = _position /*+ _gridCtrl.GetOffset()*/;

            int[] coordinates = new int[]
            {
                //Radius == 0 is indefined. 0 as default
                //Postion/Radius is the normalized space position
                //i = V/(2R) + 1
                _gridCtrl.SectorData.Radius.x != 0 ?(int)(spacePos.x/_gridCtrl.SectorData.Radius.x)/2 +1 : 0,
                _gridCtrl.SectorData.Radius.y != 0 ?(int)(spacePos.y/_gridCtrl.SectorData.Radius.y)/2 +1 : 0,
                _gridCtrl.SectorData.Radius.z != 0 ?(int)(spacePos.x/_gridCtrl.SectorData.Radius.z)/2 +1 : 0,
            };

            return coordinates;
        }
    }
}

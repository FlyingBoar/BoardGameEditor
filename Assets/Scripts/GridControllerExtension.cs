using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public static class GridControllerExtension
    {
        /// <summary>
        /// data una posizione restituisce la cella corrispondente
        /// </summary>
        /// <param name="_position">la posizione da controllare</param>
        /// <returns>la cella che si trova in quella posizione</returns>
        public static Cell GetCellFromPosition(this GridController _gridCtrl, Vector3 _position)
        {
            int[] indexes = _gridCtrl.GetCoordinatesByPosition(_position);
            Cell[,,] matrix = _gridCtrl.GetCellsMatrix();

            try
            {
                return matrix[indexes[0], indexes[1], indexes[2]];
            }
            catch(System.IndexOutOfRangeException e)
            {
                Debug.LogErrorFormat(e.Message);
                return null;
            }
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
            Cell[,,] matrix = _gridCtrl.GetCellsMatrix();

            try
            {
                return matrix[i, j, k];
            }
            catch (System.IndexOutOfRangeException e)
            {
                Debug.LogErrorFormat(e.Message);
                return null;
            }
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

            Vector3 spacePos = new Vector3(
                i * (_gridCtrl.SectorData.Diameter.x + _gridCtrl.ResolutionCorrection.x),
                j * (_gridCtrl.SectorData.Diameter.y + _gridCtrl.ResolutionCorrection.y),
                k * (_gridCtrl.SectorData.Diameter.z + _gridCtrl.ResolutionCorrection.z));
            spacePos += _gridCtrl.SectorData.Radius + _gridCtrl.transform.position;

            return spacePos;
        }
        //Associate the coordinate of clostest Cell center (or cell array index)
        //to the position vector
        //By Math: this is actually the metric conversion of the Grid Sub Vectorial Space, but
        //the int cast on the normalized position
        public static int[] GetCoordinatesByPosition(this GridController _gridCtrl, Vector3 _position)
        {
            Vector3 spacePos = _position - _gridCtrl.transform.position - _gridCtrl.ResolutionCorrection;

            int[] coordinates = new int[]
            {
                _gridCtrl.SectorData.Radius.x != 0 ?(int)(spacePos.y/_gridCtrl.SectorData.Radius.x): 0,
                _gridCtrl.SectorData.Radius.y != 0 ?(int)(spacePos.x/_gridCtrl.SectorData.Radius.y): 0,
                _gridCtrl.SectorData.Radius.z != 0 ?(int)(spacePos.x/_gridCtrl.SectorData.Radius.z): 0,
            };

            return coordinates;
        }
    }
}

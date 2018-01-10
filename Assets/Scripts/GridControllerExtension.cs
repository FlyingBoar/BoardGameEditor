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
        public static Cell GetCellFromPosition(this GridController _gridCtrl, Vector3 _position, bool isInRadius = false)
        {
            bool _inRadius = isInRadius;
            Vector3Int indexes = _gridCtrl.GetCoordinatesByPosition(_position, out _inRadius);
            if (!_inRadius)
                return null;
            Cell[,,] matrix = _gridCtrl.GetCellsMatrix();

            try
            {
                return matrix[indexes.x, indexes.y, indexes.z];
            }
            catch(System.IndexOutOfRangeException)
            {
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
        public static Cell GetCellByCoordinates(this GridController _gridCtrl, Vector3Int _coordinates)
        {
            Cell[,,] matrix = _gridCtrl.GetCellsMatrix();

            try
            {
                return matrix[_coordinates.x, _coordinates.y, _coordinates.z];
            }
            catch (System.IndexOutOfRangeException)
            {
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
        public static Vector3 GetPositionByCoordinates(this GridController _gridCtrl, Vector3Int _coordinates)
        {
            Vector3 spacePos = new Vector3(
                _coordinates.x * (_gridCtrl.SectorData.Diameter.x + _gridCtrl.ResolutionCorrection.x),
                _coordinates.y * (_gridCtrl.SectorData.Diameter.y + _gridCtrl.ResolutionCorrection.y),
                _coordinates.z * (_gridCtrl.SectorData.Diameter.z + _gridCtrl.ResolutionCorrection.z));
            spacePos += _gridCtrl.Origin;

            return spacePos;
        }
        /// <summary>
        /// Associate the coordinate of clostest Cell center (or cell array index) to the position vector 
        /// By Math: this is actually the metric conversion of the Grid Sub Vectorial Space, but the int cast on the normalized position
        /// </summary>
        /// <param name="_gridCtrl"></param>
        /// <param name="_position"></param>
        /// <returns></returns>
        public static Vector3Int GetCoordinatesByPosition(this GridController _gridCtrl, Vector3 _position)
        {
            Vector3 spacePos = _position -_gridCtrl.Origin;
            Vector3 normFactor = _gridCtrl.SectorData.Diameter + _gridCtrl.ResolutionCorrection;

            int[] coordinates = new int[]
            {
                normFactor.x != 0 ? Mathf.RoundToInt(spacePos.x/normFactor.x): 0,
                normFactor.y != 0 ? Mathf.RoundToInt(spacePos.y/normFactor.y): 0,
                normFactor.z != 0 ? Mathf.RoundToInt(spacePos.z/normFactor.z): 0,
            };

            

            return new Vector3Int(coordinates[0], coordinates[1], coordinates[2]);
        }
        //Associate the coordinate of clostest Cell center (or cell array index)
        //to the position vector
        //By Math: this is actually the metric conversion of the Grid Sub Vectorial Space, but
        //the int cast on the normalized position
        public static Vector3Int GetCoordinatesByPosition(this GridController _gridCtrl, Vector3 _position, out bool _isInCellRadius)
        {
            Vector3Int coordinates = GetCoordinatesByPosition(_gridCtrl, _position);
            bool _isInCell = true;
            
            Vector3 centerPos = _gridCtrl.GetPositionByCoordinates(coordinates);
            if (Mathf.Abs(centerPos.x - _position.x) > _gridCtrl.SectorData.Radius.x)
                _isInCell= false;

            if (Mathf.Abs(centerPos.y - _position.y) > _gridCtrl.SectorData.Radius.y)
                _isInCell = false;

            if (Mathf.Abs(centerPos.z - _position.z) > _gridCtrl.SectorData.Radius.z)
                _isInCell = false;

            _isInCellRadius = _isInCell;
            return coordinates;
        }
    }
}

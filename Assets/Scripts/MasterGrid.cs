using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid 
{
    public static class MasterGrid {

        public static GridController gridCtrl;
        public static GridLayerController gridLayerCtrl;

        public static void Init() {
            gridCtrl = new GridController();
            gridLayerCtrl = new GridLayerController(gridCtrl);
            gridCtrl.Init(gridLayerCtrl);
        }

        #region API
        /// <summary>
        /// Return the center postion of the cell
        /// </summary>
        /// <param name="_gridCtrl"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Vector3 GetPositionByCoordinates(Vector3Int _coordinates)
        {
            Vector3 spacePos = new Vector3(
                _coordinates.x * (gridCtrl.SectorData.Diameter.x + gridCtrl.ResolutionCorrection.x),
                _coordinates.y * (gridCtrl.SectorData.Diameter.y + gridCtrl.ResolutionCorrection.y),
                _coordinates.z * (gridCtrl.SectorData.Diameter.z + gridCtrl.ResolutionCorrection.z));
            spacePos += gridCtrl.Origin;

            return spacePos;
        }
        /// <summary>
        /// Associate the coordinate of clostest Cell center (or cell array index) to the position vector 
        /// By Math: this is actually the metric conversion of the Grid Sub Vectorial Space, but the int cast on the normalized position
        /// </summary>
        /// <param name="_gridCtrl"></param>
        /// <param name="_position"></param>
        /// <returns></returns>
        public static Vector3Int GetCoordinatesByPosition(Vector3 _position)
        {
            Vector3 spacePos = _position - gridCtrl.Origin;
            Vector3 normFactor = gridCtrl.SectorData.Diameter + gridCtrl.ResolutionCorrection;

            int[] coordinates = new int[]
            {
                normFactor.x != 0 ? Mathf.RoundToInt(spacePos.x/normFactor.x): 0,
                normFactor.y != 0 ? Mathf.RoundToInt(spacePos.y/normFactor.y): 0,
                normFactor.z != 0 ? Mathf.RoundToInt(spacePos.z/normFactor.z): 0,
            };



            return new Vector3Int(coordinates[0], coordinates[1], coordinates[2]);
        }
        /// <summary>
        ///Associate the coordinate of clostest Cell center (or cell array index)
        ///to the position vector
        ///By Math: this is actually the metric conversion of the Grid Sub Vectorial Space, but
        ///the int cast on the normalized position
        /// <summary>
        public static Vector3Int GetCoordinatesByPosition(Vector3 _position, out bool _isInCellRadius)
        {
            Vector3Int coordinates = GetCoordinatesByPosition(_position);
            bool _isInCell = true;

            Vector3 centerPos = GetPositionByCoordinates(coordinates);
            if (Mathf.Abs(centerPos.x - _position.x) > gridCtrl.SectorData.Radius.x)
                _isInCell = false;

            if (Mathf.Abs(centerPos.y - _position.y) > gridCtrl.SectorData.Radius.y)
                _isInCell = false;

            if (Mathf.Abs(centerPos.z - _position.z) > gridCtrl.SectorData.Radius.z)
                _isInCell = false;

            _isInCellRadius = _isInCell;
            return coordinates;
        }

        /// <summary>
        /// Return the coordinate of Forward know a specific rotation
        /// </summary>
        /// <param name="_cellLocalRotation">Local rotation</param>
        /// <returns></returns>
        public static Vector3Int GetForward(RotationDegrees _cellLocalRotation)
        {
            float angle = (int)_cellLocalRotation * 45;
            Vector3 resultant = Vector3.forward;
            resultant = Quaternion.Euler(Vector3.up * angle) * resultant;
            if ((int)_cellLocalRotation % 2 != 0)
                resultant *= Mathf.Sqrt(2);
            Vector3Int resultantAsInt = new Vector3Int(Mathf.RoundToInt(resultant.x), Mathf.RoundToInt(resultant.y), Mathf.RoundToInt(resultant.z));
            return resultantAsInt;
        }
        /// <summary>
        /// Return the coordinate of Right know a specific rotation
        /// </summary>
        /// <param name="_cellLocalRotation">Local rotation</param>
        /// <returns></returns>
        public static Vector3Int GetRight(RotationDegrees _cellLocalRotation)
        {
            RotationDegrees newRot = _cellLocalRotation + 2;
            return GetForward(newRot);
        }
        /// <summary>
        /// Return the coordinate of Backward know a specific rotation
        /// </summary>
        /// <param name="_cellLocalRotation">Local rotation</param>
        /// <returns></returns>
        public static Vector3Int GetBackward(RotationDegrees _cellLocalRotation)
        {
            RotationDegrees newRot = _cellLocalRotation + 4;
            return GetForward(newRot);
        }
        /// <summary>
        /// Return the coordinate of Left know a specific rotation
        /// </summary>
        /// <param name="_cellLocalRotation">Local rotation</param>
        /// <returns></returns>
        public static Vector3Int GetLeft(RotationDegrees _cellLocalRotation)
        {
            RotationDegrees newRot = _cellLocalRotation + 6;
            return GetForward(newRot);
        }
        /// <summary>
        /// Return the coordinate of Forward-Right know a specific rotation
        /// </summary>
        /// <param name="_cellLocalRotation">Local rotation</param>
        /// <returns></returns>
        public static Vector3Int GetForwardRight(RotationDegrees _cellLocalRotation)
        {
            RotationDegrees newRot = _cellLocalRotation + 1;
            return GetForward(newRot);
        }
        /// <summary>
        /// Return the coordinate of Backward-Right know a specific rotation
        /// </summary>
        /// <param name="_cellLocalRotation">Local rotation</param>
        /// <returns></returns>
        public static Vector3Int GetBackwardRight(RotationDegrees _cellLocalRotation)
        {
            RotationDegrees newRot = _cellLocalRotation + 3;
            return GetForward(newRot);
        }
        /// <summary>
        /// Return the coordinate of Backward-Left know a specific rotation
        /// </summary>
        /// <param name="_cellLocalRotation">Local rotation</param>
        /// <returns></returns>
        public static Vector3Int GetBackwardLeft(RotationDegrees _cellLocalRotation)
        {
            RotationDegrees newRot = _cellLocalRotation + 5;
            return GetForward(newRot);
        }
        /// <summary>
        /// Return the coordinate of Forward-Left know a specific rotation
        /// </summary>
        /// <param name="_cellLocalRotation">Local rotation</param>
        /// <returns></returns>
        public static Vector3Int GetForwardLeft(RotationDegrees _cellLocalRotation)
        {
            RotationDegrees newRot = _cellLocalRotation + 7;
            return GetForward(newRot);
        }
        /// <summary>
        /// Return the list of coordinates of the neighbours of a specific coordinate
        /// </summary>
        /// <param name="_coordinates">the position to start to take the coordinates of the neighbours </param>
        /// <returns></returns>
        public static List<Vector3Int> GetNeighbours(Vector3Int _coordinates, NeighboursShape _shape = NeighboursShape.All)
        {

            List<Vector3Int> _plusNeighbours = new List<Vector3Int>();
            List<Vector3Int> _crossNeighbours = new List<Vector3Int>();

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    if (i == 0 || j == 0)
                        _plusNeighbours.Add(_coordinates + new Vector3Int(i, 0, j));
                    else
                        _crossNeighbours.Add(_coordinates + new Vector3Int(i, 0, j));

                }
            }

            switch (_shape)
            {
                case NeighboursShape.All:
                    {
                        List<Vector3Int> allNeighbours = new List<Vector3Int>();
                        allNeighbours.AddRange(_plusNeighbours);
                        allNeighbours.AddRange(_crossNeighbours);
                        return allNeighbours;
                    }
                case NeighboursShape.Plus:
                    return _plusNeighbours;
                case NeighboursShape.Cross:
                    return _crossNeighbours;
                default:
                    return null;
            }
        }
        #endregion
    }

    public enum RotationDegrees
    {
        Angle_0 = 0,
        Angle_45,
        Angle_90,
        Angle_135,
        Angle_180,
        Angle_225,
        Angle_270,
        Angle_315,
    }

    public enum NeighboursShape
    {
        All,
        Plus,
        Cross
    }
}


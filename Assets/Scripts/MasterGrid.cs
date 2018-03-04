using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid 
{
    public static class MasterGrid
    {
        public static GridController gridCtrl;
        public static GridLayerController gridLayerCtrl;

        public static void Init()
        {
            gridCtrl = new GridController();
            gridLayerCtrl = new GridLayerController(gridCtrl);
            gridCtrl.Init(gridLayerCtrl);
        }

        #region API
        /// <summary>
        /// Rotate a vector to allign vector3.up with the Grid plane Normal
        /// </summary>
        /// <param name="_originalV"></param>
        /// <returns></returns>
        public static Vector3 ToGridVectorSpace(this Vector3 _originalV)
        {
            Vector3 gridSpaceVec = gridCtrl.RotationToGridSpace * _originalV;
            return gridSpaceVec;
        }
        /// <summary>
        /// Rotate a vector from the Grid vector space to the Unity default one.
        /// </summary>
        /// <param name="_originalV"></param>
        /// <returns></returns>
        public static Vector3 FromGridVectorSpace(this Vector3 _originalV)
        {
            Vector3 unitySpaceVec = Quaternion.Inverse(gridCtrl.RotationToGridSpace) * _originalV;
            return unitySpaceVec;
        }
        /// <summary>
        /// Return the center postion of the cell
        /// </summary>
        /// <param name="_gridCtrl"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Vector3 GetPositionByCoordinates(Vector2Int _coordinates)
        {
            Vector3 spacePos = new Vector3(
                _coordinates.x * (gridCtrl.SectorData.Diameter.x + gridCtrl.ResolutionCorrection.x),
                _coordinates.y * (gridCtrl.SectorData.Diameter.y + gridCtrl.ResolutionCorrection.y),
                0);
            spacePos = spacePos.FromGridVectorSpace();
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
        public static Vector2Int GetCoordinatesByPosition(Vector3 _position)
        {
            Vector3 spacePos = _position - gridCtrl.Origin;
            spacePos = spacePos.ToGridVectorSpace();
            Vector2 normFactor = gridCtrl.SectorData.Diameter + gridCtrl.ResolutionCorrection;

            int[] coordinates = new int[]
            {
                normFactor.x != 0 ? Mathf.RoundToInt(spacePos.x/normFactor.x): 0,
                normFactor.y != 0 ? Mathf.RoundToInt(spacePos.y/normFactor.y): 0,
            };

            return new Vector2Int(coordinates[0], coordinates[1]);
        }
        /// <summary>
        ///Associate the coordinate of clostest Cell center (or cell array index)
        ///to the position vector
        ///By Math: this is actually the metric conversion of the Grid Sub Vectorial Space, but
        ///the int cast on the normalized position
        /// <summary>
        public static Vector2Int GetCoordinatesByPosition(Vector3 _position, out bool _isInCellRadius)
        {
            Vector2Int coordinates = GetCoordinatesByPosition(_position);
            bool _isInCell = true;

            Vector3 centerPos = GetPositionByCoordinates(coordinates);
            if (Mathf.Abs(centerPos.x - _position.x) > gridCtrl.SectorData.Radius.x)
                _isInCell = false;

            if (Mathf.Abs(centerPos.y - _position.y) > gridCtrl.SectorData.Radius.y)
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
        /// <param name="_coordinates">The position to start to take the coordinates of the neighbours</param>
        /// <returns></returns>
        public static List<Vector2Int> GetNeighbours(Vector2Int _coordinates, NeighboursShape _shape = NeighboursShape.All)
        {
            List<Vector2Int> _plusNeighbours = new List<Vector2Int>();
            List<Vector2Int> _crossNeighbours = new List<Vector2Int>();

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    if (i == 0 || j == 0)
                        _plusNeighbours.Add(_coordinates + new Vector2Int(i, j));
                    else
                        _crossNeighbours.Add(_coordinates + new Vector2Int(i, j));
                }
            }

            switch (_shape)
            {
                case NeighboursShape.All:
                    List<Vector2Int> allNeighbours = new List<Vector2Int>();
                    allNeighbours.AddRange(_plusNeighbours);
                    allNeighbours.AddRange(_crossNeighbours);
                    return allNeighbours;
                case NeighboursShape.Plus:
                    return _plusNeighbours;
                case NeighboursShape.Cross:
                    return _crossNeighbours;
                default:
                    return null;
            }
        }
        /// <summary>
        /// Return the list of coordinates of the neighbours available on a specific coordinate, filtered by the type of LinkNetwork
        /// </summary>
        /// <param name="_coordinates">The position to start to take the coordinates of the neighbours</param>
        /// <param name="_networkType">The type of the LinkNetwork to search</param>
        /// <returns></returns>
        public static List<Vector2Int> GetNeighboursByLinkNetwork(Vector2Int _coordinates, LinkNetworkType _networkType, NeighboursShape _shape = NeighboursShape.All)
        {
            List<Layer> layers = gridLayerCtrl.GetLayers();

            List<Vector2Int> filteredNeighbours = GetNeighboursByLinkNetworkUnfiltered(_coordinates, _networkType, layers, _shape);

            List<LayerItem> itemsToCheck = new List<LayerItem>();
            LayerItem itemFound = null;
            for (int i = 0; i < filteredNeighbours.Count; i++)
            {
                itemFound = SearchLayerItemByCoordinates(_coordinates, layers);
                if (itemFound != null)
                    itemsToCheck.Add(itemFound);
            }

            List<Vector2Int> unfilterdNeighboursLinks = new List<Vector2Int>();

            for (int i = 0; i < itemsToCheck.Count; i++)
            {
                unfilterdNeighboursLinks.AddRange(GetNeighboursByLinkNetworkUnfiltered(itemsToCheck[i].GetData().GridCoordinates, _networkType, layers, _shape));
                for (int j = 0; j < unfilterdNeighboursLinks.Count; j++)
                {
                    filteredNeighbours.Remove(unfilterdNeighboursLinks[i]);
                }
            }

            return filteredNeighbours;
        }
        #endregion

        #region LinkNetwork
        /// <summary>
        /// Return the list of the neighbours, removing the one blocked by the LayerItem if the is one
        /// </summary>
        /// <param name="_coordinates"></param>
        /// <param name="_networkType"></param>
        /// <param name="_layers"></param>
        /// <param name="_shape"></param>
        /// <returns></returns>
        static List<Vector2Int> GetNeighboursByLinkNetworkUnfiltered(Vector2Int _coordinates, LinkNetworkType _networkType, List<Layer> _layers, NeighboursShape _shape = NeighboursShape.All)
        {
            List<Vector2Int> filteredNeighbours = GetNeighbours(_coordinates, _shape);
            
            LayerItem itemFound = SearchLayerItemByCoordinates(_coordinates, _layers);
            if (itemFound != null)
            {
                List<Vector2Int> links = itemFound.GetBlockedLinkNetworkByID(_networkType.ID).GetLinks();
                for (int i = 0; i < links.Count; i++)
                {
                    filteredNeighbours.Remove(links[i] + _coordinates);
                }
            }
            return filteredNeighbours;
        }

        /// <summary>
        /// Return the LayerItem at the given coordinates if the is one, else return null
        /// </summary>
        /// <param name="_coordinates"></param>
        /// <param name="_layers"></param>
        /// <returns></returns>
        static LayerItem SearchLayerItemByCoordinates(Vector2Int _coordinates, List<Layer> _layers)
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                for (int j = 0; j < _layers[i].LayerItemInstances.Count; j++)
                {
                    LayerItem item = _layers[i].LayerItemInstances[j];
                    if (item.GetData().GridCoordinates == _coordinates)
                    {
                        return item;
                    }
                }
            }
            return null;
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


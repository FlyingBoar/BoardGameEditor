using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class LayerItemData
    {
        public Vector3Int GridCoordinates;
        public RotationDegrees Rotation;
        public string PrefabName;

        public LayerItemData() { }

        public LayerItemData(Vector3Int _gridCoordinates, RotationDegrees _rotation, string _prefabName) {
            GridCoordinates = _gridCoordinates;
            Rotation = _rotation;
            PrefabName = _prefabName;
        }
    }
}
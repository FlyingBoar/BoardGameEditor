using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public struct LayerItemData
    {
        public Vector3Int Coordinates;
        public RotationDegrees Rotation;
        public string PrefabName;
    }
}
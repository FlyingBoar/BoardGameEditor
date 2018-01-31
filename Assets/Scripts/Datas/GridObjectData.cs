using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class GridObjectData
    {

        public Vector3Int Coordinates;
        public Rotation Rotation;
        public string PrefabName;

    }

    public enum Rotation
    {
        Nord,
        Est,
        Sud,
        Ovest
    }
}
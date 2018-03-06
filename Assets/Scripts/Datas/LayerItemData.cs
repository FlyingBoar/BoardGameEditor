using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class LayerItemData
    {
        public Vector2Int GridCoordinates;
        public RotationDegrees Rotation;
        public string PrefabName;

        public LayerItemData() { }

        public LayerItemData(Vector2Int _gridCoordinates, RotationDegrees _rotation, string _prefabName) {
            GridCoordinates = _gridCoordinates;
            Rotation = _rotation;
            PrefabName = _prefabName;
        }
    }
}
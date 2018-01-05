using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class ScannerCollider : MonoBehaviour
    {
        public ObjectType ObjType;
        public List<ScannerLayer> ScannerLayers;
        BoxCollider boxCollider;

        public void Init(CellData.SectorData _sectorData, ObjectType _objType)
        {
            Init(_sectorData, _objType, new Vector3(_sectorData.Radius.x, _sectorData.Radius.y, _sectorData.Radius.z));
        }

        public void Init(CellData.SectorData _sectorData, ObjectType _objType, Vector3 _size)
        {
            ObjType = _objType;
            boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = _size;
        }
    }

    public class ScannerLayer
    {
        public Layer Layer;
        public bool Active;

        public ScannerLayer(Layer _layer, bool _active)
        {
            Layer = _layer;
            Active = _active;
        }
    }
}
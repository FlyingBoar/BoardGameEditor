using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class ScanCollider : MonoBehaviour
    {
        public ObjectType ObjType;

        BoxCollider boxCollider;

        public void Init(SectorData _sectorData, ObjectType _objType)
        {
            Init(_sectorData, _objType, new Vector3(_sectorData.Radius.x, _sectorData.Radius.y, _sectorData.Radius.z));
        }

        public void Init(SectorData _sectorData, ObjectType _objType, Vector3 _size)
        {
            ObjType = _objType;
            boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = _size;
        }
    }
}
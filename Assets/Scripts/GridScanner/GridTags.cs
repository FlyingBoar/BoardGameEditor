using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    [ExecuteInEditMode]
    [System.Serializable]
    public class GridTags : MonoBehaviour
    {
        [System.NonSerialized]
        public List<ScannerLayer> ScannerLayers = new List<ScannerLayer>();
        public string Prefabname;
        public Vector3Int GridPosition;

        public void Awake()
        {
            Prefabname = PrefabUtility.GetPrefabParent(gameObject).name;
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
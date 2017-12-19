using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid {
    [CreateAssetMenu(fileName ="Network")]
    public class NodeNetworkData : ScriptableObject
    {
        public Vector3 Size;
        public List<CellData> Cells = new List<CellData>();
    }
}
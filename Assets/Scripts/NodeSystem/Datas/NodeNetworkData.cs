using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BGEditor.NodeSystem {
    [CreateAssetMenu(fileName ="Network")]
    public class NodeNetworkData : ScriptableObject
    {
        public Vector3 Size;
        public List<CellData> Cells = new List<CellData>();
    }
}
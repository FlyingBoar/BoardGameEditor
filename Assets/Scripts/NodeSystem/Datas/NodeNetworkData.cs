using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BGEditor.NodeSystem {
    [CreateAssetMenu(fileName ="Network")]
    public class NodeNetworkData : ScriptableObject {

        public List<CellData> Cells = new List<CellData>();

    }
}
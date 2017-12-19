using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [CreateAssetMenu(fileName ="Network")]
    public class NodeNetworkData : ScriptableObject
    {
        public Vector3Int Size;
        public Cell[,,] CellsMatrix;
    }
}
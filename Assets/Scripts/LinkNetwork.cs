using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class LinkNetwork
    {
        public string ID;
        [SerializeField]
        List<Vector3Int> blockedDirections = new List<Vector3Int>();

        public LinkNetwork(string _id)
        {
            ID = _id;
        }

        public List<Vector3Int> GetLinks()
        {
            return blockedDirections;
        }

        public void AddBlockedDirection(Vector3Int _direction)
        {
            if(!blockedDirections.Contains(_direction))
                blockedDirections.Add(_direction);
        }

        public void RemoveBlockedDirection(Vector3Int _direction)
        {
            blockedDirections.Remove(_direction);
        }
    }
}


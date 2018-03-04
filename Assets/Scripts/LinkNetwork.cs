using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class LinkNetwork
    {
        public string ID;
        [SerializeField]
        List<Vector2Int> blockedDirections = new List<Vector2Int>();

        public LinkNetwork(string _id)
        {
            ID = _id;
        }

        public List<Vector2Int> GetLinks()
        {
            return blockedDirections;
        }

        public void AddBlockedDirection(Vector2Int _direction)
        {
            if(!blockedDirections.Contains(_direction))
                blockedDirections.Add(_direction);
        }

        public void RemoveBlockedDirection(Vector2Int _direction)
        {
            blockedDirections.Remove(_direction);
        }
    }
}


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

        public LinkNetwork(LinkNetworkType _type)
        {
            ID = _type.ID;
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
    
    //[System.Serializable]
    //public struct Link
    //{
    //    public DirectionID ID;
    //    public Vector3Int Connection;

    //    public Link(DirectionID _id, Vector3Int _direction)
    //    {
    //        ID = _id;
    //        Connection = _direction;
    //    }
    //}

    //public enum DirectionID
    //{
    //    Forward,
    //    ForwardRight,
    //    Right,
    //    BackwardRight,
    //    Backward,
    //    BackwardLeft,
    //    Left,
    //    ForwardLeft
    //}
}


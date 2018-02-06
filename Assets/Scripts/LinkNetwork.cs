using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class LinkNetwork
    {
        public LinkNetworkType Type;
        List<Vector3Int> links = new List<Vector3Int>();

        public LinkNetwork(LinkNetworkType _type)
        {
            Type = _type;
        }

        public List<Vector3Int> GetLinks()
        {
            return links;
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


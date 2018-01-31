using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class LinkNetwork
    {
        List<Link> Links = new List<Link>();

        public LinkNetwork() { }

        public void AddLink(Link _newLink)
        {
            foreach (Link link in Links)
            {
                if (link.ID == _newLink.ID)
                    return;
            }

            Links.Add(_newLink);
        }
    } 
    
    public struct Link
    {
        public DirectionID ID;
        public Vector3Int Direction;

        public Link(DirectionID _id, Vector3Int _direction)
        {
            ID = _id;
            Direction = _direction;
        }
    }

    public enum DirectionID
    {
        Forward,
        ForwardRight,
        Right,
        BackwardRight,
        Backward,
        BackwardLeft,
        Left,
        ForwardLeft
    }
}


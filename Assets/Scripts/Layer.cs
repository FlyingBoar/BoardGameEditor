using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class Layer
    {
        public string Name;
        public bool IsEditable;

        public Layer(string _name, bool _isEditable)
        {
            Name = _name;
            IsEditable = _isEditable;
        }

        public static bool operator ==(Layer l1, Layer l2)
        {
            return l1.Equals(l2);
        }

        public static bool operator !=(Layer l1, Layer l2)
        {
            return !(l1 == l2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
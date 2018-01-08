using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class Layer
    {
        public string Name;
        public Color HandlesColor;

        public Layer()
        {
            HandlesColor.a = 100;
        }

        public Layer(string _name)
        {
            Name = _name;
            HandlesColor.a = 100;
        }

        public Layer(string _name, Color _gizmosColor)
        {
            Name = _name;
            HandlesColor = _gizmosColor;
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
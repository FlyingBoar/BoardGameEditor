using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Grid
{
    public class Layer
    {
        LayerData Data;

        public Layer()
        {
            Data.Color.a = 100;
        }

        public Layer(string _name)
        {
            Data.Name = _name;
            Data.Color.a = 100;
        }

        public Layer(string _name, Color _gizmosColor)
        {
            Data.Name = _name;
            Data.Color = _gizmosColor;
        }

        public string Save(GridController _gridCtrl)
        {

            return JsonUtility.ToJson(this);
        }

        public static bool operator ==(Layer l1, Layer l2)
        {
            if (l1.Data.Name == l2.Data.Name)
                return true;
            else
                return false;
        }

        public static bool operator !=(Layer l1, Layer l2)
        {
            if (l1.Data.Name == l2.Data.Name)
                return false;
            else
                return true;
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
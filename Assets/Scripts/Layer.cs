using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Grid
{
    [System.Serializable]
    public class Layer
    {
        public string Name;
        public Color Color;
        [SerializeField]
        List<LayerItemData> LayerItems;

        public Layer()
        {
            Color.a = 100;
        }

        public Layer(string _name)
        {
            Name = _name;
            Color.a = 100;
        }

        public Layer(string _name, Color _gizmosColor)
        {
            Name = _name;
            Color = _gizmosColor;
        }

        public string Save(GridController _gridCtrl)
        {

            return JsonUtility.ToJson(this);
        }

        public static bool operator ==(Layer l1, Layer l2)
        {
            if (l1.Name == l2.Name)
                return true;
            else
                return false;
        }

        public static bool operator !=(Layer l1, Layer l2)
        {
            if (l1.Name == l2.Name)
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
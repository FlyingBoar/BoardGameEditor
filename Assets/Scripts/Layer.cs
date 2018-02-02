using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class Layer
    {
        LayerData _data;
        public LayerData Data
        {
            get { return _data; }
            private set { _data = value; }
        }

        #region Constructors
        public Layer()
        {
            Data = new LayerData();
        }

        public Layer(string _name)
        {
            Data = new LayerData(_name);
        }

        public Layer(string _name, Color _color)
        {
            Data = new LayerData(_name, _color);
        }

        public Layer(LayerData _data)
        {
            Data = _data;
        }
        #endregion

        #region API
        public string SaveToJson(GridController _gridCtrl)
        {
            //AddObjectsInThisLayer(_gridCtrl);
            return JsonUtility.ToJson(Data);
        }
        #endregion

        void AddObjectsInThisLayer(GridController _gridCtrl)
        {
            LayerItem[] itemInLayer = GameObject.FindObjectsOfType<LayerItem>();
            Data.ItemsInLayer = new List<LayerItemData>();
            for (int i = 0; i < itemInLayer.Length; i++)
            {
                if(itemInLayer[i].MembershipLayer == this)
                    Data.ItemsInLayer.Add(itemInLayer[i].GetData());
            }
        }

        #region Operators
        public static bool operator ==(Layer l1, Layer l2)
        {
            if (l1.Data.ID == l2.Data.ID)
                return true;
            else
                return false;
        }

        public static bool operator !=(Layer l1, Layer l2)
        {
            if (l1.Data.ID == l2.Data.ID)
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
        #endregion
    }
}
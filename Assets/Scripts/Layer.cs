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
        public Color HandlesColor;
        public LayerType Type;
        [SerializeField]
        GridTags[] allGridTags;

        public string Save(GridController _gridCtrl) {
            switch (Type) {
                case LayerType.Prefab:
                    SavePrefab(_gridCtrl);
                    break;
                case LayerType.Movement:
                    SaveMovement();
                    break;
            }

            return JsonUtility.ToJson(this);
        }

        void SavePrefab(GridController _gridCtrl) {
            allGridTags = GameObject.FindObjectsOfType<GridTags>();
            foreach (GridTags item in allGridTags) {
                item.GridPosition = _gridCtrl.GetCoordinatesByPosition(item.transform.position);
            }
        }

        void SaveMovement() {

        }

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

        public enum LayerType {
            Prefab, Movement
        }
    }
}
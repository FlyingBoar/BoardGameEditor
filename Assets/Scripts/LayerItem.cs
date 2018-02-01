using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class LayerItem : MonoBehaviour
    {
        List<LinkNetwork> linkNetworks = new List<LinkNetwork>();

        public LayerItemData SaveToData(GridController _gridCtrl)
        {
            LayerItemData saveData = new LayerItemData();
            saveData.Coordinates = _gridCtrl.GetCoordinatesByPosition(transform.position);

            return saveData;
        }

        public void SetUpByData(LayerItemData _data, GridController _gridCtrl)
        {
            transform.position = _gridCtrl.GetPositionByCoordinates(_data.Coordinates);

            Vector3 positionToLook = _gridCtrl.GetPositionByCoordinates(GridControllerExtension.GetForward(_data.Rotation));
            transform.rotation = Quaternion.LookRotation(positionToLook);
        }

        public void GetNeighborhood() { }
    }
}


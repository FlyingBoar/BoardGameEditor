using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid 
{
    [ExecuteInEditMode]
    public class LayerItem : MonoBehaviour
    {
        List<LinkNetwork> linkNetworks = new List<LinkNetwork>();

        LayerItemData data = new LayerItemData();

        public Layer MembershipLayer { get; set; }

        #region API
        public void SetCoordinates(Vector3Int _gridCoordinates)
        {
            if (_gridCoordinates == data.GridCoordinates)
                return;
            data.GridCoordinates =_gridCoordinates;
            transform.position = MasterGrid.gridCtrl.GetPositionByCoordinates(data.GridCoordinates);
        }

        public LayerItemData GetData()
        {
            return data;
        }
        #endregion

        private void Awake()
        {
            if (MasterGrid.gridLayerCtrl != null)
                MasterGrid.gridLayerCtrl.NewItemInScene(this);
        }

        private void OnDestroy()
        {
            if (MasterGrid.gridLayerCtrl != null)
                MasterGrid.gridLayerCtrl.RemoveItemFromScene(this);
        }
    }
}


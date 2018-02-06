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
            transform.position = MasterGrid.GetPositionByCoordinates(data.GridCoordinates);
        }

        public LayerItemData GetData()
        {
            return data;
        }

        public LinkNetwork GetLinkNetworkByType(LinkNetworkType _networkType)
        {
            for (int i = 0; i < linkNetworks.Count; i++)
                if(linkNetworks[i].Type == _networkType)
                    return linkNetworks[i];

            return null;
        }

        public List<LinkNetwork> GetLinkNetworks()
        {
            return linkNetworks;
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


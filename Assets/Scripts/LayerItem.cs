using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid 
{
    [ExecuteInEditMode]
    public class LayerItem : MonoBehaviour
    {
        [SerializeField]
        List<LinkNetwork> blockedLinkNetworks = new List<LinkNetwork>();

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

        public LinkNetwork GetBlockedLinkNetworkByType(LinkNetworkType _networkType)
        {
            for (int i = 0; i < blockedLinkNetworks.Count; i++)
                if(blockedLinkNetworks[i].ID == _networkType.ID)
                    return blockedLinkNetworks[i];

            return null;
        }

        public void AddBlockedLink(Vector3Int _direction, LinkNetworkType _type)
        {
            foreach (LinkNetwork network in blockedLinkNetworks)
            {
                if(network.ID == _type.ID)
                {
                    network.AddBlockedDirection(_direction);
                    return;
                }
            }

            blockedLinkNetworks.Add(new LinkNetwork(_type));
            AddBlockedLink(_direction, _type);
        }

        public void RemoveBlockedLink(Vector3Int _direction, LinkNetworkType _type)
        {
            foreach (LinkNetwork network in blockedLinkNetworks)
            {
                if (network.ID == _type.ID)
                {
                    network.RemoveBlockedDirection(_direction);
                    return;
                }
            }
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


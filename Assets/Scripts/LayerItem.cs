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
        public void SetCoordinates(Vector2Int _gridCoordinates)
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

        public int GetBlockedLinkNetworksCount()
        {
            return blockedLinkNetworks.Count;
        }

        public LinkNetwork GetBlockedLinkNetworkByID(string _id)
        {
            for (int i = 0; i < blockedLinkNetworks.Count; i++)
                if(blockedLinkNetworks[i].ID == _id)
                    return blockedLinkNetworks[i];

            return null;
        }

        public LinkNetwork GetBlockedLinkNetworkByIndex(int _index)
        {
            if (_index < 0 || _index >= blockedLinkNetworks.Count)
                return null;
            else
                return blockedLinkNetworks[_index];
        }

        public void AddBlockedLink(Vector2Int _direction, string _id)
        {
            foreach (LinkNetwork network in blockedLinkNetworks)
            {
                if(network.ID == _id)
                {
                    network.AddBlockedDirection(_direction);
                    return;
                }
            }

            blockedLinkNetworks.Add(new LinkNetwork(_id));
            AddBlockedLink(_direction, _id);
        }

        public void RemoveBlockedLink(Vector2Int _direction, string _id)
        {
            foreach (LinkNetwork network in blockedLinkNetworks)
            {
                if (network.ID == _id)
                {
                    network.RemoveBlockedDirection(_direction);
                    return;
                }
            }
        }

        public void AddLinkNetwork(string _id)
        {
            blockedLinkNetworks.Add(new LinkNetwork(_id));
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


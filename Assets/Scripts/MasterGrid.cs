using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid 
{
    public static class MasterGrid {

        public static GridController gridCtrl;
        public static GridLayerController gridLayerCtrl;

        public static void Init() {
            gridCtrl = new GridController();
            gridLayerCtrl = new GridLayerController(gridCtrl);
            gridCtrl.Init(gridLayerCtrl);
        }
    }
}


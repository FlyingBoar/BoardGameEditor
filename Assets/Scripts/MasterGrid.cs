using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid {
    public class MasterGrid : EditorWindow
    {
        public static GridController GridCtrl { get; private set; }
        public static GridControllerWindow GridCtrlWindow { get; private set; }

        public static GridControllerVisualizer GridVisualizer { get; private set; }
        public static LayerController GridLayerCtrl { get; private set; }
        public static GridScanner GridScanner { get; private set; }

        [MenuItem("Window/Master Grid")]
        static void Init()
        {
            MasterGrid masterGrid = (MasterGrid)EditorWindow.GetWindow(typeof(MasterGrid));
            masterGrid.titleContent = new GUIContent("Master Grid");
            masterGrid.Show();

            GridCtrl = new GridController();
            GridVisualizer = new GridControllerVisualizer(GridCtrl);
            GridLayerCtrl = new LayerController(GridCtrl);
            GridScanner = new GridScanner();

            GridCtrl.Init(GridVisualizer, GridLayerCtrl);
        }

        private void OnGUI()
        {
            if (GridCtrlWindow == null)
                GridCtrlWindow = new GridControllerWindow(GridCtrl);
            GridCtrlWindow.Show();
        }
    }
}

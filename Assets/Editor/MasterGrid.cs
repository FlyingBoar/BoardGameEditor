using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    public class MasterGrid : EditorWindow
    {
        public static GridController GridCtrl { get; private set; }
        public static GridControllerWindow GridCtrlWindow { get; private set; }

        public static GridControllerVisualizer GridVisualizer { get; private set; }
        public static GridVisualizerWindow GridVisualizerWindow { get; private set; }

        public static LayerController LayerCtrl { get; private set; }
        public static LayerControllerWindow LayerCtrlWindow { get; private set; }

        public static GridScanner GridScanner { get; private set; }
        public static GridScannerWindow GridScannerWindow { get; private set; }

        static int selectedToolbarItem;
        static List<string> toolbarEntries = new List<string>();

        [MenuItem("Window/Master Grid _%g")]
        static void Init()
        {            
            MasterGrid masterGrid = (MasterGrid)GetWindow(typeof(MasterGrid));
            masterGrid.titleContent = new GUIContent("Master Grid");
            masterGrid.Show();

            GridCtrl = new GridController();
            GridVisualizer = new GridControllerVisualizer(GridCtrl);
            LayerCtrl = new LayerController(GridCtrl);
            GridScanner = new GridScanner();

            GridCtrl.Init(GridVisualizer, LayerCtrl);

            GridCtrlWindow = new GridControllerWindow(GridCtrl);
            GridVisualizerWindow = new GridVisualizerWindow(GridVisualizer, LayerCtrl);
            LayerCtrlWindow = new LayerControllerWindow(LayerCtrl);
            GridScannerWindow = new GridScannerWindow(GridScanner, GridCtrl);

            toolbarEntries.Add("Grid Controller");
            toolbarEntries.Add("Grid Visualizer");
            toolbarEntries.Add("Layer Controller");
            toolbarEntries.Add("Grid Scanner");

            SceneView.onSceneGUIDelegate += DrawCall;
        }

        private void OnGUI()
        {
            selectedToolbarItem = GUILayout.Toolbar(selectedToolbarItem, toolbarEntries.ToArray());
            switch (selectedToolbarItem)
            {
                case 0: // grid controller
                    GridCtrlWindow.Show();
                    break;
                case 1: // grid visualizer    
                    GridVisualizerWindow.Show();
                    break;
                case 2: // layer controller
                    LayerCtrlWindow.Show();
                    break;
                case 3: // grid scanner
                    GridScannerWindow.Show();
                    break;
            }
        }
       
        static void DrawCall(SceneView _sceneView)
        {
            GridVisualizer.DrawHandles();
        }

        private void OnDestroy()
        {
            SceneView.onSceneGUIDelegate -= DrawCall;
        }
    }
}

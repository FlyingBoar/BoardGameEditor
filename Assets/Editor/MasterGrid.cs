using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    [System.Serializable]
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

        [SerializeField]
        static int selectedToolbarItem;
        [SerializeField]
        static List<string> toolbarEntries = new List<string>();

        [MenuItem("Window/Master Grid _%g")]
        static void Init()
        {
            MasterGrid masterGrid = (MasterGrid)GetWindow(typeof(MasterGrid));
            masterGrid.titleContent = new GUIContent("Master Grid");
            masterGrid.minSize = new Vector2(600, 350);
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

            if(toolbarEntries.Count == 0)
            {
                toolbarEntries.Add("Grid Controller");
                toolbarEntries.Add("Grid Visualizer");
                toolbarEntries.Add("Layer Controller");
                toolbarEntries.Add("Grid Scanner");
            }

            SceneView.onSceneGUIDelegate += DrawCall;
            SceneView.onSceneGUIDelegate += MouseInteraction;
        }

        private void OnEnable()
        {
            //Workaround
            if(GridCtrl == null)
                Init();
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

        static void MouseInteraction(SceneView _sceneView)
        {
            if (_sceneView != SceneView.currentDrawingSceneView)
                return;
            if (Event.current.type == EventType.MouseDown)
            {
                GridVisualizer.SelectedCell = GridCtrl.GetCellFromPosition(GridInput.PointerPosition);

                if (Event.current.button == 1 && Event.current.control)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Select Cell"), false, GridCtrlWindow.SelectCell);
                    if (GridCtrlWindow.SavedCell != null)
                    {
                        menu.AddItem(new GUIContent("Link Cell"), false, GridCtrlWindow.LinkSelectedCell);
                        menu.AddItem(new GUIContent("Unlink Cell"), false, GridCtrlWindow.UnlinkSelectedCell);
                    }
                    menu.AddItem(new GUIContent("Deselect Cell"), false, GridCtrlWindow.DeselectCell);
                    menu.ShowAsContext();
                }
            }
        }

        private void OnDestroy()
        {
            SceneView.onSceneGUIDelegate -= DrawCall;
            SceneView.onSceneGUIDelegate -= MouseInteraction;
        }
    }
}

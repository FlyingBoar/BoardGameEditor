using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using Grid;

public static class DataManager {

    

    static GridData _gridData = new GridData();

    static GridData GridData {
        get { return _gridData; }
        set
        {
            _gridData = value;
            GridDataInstance = _gridData;
        }
    }
    
    private static GridData _dataInstance;

    public static GridData GridDataInstance
    {
        get
        {
            if (_dataInstance == null)
                _dataInstance = _gridData;
            return _dataInstance;
        }
        set { _dataInstance = value; }
    }

    public static void SaveNewData(string _dataName = null)
    {
        string assetName;
        if (_dataName == null)
            assetName = "NewGridData.json";
        else
            assetName = _dataName + ".json";

        string completePath = AssetDatabase.GenerateUniqueAssetPath(CheckFolder() + assetName);

        string newJsonData = JsonUtility.ToJson(GridDataInstance);

        File.WriteAllText(completePath, newJsonData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void SaveData(string _jsonGridDataPath)
    {
        string _jsonGridData = File.ReadAllText(_jsonGridDataPath);
        string newJsonData = JsonUtility.ToJson(GridDataInstance);

        if (_jsonGridData != newJsonData)
        {
            File.WriteAllText(_jsonGridDataPath, newJsonData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// Load the data .json from a path
    /// </summary>
    /// <param name="_jsonGridDataPath"></param>
    public static void LoadData(string _jsonGridDataPath)
    {
        string _jsonGridData = File.ReadAllText(_jsonGridDataPath);
        if (_jsonGridData == null)
        {
            Debug.LogWarning("GridController -- No data to load !");
            return;
        }
        GridData = JsonUtility.FromJson<GridData>(_jsonGridData);
    }

    /// <summary>
    /// Determina il percorso dell'asset che gli viene passato come paramentro e controlla se è del formato corretto
    /// </summary>
    /// <param name="_assetToLoad">Il file da cui ricavare il path</param>
    /// <returns></returns>
	public static string GetAssetPath(TextAsset _assetToLoad)
    {
        string assetPath = AssetDatabase.GetAssetPath(_assetToLoad);
        if (!assetPath.Contains(".json"))
        {
            Debug.LogWarning("File format not supported.");
            return null;
        }
        else
            return assetPath;
    }

    static string CheckFolder()
    {
#if UNITY_EDITOR
        if (!AssetDatabase.IsValidFolder("Assets/GridData"))
            AssetDatabase.CreateFolder("Assets", "GridData");
#endif

        return "Assets/GridData/";
    }
}

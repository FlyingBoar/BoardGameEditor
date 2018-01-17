using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataManager {

    /// <summary>
    /// Determina il percorso dell'asset che gli viene passato come paramentro e controlla se è del formato corretto
    /// </summary>
    /// <param name="_assetToLoad">Il file da cui ricavare il path</param>
    /// <returns></returns>
	public string GetAssetPath(TextAsset _assetToLoad)
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
}

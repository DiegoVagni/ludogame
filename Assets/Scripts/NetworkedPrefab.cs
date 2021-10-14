using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedPrefab
{
    private GameObject prefab;
    private string path;

    public NetworkedPrefab(GameObject obj, string path) {
        prefab = obj;
        this.path = ReturnPrefabPathModified(path);
    }

    public GameObject GetPrefab()
	{
        return prefab;
	}
    private string ReturnPrefabPathModified(string path) {
        int extensionLength = System.IO.Path.GetExtension(path).Length;
        int startIndex = path.ToLower().IndexOf("resources");
        //lunghezza della cartella resources che va tagliata
        int additionalLenght = 10;
        if (startIndex == -1)
        {
            return string.Empty;
        }
        else {
            return path.Substring(startIndex+additionalLenght, path.Length - (startIndex + extensionLength+additionalLenght));
        }
    }
    public string GetPath() {
        return path;
    }
}

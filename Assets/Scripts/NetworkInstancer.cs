using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif 
using UnityEngine;

public class NetworkInstancer 
{
	[SerializeField]
	private List<NetworkedPrefab> networkedPrefabs;
	private static NetworkInstancer instance;
	private NetworkInstancer() {
		networkedPrefabs = new List<NetworkedPrefab>();
	}
	public static NetworkInstancer GetInstance() {
		if (instance == null) {
			instance = new NetworkInstancer();
		}
		return instance;
	}
	public static GameObject NetworkInstantiate(GameObject obj, Vector3 pos, Quaternion rot) {
		GetInstance();
		foreach (NetworkedPrefab networkedPrefab in instance.networkedPrefabs) {
			if (networkedPrefab.GetPrefab() == obj)
			{
				GameObject result = PhotonNetwork.Instantiate(networkedPrefab.GetPath(), pos, rot);
				return result;
			}
			else {
				Debug.Log(networkedPrefab.GetPrefab().name);
			}
		}
		Debug.LogError("not found " + obj.name);
		return null;
	}
	public List<NetworkedPrefab> GetPrefabs() {
		return networkedPrefabs;
	}

	internal static void Populate()
	{
#if UNITY_EDITOR
		GetInstance().networkedPrefabs.Clear();
		GameObject[] results = Resources.LoadAll<GameObject>("");
		for (int i = 0; i < results.Length; i++) {
			if (results[i].GetComponent<PhotonView>() != null) {
				string path = AssetDatabase.GetAssetPath(results[i]);
				GetInstance().networkedPrefabs.Add(new NetworkedPrefab(results[i],path));
				Debug.LogError(GetInstance().networkedPrefabs[GetInstance().networkedPrefabs.Count - 1].GetPath());
			}
		}
#endif
	}
}

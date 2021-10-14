using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Singletons/MasterManager")]
public class MasterManager : ScriptableObject
{
	[SerializeField]
	private GameSettings gameSettings;

	public GameSettings GetGameSettings() {
		return gameSettings;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private string gameVersion = "0.0.0";
    [SerializeField]
    private string nickName = "Zeb";
    public string GetGameVersion() {
        return gameVersion;
    }

    public string GetNickname() {
        int value = Random.Range(0, 999);
        return nickName + value.ToString();
    }
}

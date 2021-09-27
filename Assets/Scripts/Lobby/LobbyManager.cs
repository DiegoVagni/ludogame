using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text textPrefab;
    [SerializeField]
    private Transform content;
    private List<Text> texts;
    void Start()
    {
        texts = new List<Text>();
        UpdatePlayers();
    }
    private void UpdatePlayers() {
        foreach (Text t in texts) {
            
            Destroy(t.gameObject);
        }
        texts.Clear();
        Vector3 position = new Vector3(100, 500, 0);
     foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList) {
            Text t = Instantiate(textPrefab, position, Quaternion.identity);
            t.text = p.NickName;
            t.transform.SetParent(content);
            position += new Vector3(0, -30, 0);
            texts.Add(t);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 60 == 0) {
            UpdatePlayers();
        }
    }
}

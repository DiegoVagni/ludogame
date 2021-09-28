using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using static UnityEngine.UI.Dropdown;

public class ServerConnecter : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField createRoomName;
    [SerializeField]
    private Dropdown dropDownRoomName;
    [SerializeField]
    private byte maxPlayers = 4;
    [SerializeField]
    private InputField playerName;
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateRoomButtonCallback()
	{
        if (playerName.text != "")
        {

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayers;
            PhotonNetwork.LocalPlayer.NickName = playerName.text;
            PhotonNetwork.CreateRoom(createRoomName.text, roomOptions, null);
        }else {
            Debug.LogError("name can't be empty");
        }
    }

    public void JoinRoomButtonCallback()
    {
        if (playerName.text != "") {
            PhotonNetwork.LocalPlayer.NickName = playerName.text;
            PhotonNetwork.JoinRoom(dropDownRoomName.options[dropDownRoomName.value].text);
        }else {
            Debug.LogError("name can't be empty");
        }
    }
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            Debug.Log(roomList[i]);
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
        dropDownRoomName.ClearOptions();
        List<OptionData> options = new List<OptionData>();
        foreach (KeyValuePair<string, RoomInfo> entry in cachedRoomList) {
            OptionData data = new OptionData(entry.Key);
            options.Add(data);
        }
        dropDownRoomName.AddOptions(options);
    }

    #region PunCallbacks
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
    #endregion


}

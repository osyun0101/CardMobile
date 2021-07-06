using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1.0";
        if (!PhotonNetwork.IsConnected)
        {
            // ここでPhotonに接続している
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //マスターサーバーへの接続が成功した時に呼ばれる
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーに接続しました");
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoomMove()
    {
        SceneManager.LoadScene("RoomCreateName");
    }

    public void SelectRoomMove()
    {
        SceneManager.LoadScene("SelectRoom");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("ロビー入室完了");
    }
}

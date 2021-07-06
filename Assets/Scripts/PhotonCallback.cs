using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonCallback : MonoBehaviourPunCallbacks
{
    /*
    //マスターサーバーへの接続が成功した時に呼ばれる
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーに接続しました");
        PhotonNetwork.JoinLobby();
    }

    //ルームが作成された時に呼ばれる
    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
    }

    // コールバック：ルームに入室した時
    public override void OnJoinedRoom()
    {
        Debug.Log("ルームに入室しました");
    }

    // ルームリストに更新があった時
    //ただ、ルームを取得するにはロビーに入ってなければいけないらしい
    //pun2では必要なければロビーに入らずルームに入ることを推奨しているがルームを取得したい時はロビーに入る必要がある
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
    }*/
}

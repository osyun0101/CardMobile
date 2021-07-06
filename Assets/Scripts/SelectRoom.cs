using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class SelectRoom : MonoBehaviourPunCallbacks
{
    //public Text text;
    public TextMeshProUGUI text;

    public void InRoom()
    {
        Debug.Log("Try: InRoom");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("ロビー入室完了");
        DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.JoinRoom(text.text);
    }

    // コールバック：ルームに入室した時
    public override void OnJoinedRoom()
    {
        Debug.Log("SelectRoomからルームに入室しました");
    }

    // ルーム入室に失敗した時
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("ルームに参加できませんでした");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("ランダムに入室できませんでした");
    }
}

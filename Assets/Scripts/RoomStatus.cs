using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using TMPro;

public class RoomStatus : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI RoomName;
    public Text num;
    public GameObject Canvas;

    void Awake()
    {
        //部屋が作成された場合
        var obj1 = GameObject.Find("InputFieldEvent");
        if(obj1 != null)
        {
            RoomName.text = obj1.GetComponent<InputFieldEvent>().RoomNameText;
        }
        //部屋に参加した場合
        var obj2 = GameObject.Find("SelectRoom");
        if(obj2 != null)
        {
            RoomName.text = obj2.GetComponent<SelectRoom>().text.text;
        }
    }

    //ルームが作成された時に呼ばれる
    public override void OnCreatedRoom()
    {
        Debug.Log("ルーム作成RoomStatus");
        //マスタークライアントの時、Startボタンをアクティブにする
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            var StartButton = Canvas.transform.Find("Start").gameObject;
            StartButton.SetActive(true);
        }
        var PlayersCount = PhotonNetwork.CountOfPlayersInRooms + 1;
        num.text = PlayersCount.ToString();
        CreatePanel(PhotonNetwork.NickName);
    }

    // コールバック：ルームに入室した時
    public override void OnJoinedRoom()
    {
        var defaultCount = PhotonNetwork.CountOfPlayersInRooms;
        var PlayersCount = defaultCount + 1;
        num.text = PlayersCount.ToString();
        Debug.Log("ルームに入室しましたRoomStatus");
    }

    // 他のプレイヤーが入室してきた時
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("新たなプレイヤーが参加しました");
        PlayerCountFunc();
        CreatePanel(newPlayer.NickName);
    }

    public void PlayerCountFunc()
    {
        var Canvas = GameObject.Find("Canvas");
        var MemberNum = Canvas.transform.Find("MemberNum").gameObject;
        var PlayerCount = MemberNum.GetComponent<Text>().text;
        int count;
        int.TryParse(PlayerCount, out count);
        count += 1;
        MemberNum.GetComponent<Text>().text = count.ToString();
    }

    public void CreatePanel(string Name)
    {
        var panel = PhotonNetwork.InstantiateRoomObject("Panel", new Vector3(0, 0, 0), Quaternion.identity);
        if(panel != null)
        {
            panel.GetComponent<PhotonView>().RPC("RPCFunc", RpcTarget.AllBuffered, Name);
        }
    }
}

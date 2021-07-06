using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Photon.Realtime;
using Photon.Pun;

public class InputFieldEvent : MonoBehaviourPunCallbacks
{
    public  TextMeshProUGUI InputText;
    public string RoomNameText;
    public GameObject ErrorText;

    public void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            // ここでPhotonに接続している
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void RoomName()
    {
        
        // 部屋としてボタンを作成していた時の処理
        if (InputText.text.Length > 1)
        {
            // ルームオプションの基本設定
            RoomOptions roomOptions = new RoomOptions
            {
                // 部屋の最大人数
                MaxPlayers = 10,

                // 公開
                IsVisible = true,

                // 入室可
                IsOpen = true
            };

            // ルームオプションにカスタムプロパティを設定
            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "Stage", 1 }
            };
            roomOptions.CustomRoomProperties = customRoomProperties;

            // ロビーに公開するカスタムプロパティを指定
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "Stage" };

            // 部屋を作成して入室する
            var boolRoom = PhotonNetwork.CreateRoom(InputText.text, roomOptions);
            
            if (boolRoom)
            {
                this.RoomNameText = InputText.text;
                DontDestroyOnLoad(this.gameObject);
                //SceneManager.LoadSceneAsync("RoomPlayerIndex");
                PhotonNetwork.LoadLevel("RoomPlayerIndex");
            }
        }
        else
        {
            ErrorText.SetActive(true);
        }
    }

    //マスターサーバーへの接続が成功した時に呼ばれる
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーに接続しました");
        //PhotonNetwork.JoinLobby();
    }

    // コールバック：ロビー入室完了
    public override void OnJoinedLobby()
    {
        Debug.Log("ロビー入室完了");
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
        //今いるルームを取得
        var thisroom = PhotonNetwork.CurrentRoom;
        Debug.Log(thisroom.Name);
    }

    // ルームリストに更新があった時
    //ただ、ルームを取得するにはロビーに入ってなければいけないらしい
    //pun2では必要なければロビーに入らずルームに入ることを推奨しているがルームを取得したい時はロビーに入る必要がある
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
    }

    // 部屋から退室した時
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }
}

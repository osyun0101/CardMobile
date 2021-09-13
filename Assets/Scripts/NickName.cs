using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;

public class NickName : MonoBehaviourPunCallbacks
{
    public Text text;
    public GameObject ErrorText;

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

    public override void OnJoinedLobby()
    {
        Debug.Log("ロビー入室完了");
    }

    //登録ボタンが押された時、ニックネーム登録
    public void SetName()
    {
        if (!string.IsNullOrEmpty(text.text))
        {
            PhotonNetwork.NickName = text.text;
            SceneManager.LoadScene("CreateOrRoomIn");
        }
        else
        {
            ErrorText.GetComponent<Text>().text = "ニックネームを入力して下さい";
            ErrorText.SetActive(true);
        }
    }
}

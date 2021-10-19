using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class TekashiNextManager : MonoBehaviour, IOnEventCallback
{
    public GameObject Canvas;
    public static GameObject ScrollView;
    private void Awake()
    {
        ScrollView = Canvas.transform.Find("ScrollView").gameObject;
        // マスタークライアントしかネットワークオブジェクトを生成できないから
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            var screenWidth = Screen.width;
            //パネルの間隔割る値を大きくする程間隔が狭くなってパネルの横幅が大きくなる
            float widthCountUp = (screenWidth - 40) / 80;
            //パネル一つあたりの横幅
            float width = (screenWidth - 40 - (widthCountUp * 4)) / 5;
            //一番最初のパネルのx座標
            float posx = -((screenWidth - 40) / 2 - width / 2);
            float posy = 100.0f;
            var playerList = PhotonNetwork.PlayerList;
            for (var i = 0; i < playerList.Length; i++)
            {
                if (i != 0 && i % 5 == 0)
                {
                    posx = -((screenWidth - 40) / 2 - width / 2);
                    posy -= 220.0f;
                }
                var ReleaseCountPanel = PhotonNetwork.InstantiateRoomObject("ReleaseCountPanel", new Vector3(0, 0, 0), Quaternion.identity).transform;
                ReleaseCountPanel.GetComponent<PhotonView>().RPC("SetPosition", RpcTarget.All, posx, posy, width, playerList[i].NickName);
                //+の値にしてposxを更新
                posx += width + widthCountUp;
            }
        }
    }

    public enum EEventType : byte
    {
        Release = 1
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)EEventType.Release)
        {
            var dic = photonEvent.CustomData as Dictionary<string, int>;
            var panel = GameObject.FindGameObjectsWithTag("CountPanel")[dic["index"]].transform.Find("Panel");
            panel.GetComponent<CountPanelAction>().HandCount = dic["count"];
            var anim = panel.GetComponent<Animator>();
            anim.SetBool("Release", true);
        }
    }
}
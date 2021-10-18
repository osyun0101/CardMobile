using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class TekashiNextManager : MonoBehaviour
{
    public GameObject ScrollView;
    private void Awake()
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
            if(i != 0 && i % 5 == 0)
            {
                posx = -((screenWidth - 40) / 2 - width / 2);
                posy -= 220.0f;
            }
            var ReleaseCountPanel = PhotonNetwork.InstantiateRoomObject("ReleaseCountPanel", new Vector3(0, 0, 0), Quaternion.identity).transform;
            ReleaseCountPanel.SetParent(ScrollView.transform);
            ReleaseCountPanel.localPosition = new Vector3(posx, posy, 0);
            ReleaseCountPanel.localScale = new Vector3(1, 1, 1);
            ReleaseCountPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 160.0f);
            var PlayerNameObj = ReleaseCountPanel.Find("PlayerName");
            PlayerNameObj.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 50.0f);
            PlayerNameObj.GetComponent<TextMeshProUGUI>().text = playerList[i].NickName;
            ReleaseCountPanel.Find("Panel").GetComponent<CountPanelAction>().HandCount = 20;

            //+の値にしてposxを更新
            posx += width + widthCountUp;
        }
    }
}

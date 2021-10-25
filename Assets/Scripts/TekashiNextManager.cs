using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Linq;
using System.Threading.Tasks;

public class TekashiNextManager : MonoBehaviour, IOnEventCallback
{
    public GameObject Canvas;
    public GameObject TekashiPlayerName;
    public static GameObject ScrollView;
    public static int TekashiPlayerId;
    public static List<Dictionary<string, object>> ReleaseList = new List<Dictionary<string, object>>();
    public GameObject ModalPanel;
    public GameObject ModalBackPanel;
    public GameObject LoserTitle;
    public GameObject DeltaTitle;
    public GameObject LoserPlayerPanel;
    public GameObject DeltaCountPanel;
    public GameObject LoserPlayerName;
    public GameObject DeltaText;
    private void Awake()
    {
        var tkPlayerText = TekashiPlayerName.GetComponent<TextMeshProUGUI>().text;
        TekashiPlayerName.GetComponent<TextMeshProUGUI>().text = tkPlayerText.Replace("$name",PhotonNetwork.PlayerList[TekashiPlayerId - 1].NickName);
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

            var listDic = new Dictionary<string, object>();
            var tekashiPlayerName = PhotonNetwork.PlayerList[dic["index"]].NickName;
            listDic.Add("name", tekashiPlayerName);
            listDic.Add("count", dic["count"]);
            ReleaseList.Add(listDic);
            if(ReleaseList.Count == PhotonNetwork.PlayerList.Length)
            {
                Delay();
            }
        }
    }

    private async void Delay()
    {
        await Task.Delay(3000);
        ModalPanel.SetActive(true);
        ModalBackPanel.SetActive(true);
        LoserTitle.SetActive(true);
        DeltaTitle.SetActive(true);
        DeltaCountPanel.SetActive(true);
        LoserPlayerPanel.SetActive(true);
        //Countが最も大きいプレイヤーの情報を取得
        int max = 0;
        var maxPlayerIndex = new List<int>();
        for(var i = 0; i <  ReleaseList.Count; i++)
        {
            int.TryParse(ReleaseList[i]["count"].ToString(), out int count);
            if(max == count)
            {
                maxPlayerIndex.Add(i);
                continue;
            }
            if (max < count)
            {
                max = count;
                maxPlayerIndex.Clear();
                maxPlayerIndex.Add(i);
            }
        }
        
        //LoserPlayerName.GetComponent<TextMeshProUGUI>().text = ReleaseList.Where(x => x["count"]  )

        var width = Screen.width * 0.75f;
        var height = Screen.height * 0.75f;
        ModalBackPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        var anim = ModalPanel.GetComponent<Animator>();
        anim.SetBool("ModalOpen", true);
    }
}
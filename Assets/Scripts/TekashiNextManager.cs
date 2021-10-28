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
            listDic.Add("playerId", dic["index"] + 1);
            ReleaseList.Add(listDic);
            Debug.Log(ReleaseList.Count);
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
        //Countが最も大きいプレイヤーの情報を取得
        int max = 0;
        int min = 100;
        var maxPlayerId = new List<int>();
        foreach(var dic in ReleaseList)
        {
            int.TryParse(dic["count"].ToString(), out int count);
            if(max == count)
            {
                maxPlayerId.Add((int)dic["playerId"]);
                continue;
            }
            if (max < count)
            {
                max = count;
                maxPlayerId.Clear();
                maxPlayerId.Add((int)dic["playerId"]);
            }
            if(min > count)
            {
                min = count;
            }
        }

        //テカシしたプレイヤーよりカウントが低いプレイヤーがいた時、テカシしたプレイヤーを負けにする
        var tekashiCount = (int)ReleaseList.Where(x => (int)x["playerId"] == TekashiPlayerId).FirstOrDefault()["count"];
        var lowTekashiCountList = ReleaseList.Where(x => (int)x["count"] <= tekashiCount).ToList();
        if(lowTekashiCountList.Count != 1)
        {
            maxPlayerId.Clear();
            maxPlayerId.Add(TekashiPlayerId);
        }

        var instanceList = new List<Animator>();
        var posy = 130f;
        foreach (var id in maxPlayerId)
        {
            var obj = (GameObject)Resources.Load("LoserPlayerPanel");
            // プレハブを元にオブジェクトを生成する
            GameObject instance = Instantiate(obj, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            instance.transform.SetParent(ModalBackPanel.transform);
            instance.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            instance.transform.localPosition = new Vector3(0, posy, 0);
            instance.transform.Find("LoserPlayerName").GetComponent<TextMeshProUGUI>().text = ReleaseList.Where(x => (int)x["playerId"] == id).FirstOrDefault()["name"].ToString();
            instance.SetActive(true);
            instanceList.Add(instance.GetComponent<Animator>());
            posy -= 90;
        }

        //差分のタイトルのオブジェクトを生成する、posyの値が複数負けたプレイヤーがいると変わるから
        var deltaTitle = (GameObject)Resources.Load("DeltaTitle");
        // プレハブを元にオブジェクトを生成する
        GameObject deltaTitleObj = Instantiate(deltaTitle, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        deltaTitleObj.transform.SetParent(ModalBackPanel.transform);
        deltaTitleObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        deltaTitleObj.transform.localPosition = new Vector3(0, posy, 0);
        deltaTitleObj.SetActive(true);
        var deltaTitleAnim = deltaTitleObj.GetComponent<Animator>();
        posy -= 90;

        //差分のカウントオブジェクト生成
        var deltaCount = (GameObject)Resources.Load("DeltaCountPanel");
        GameObject deltaCountObj = Instantiate(deltaCount, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        deltaCountObj.transform.SetParent(ModalBackPanel.transform);
        deltaCountObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        deltaCountObj.transform.localPosition = new Vector3(0, posy, 0);
        deltaCountObj.transform.Find("DeltaText").GetComponent<TextMeshProUGUI>().text = (max - min).ToString();
        deltaCountObj.SetActive(true);
        var deltaCountAnim = deltaCountObj.GetComponent<Animator>();
        posy -= 90;

        var width = Screen.width * 0.75f;
        var height = Screen.height * 0.75f;
        ModalBackPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        var anim = ModalPanel.GetComponent<Animator>();
        anim.SetBool("ModalOpen", true);
        foreach(var animator in instanceList)
        {
            animator.SetBool("LoserPanelOpen", true);
        }
        deltaTitleAnim.SetBool("DeltaTitleOpen", true);
        deltaCountAnim.SetBool("DeltaCountOpen", true);
    }
}
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TekashiNextManager : MonoBehaviour, IOnEventCallback
{
    public GameObject Canvas;
    public GameObject TekashiPlayerName;
    public static GameObject ScrollView;
    public static int TekashiPlayerId;
    public static List<Dictionary<string, object>> ReleaseList = new List<Dictionary<string, object>>();
    public GameObject ModalPanel;
    public GameObject ModalBackPanel;
    public GameObject ModalScrollViewContent;
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
        var modalScrollViewHeight = 200f;
        var width = Screen.width * 0.75f;
        var height = Screen.height * 0.75f;
        ModalBackPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        ModalScrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, height);
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
        var posy = -130f;
        foreach (var id in maxPlayerId)
        {
            var obj = (GameObject)Resources.Load("LoserPlayerPanel");
            // プレハブを元にオブジェクトを生成する
            var instance = SetInstance(obj, posy, 30f, -30f);
            instance.transform.Find("LoserPlayerName").GetComponent<TextMeshProUGUI>().text = ReleaseList.Where(x => (int)x["playerId"] == id).FirstOrDefault()["name"].ToString();
            instanceList.Add(instance.GetComponent<Animator>());
            modalScrollViewHeight += instance.GetComponent<RectTransform>().sizeDelta.y + 10;
            posy += -90;
        }
        
        //差分のタイトルのオブジェクトを生成する、posyの値が複数負けたプレイヤーがいると変わるから
        var deltaTitle = (GameObject)Resources.Load("DeltaTitle");
        // プレハブを元にオブジェクトを生成する
        var deltaTitleObj = SetInstance(deltaTitle, posy, 30f, -30f);
        var deltaTitleAnim = deltaTitleObj.GetComponent<Animator>();
        modalScrollViewHeight += deltaTitleObj.GetComponent<RectTransform>().sizeDelta.y;
        posy += -90;

        //差分のカウントオブジェクト生成
        var deltaCount = (GameObject)Resources.Load("DeltaCountPanel");
        var deltaCountObj = SetInstance(deltaCount, posy, 30f, -30f);
        modalScrollViewHeight += deltaCountObj.GetComponent<RectTransform>().sizeDelta.y;
        deltaCountObj.transform.Find("DeltaText").GetComponent<TextMeshProUGUI>().text = (max - min).ToString();
        var deltaCountAnim = deltaCountObj.GetComponent<Animator>();
        posy += -90;

        //もう一度プレイするボタンの表示
        var replayButton = (GameObject)Resources.Load("ReplayButton");
        var replayButtonObj = SetInstance(replayButton, posy, 100f, -100f);
        modalScrollViewHeight += replayButtonObj.GetComponent<RectTransform>().sizeDelta.y;
        var replayButtonAnim = replayButtonObj.GetComponent<Animator>();
        posy += -90;

        //Topに戻るボタンの表示
        var topRedirectButton = (GameObject)Resources.Load("TopRedirectButton");
        var topRedirectButtonObj = SetInstance(topRedirectButton, posy, 100f, -100f);
        modalScrollViewHeight += topRedirectButtonObj.GetComponent<RectTransform>().sizeDelta.y;
        var topRedirectButtonAnim = topRedirectButtonObj.GetComponent<Animator>();
        posy += -90;

        //結果一覧
        //DeltaTitleだけど使えそうだからこれでいいや的な感じ
        var releaseIndex = (GameObject)Resources.Load("DeltaTitle");
        releaseIndex.GetComponent<TextMeshProUGUI>().text = "結果一覧";
        // プレハブを元にオブジェクトを生成する
        var releaseIndexObj = SetInstance(deltaTitle, posy, 30f, -30f);
        releaseIndexObj.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1.0f);
        var releaseAnim = releaseIndexObj.GetComponent<Animator>();
        modalScrollViewHeight += releaseIndexObj.GetComponent<RectTransform>().sizeDelta.y;
        posy += -90;

        //公開された手札のリストをソート
        var sortList = ReleaseList.OrderBy(x => x["count"]);
        var rankNum = 0;
        var prevCount = 0;
        var rankObjAnim = new List<Animator>();
        foreach(var releaseDic in sortList)
        {
            if (prevCount < (int)releaseDic["count"])
            {
                rankNum += 1;
                prevCount = (int)releaseDic["count"];
            }
            //順位
            var rank = (GameObject)Resources.Load("DeltaTitle");
            rank.GetComponent<TextMeshProUGUI>().text = rankNum.ToString();
            rank.GetComponent<TextMeshProUGUI>().fontSize = 60f;
            var rankObj = SetInstance(rank, posy, 30f, -(width * 0.9f));
            rankObj.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1.0f);
            var rankObjWidth = rankObj.GetComponent<RectTransform>().rect.width;
            rankObjAnim.Add(rankObj.GetComponent<Animator>());
            //プレイヤー名
            var obj = (GameObject)Resources.Load("ReleasePlayerIndexPanel");
            // プレハブを元にオブジェクトを生成する
            var instance = SetInstance(obj, posy, 31f + rankObjWidth, -(width * 0.3f));
            instance.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = releaseDic["name"].ToString();
            instanceList.Add(instance.GetComponent<Animator>());
            //カウントパネル
            var releaseCountPanel = (GameObject)Resources.Load("ReleasePlayerIndexPanel");
            // プレハブを元にオブジェクトを生成する
            var instance2 = SetInstance(releaseCountPanel, posy, (width * 0.71f), -30f);
            instance2.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = releaseDic["count"].ToString();
            instanceList.Add(instance2.GetComponent<Animator>());

            modalScrollViewHeight += instance.GetComponent<RectTransform>().sizeDelta.y + 10;
            posy += -90;
        }

        if(height < modalScrollViewHeight)
        {
            ModalScrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, modalScrollViewHeight);
        }
        
        //各モーダル上のオブジェクトのアニメーション実行
        var anim = ModalPanel.GetComponent<Animator>();
        anim.SetBool("ModalOpen", true);
        foreach(var animator in instanceList)
        {
            animator.SetBool("LoserPanelOpen", true);
        }
        foreach(var animator in rankObjAnim)
        {
            animator.SetBool("DeltaTitleOpen", true);
        }
        deltaTitleAnim.SetBool("DeltaTitleOpen", true);
        deltaCountAnim.SetBool("DeltaCountOpen", true);
        replayButtonAnim.SetBool("ReplayButtonOpen", true);
        topRedirectButtonAnim.SetBool("TopRedirectButtonOpen", true);
        releaseAnim.SetBool("DeltaTitleOpen", true);
    }

    public GameObject SetInstance(GameObject loadObject, float posy, float left, float right)
    {
        GameObject obj = Instantiate(loadObject, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        obj.transform.SetParent(ModalScrollViewContent.transform);
        obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        var height = obj.GetComponent<RectTransform>().sizeDelta.y;
        obj.GetComponent<RectTransform>().offsetMin = new Vector2(left, 0f);
        obj.GetComponent<RectTransform>().offsetMax = new Vector2(right, 0f);
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(obj.GetComponent<RectTransform>().sizeDelta.x, height);
        var x = obj.GetComponent<RectTransform>().localPosition.x;
        obj.GetComponent<RectTransform>().localPosition= new Vector3(x, posy, 0f);
        obj.SetActive(true);
        return obj;
    }
}
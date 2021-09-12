using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static List<string> cards = new List<string>();
    public static GameObject[] PlayerHands = new GameObject[] { };
    public static int PlayerActorNumber = 0;
    public GameObject Canvas;
    string mark;

    private void Awake()
    {
        Debug.Log("PlayRoomに遷移");
        
        // マスタークライアントの時
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //誰のターンか示すテキスト
            var parent = GameObject.Find("Canvas").transform.Find("SelfHandPanel").Find("YouturnImage");
            var turnText = PhotonNetwork.InstantiateRoomObject("TurnText", new Vector3(0, 0, 0), Quaternion.identity);
            turnText.GetComponent<PhotonView>().RPC("SetText", RpcTarget.All, PhotonNetwork.NickName);
            turnText.GetComponent<TextMeshProUGUI>().text = "あなたの番です";
            parent.GetComponent<Image>().color = new Color(226.0f/ 255.0f, 85.0f / 255.0f, 80.0f / 255.0f, 1f);

            var HandsSetobj = PhotonNetwork.InstantiateRoomObject("HandsSet", new Vector3(0, 0, 0), Quaternion.identity);
            int num = 1;
            for (int s = 0; s < 13; s++)
            {
                for (int i = 0; i < 4; i++)
                {
                    
                    switch (i)
                    {
                        case 0:
                            mark = "Heart";
                            break;
                        case 1:
                            mark = "Club";
                            break;
                        case 2:
                            mark = "Diamond";
                            break;
                        case 3:
                            mark = "Spade";
                            break;
                    }
                    if(num < 10)
                    {
                        cards.Add(mark + 0 + num.ToString());
                    }
                    else
                    {
                        cards.Add(mark + num.ToString());
                    }
                }
                num += 1;
            }
            cards.Add("Joker_Color");
            cards.Add("Joker_Color");

            var PlayerList = PhotonNetwork.PlayerList;

            foreach (var player in PlayerList)
            {
                List<string> dataList = new List<string>();
                for (int i = 0; i < 5; i++)
                {
                    var random = Random.Range(0, cards.Count);
                    dataList.Add(cards[random]);
                    cards.RemoveAt(random);
                }
                HandsSetobj.GetComponent<PhotonView>().RPC("SetHand", RpcTarget.All, player.ActorNumber, dataList.ToArray());
                var playerPanel = PhotonNetwork.InstantiateRoomObject("PlayerHandPanel", new Vector3(0, 0, 0), Quaternion.identity);
                playerPanel.GetComponent<PhotonView>().RPC("SetPlayerModel", RpcTarget.All, dataList.ToArray(), player.ActorNumber, PlayerList.Length);
            }

            //開始時に一枚場に捨てて置く時の処理
            var r = Random.Range(0, cards.Count);
            var SubmitCard = cards[r];
            cards.RemoveAt(r);
            var SubmitImage = PhotonNetwork.InstantiateRoomObject("SubmitImage", new Vector3(-0, 0, 0), Quaternion.identity);
            SubmitImage.GetComponent<PhotonView>().RPC("SetSubmitImage", RpcTarget.All, SubmitCard);
            SetDeckCount();
        }
    }

    //Rpcで生成したネットワークオブジェクトから実行
    public static void GetAllPlayerHand()
    {
        PlayerHands = GameObject.FindGameObjectsWithTag("Player");
    }

    public static void SetDeckCount()
    {
        //残り枚数を表示する処理
        var cardsResidue = PhotonNetwork.InstantiateRoomObject("CardsResidue", new Vector3(0, 0, 0), Quaternion.identity);
        cardsResidue.GetComponent<PhotonView>().RPC("cardSet", RpcTarget.All, cards.Count);
    }
}

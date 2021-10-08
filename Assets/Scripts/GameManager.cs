using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static List<string> cards = new List<string>();
    public static GameObject[] PlayerHands = new GameObject[] { };
    public static int PlayerActorNumber = 0;
    public GameObject Canvas;
    string mark;
    public GameObject TekashiAlertPanel;
    public GameObject TekashiButtom;

    private void Awake()
    {
        Debug.Log("PlayRoomに遷移");
        // マスタークライアントの時
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //TekashiButtomアクティブ
            TekashiButtom.SetActive(true);
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
                playerPanel.name = "PlayerHandPanel" + player.ActorNumber.ToString();
                playerPanel.GetComponent<PhotonView>().RPC("SetPlayerModel", RpcTarget.All, dataList.ToArray(), player.ActorNumber, PlayerList.Length);
            }

            //開始時に一枚場に捨てて置く時の処理
            var r = Random.Range(0, cards.Count);
            var SubmitCard = cards[r];
            cards.RemoveAt(r);

            //全てのプレイヤーで山札を共有する
            var option = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All,
            };
            PhotonNetwork.RaiseEvent((byte)EEventType.deckSet, cards.ToArray(), option, SendOptions.SendReliable);

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

    //次のプレイヤーにターンを移す処理
    public static void NextTurn(GameObject canvas, int ActorNumber)
    {
        var SelectCardText = canvas.transform.Find("SelfHandPanel").transform.Find("SelectCardText");
        SelectCardText.GetComponent<TextMeshProUGUI>().text = "";
        var outPutButton = canvas.transform.Find("Button");
        outPutButton.gameObject.SetActive(false);
        var YouturnImage = GameObject.Find("Canvas").transform.Find("SelfHandPanel").Find("YouturnImage");
        var turnText = YouturnImage.Find("TurnText(Clone)");
        var PlayerList = PhotonNetwork.PlayerList;
        var nextPlayer = PlayerList.Where(x => x.ActorNumber == ActorNumber + 1).FirstOrDefault();
        if(nextPlayer == null)
        {
            nextPlayer = PlayerList[0];
        }
        turnText.GetComponent<PhotonView>().RPC("SetText", RpcTarget.All, nextPlayer.NickName);

        var option = new RaiseEventOptions()
        {
            TargetActors = new int[] { nextPlayer.ActorNumber },
            Receivers = ReceiverGroup.All,
        };
        PhotonNetwork.RaiseEvent((byte)EEventType.Turn, "neko", option, SendOptions.SendReliable);
        var handsCountOption = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.All,
        };
        var actorAndHandCount = new int[] { ActorNumber, SelectHandManager.PlayerHands.Count };
        PhotonNetwork.RaiseEvent((byte)EEventType.handsCount, actorAndHandCount, handsCountOption, SendOptions.SendReliable);
        YouturnImage.GetComponent<Image>().color = new Color(0f, 100.0f / 255.0f, 255.0f / 255.0f, 1f);
    }

    //引いてきたカードを手札に設置する処理
    public static void SetSelfHand(List<HandCardScript> selectCards)
    {
        //SelfHandPanelにある手札を削除
        foreach (var cardobj in selectCards)
        {
            Destroy(cardobj.gameObject);
        }
        //手札のソート
        var posx = -250;
        List<GameObject> newList = new List<GameObject>();
        foreach (var hand in SelectHandManager.PlayerHands)
        {
            if (selectCards.Contains(hand.GetComponent<HandCardScript>()))
            {
                continue;
            }
            hand.transform.localPosition = new Vector3(posx, 0, 0);
            posx += 125;
            newList.Add(hand);
        }
        SelectHandManager.PlayerHands = newList;
    }

    public enum EEventType : byte
    {
        Turn = 1,
        cardStage = 2,
        cardCount = 3,
        deckSet = 4,
        tekashiFadeIn = 5,
        handsCount = 6
    }

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == (byte)EEventType.Turn)
        {
            var YouturnImage = Canvas.transform.Find("SelfHandPanel").Find("YouturnImage");
            var turnText = YouturnImage.Find("TurnText(Clone)");
            YouturnImage.GetComponent<Image>().color = new Color(226.0f / 255.0f, 85.0f / 255.0f, 80.0f / 255.0f, 1f);
            turnText.GetComponent<TextMeshProUGUI>().text = "あなたの番です";
            var buttom = Canvas.transform.Find("Button");
            buttom.gameObject.SetActive(true);
            var selectCardText = Canvas.transform.Find("SelfHandPanel").Find("SelectCardText");
            selectCardText.GetComponent<TextMeshProUGUI>().text = "捨てるカードを選択してください";
            selectCardText.gameObject.SetActive(true);
            TekashiButtom.SetActive(true);
        }
        else if(photonEvent.Code == (byte)EEventType.cardStage)
        {
            var submitImagePanel = Canvas.transform.Find("SubmitImage(Clone)").gameObject;
            var cardName = photonEvent.CustomData.ToString();
            var path = $"Playing_Cards/Image/PlayingCards/{cardName}";
            var card = Resources.Load<Sprite>(path);
            //山札からカードを引いた後、場に出す処理
            submitImagePanel.SetActive(true);
            submitImagePanel.GetComponent<RawImage>().texture = card.texture;
            submitImagePanel.GetComponent<SubmitImage>().CardName = cardName;
        }
        else if(photonEvent.Code == (byte)EEventType.cardCount)
        {
            int num;
            var cardResidue = Canvas.transform.Find("Image").Find("CardsResidue(Clone)");
            var cardCount = cardResidue.GetComponent<Text>().text;
            int.TryParse(cardCount, out num);
            cardResidue.GetComponent<Text>().text = (num - 1).ToString();
        }
        else if(photonEvent.Code == (byte)EEventType.deckSet)
        {
            var cardsAr = (string[])photonEvent.CustomData;
            cards = cardsAr.ToList();
        }
        else if(photonEvent.Code == (byte)EEventType.tekashiFadeIn)
        {
            var SubmitImagePanel = Canvas.transform.Find("SubmitImage(Clone)").gameObject;
            //Animationを実行する
            var anim = TekashiAlertPanel.GetComponent<Animator>();
            anim.SetBool("FadeIn", true);
            SubmitImagePanel.SetActive(false);
        }
        else if(photonEvent.Code == (byte)EEventType.handsCount)
        {
            var CustomData = (int[])photonEvent.CustomData;
            var PlayerHandPanel = Canvas.transform.Find("PlayerHandPanel" + CustomData[0]);
            if(PlayerHandPanel != null)
            {
                PlayerHandPanel.Find("PlayerHand").GetComponent<TextMeshProUGUI>().text = CustomData[1].ToString();
            }
            else
            {
                Debug.Log("nulnul");
            }
        }
    }
}

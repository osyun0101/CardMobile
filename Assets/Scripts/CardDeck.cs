using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Newtonsoft.Json;

using static GameManager;

public class CardDeck : MonoBehaviour, IPointerClickHandler
{
    public GameObject SelfHandPanel;
    public GameObject Canvas;
    public GameObject Image;

    public void OnPointerClick(PointerEventData pointerData)
    {
        var r = Random.Range(0, GameManager.cards.Count);
        var cardstr = GameManager.cards[r];
        var cardImage = Instantiate((GameObject)Resources.Load("CardImage"));
        cardImage.GetComponent<HandCardScript>().cardName = cardstr;
        var path = $"Playing_Cards/Image/PlayingCards/{cardstr}";
        var card = Resources.Load<Sprite>(path);
        cardImage.GetComponent<RawImage>().texture = card.texture;
        cardImage.transform.SetParent(SelfHandPanel.transform);
        cardImage.transform.localScale = new Vector3(1, 1, 1);
        cardImage.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 190);
        var selectCards = SelectHandManager.selectCards;

        //ゲームマネージャーのカードリストからオブジェクト削除と山札のカウント修正
        GameManager.cards.RemoveAt(r);
        var option = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.All,
        };
        PhotonNetwork.RaiseEvent((byte)EEventType.cardCount, "neko", option, SendOptions.SendReliable);

        var random = Random.Range(0, selectCards.Count);
        var setCard = selectCards[random];
        PhotonNetwork.RaiseEvent((byte)EEventType.cardStage, setCard.gameObject.GetComponent<HandCardScript>().cardName, option, SendOptions.SendReliable);
        this.gameObject.SetActive(false);

        //プレイヤーの手札に山札から引いたカードを追加
        SelectHandManager.PlayerHands.Add(cardImage);
        //選択されたカードを削除し、新しい手札に整形
        SetSelfHand(SelectHandManager.selectCards);
        //SelectHandManagerのselectCardsフィールドをクリア
        selectCards.Clear();

        var SubmitImagePanel = Canvas.transform.Find("SubmitImage(Clone)").transform;
        var Triangle_1 = SubmitImagePanel.Find("Triangle");
        Triangle_1.gameObject.SetActive(false);
        var Triangle_2 = Image.transform.Find("Triangle(Clone)");
        Triangle_2.gameObject.SetActive(false);
        var clickPanel_1 = SubmitImagePanel.Find("GetClickPanel");
        clickPanel_1.gameObject.SetActive(false);
        var clickPanel_2 = Image.transform.Find("GetClickPanel2");
        clickPanel_2.gameObject.SetActive(false);
        GameManager.NextTurn(Canvas, PhotonNetwork.LocalPlayer.ActorNumber);
    }
}

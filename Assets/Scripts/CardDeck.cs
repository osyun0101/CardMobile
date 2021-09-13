using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDeck : MonoBehaviour, IPointerClickHandler
{
    public GameObject SelfHandPanel;
    public GameObject Canvas;

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
        GameManager.SetDeckCount();

        SelectHandManager.PlayerHands.Add(cardImage);
        OutPutButton.SetHandCard(Canvas.transform.Find("SubmitImage(Clone)").gameObject);
        this.gameObject.SetActive(false);
    }
}

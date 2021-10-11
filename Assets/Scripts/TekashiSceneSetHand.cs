using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TekashiSceneSetHand : MonoBehaviour
{
    private void Awake()
    {
        var posx = -250;
        foreach (var cardName in GameObject.Find("TekashiPlayerHands").GetComponent<TekashiPlayerHands>().PlayerHands)
        {
            var cardImage = Instantiate((GameObject)Resources.Load("CardImage"));
            cardImage.GetComponent<HandCardScript>().cardName = cardName;
            var path = $"Playing_Cards/Image/PlayingCards/{cardName}";
            var card = Resources.Load<Sprite>(path);
            cardImage.GetComponent<RawImage>().texture = card.texture;
            cardImage.transform.SetParent(this.gameObject.transform);
            cardImage.transform.localScale = new Vector3(1, 1, 1);
            cardImage.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 190);
            cardImage.transform.localPosition = new Vector3(posx, 0, 0);
            posx += 125;
        }
    }
}

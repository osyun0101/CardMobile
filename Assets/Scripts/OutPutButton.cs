using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OutPutButton : MonoBehaviour
{
    public GameObject Canvas;
    public void OutputCard()
    {
        var SubmitImagePanel = Canvas.transform.Find("SubmitImage").gameObject;
        var selectCards = SelectHandManager.selectCards;
        var random = Random.Range(0, selectCards.Count);
        var selectCard = selectCards[random];
        SubmitImagePanel.SetActive(true);
        SubmitImagePanel.GetComponent<RawImage>().texture = selectCard.gameObject.GetComponent<RawImage>().texture;

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

        //SelectHandManagerのselectCardsフィールドをクリア
        selectCards.Clear();
    }
}

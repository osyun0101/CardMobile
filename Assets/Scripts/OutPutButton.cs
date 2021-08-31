using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class OutPutButton : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject ErrorPanel;

    public void OutputCard()
    {
        var SubmitImagePanel = Canvas.transform.Find("SubmitImage").gameObject;
        var selectCards = SelectHandManager.selectCards;

        //場に出せる条件に沿っているか検証
        bool Judge = false;
        if(selectCards.Count >= 3 || selectCards.Count == 1)
        {
            Judge = true;
        }
        else if(selectCards.Count == 2)
        {
            var a = SelectHandManager.LastStr(selectCards[0].cardName);
            var b = SelectHandManager.LastStr(selectCards[1].cardName);
            if(a == b || selectCards[0].cardName.Contains("Joker") || selectCards[1].cardName.Contains("Joker"))
            {
                Judge = true;
            }
        }

        if (Judge)
        {
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
        else
        {
            ErrorPanel.SetActive(true);
            var red = ErrorPanel.GetComponent<Image>().color.r;
            var green = ErrorPanel.GetComponent<Image>().color.g;
            var blue = ErrorPanel.GetComponent<Image>().color.b;

            float alfa = 1.0f;
            for(int i = 0; i < 255; i++)
            {
                alfa -= alfa / 255;
                ErrorPanel.GetComponent<Image>().color = new Color(red, green, blue, alfa);
            }
        }
    }
}

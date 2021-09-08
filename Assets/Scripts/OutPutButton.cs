using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OutPutButton : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject ErrorPanel;
    public TextMeshProUGUI ErrorText;
    public bool isFadeOut = false;
    float speed = 0.001f;
    float Panelred, Panelgreen, Panelblue, Panelalfa;
    float Textred, Textgreen, Textblue, Textalfa;

    void Update()
    {
        if (isFadeOut)
        {
            StartFadeOut();
        }
    }

    public void OutputCard()
    {
        var SubmitImagePanel = Canvas.transform.Find("SubmitImage(Clone)").gameObject;
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
            var SelectCardText = Canvas.transform.Find("SelfHandPanel").Find("SelectCardText");
            SelectCardText.GetComponent<TextMeshProUGUI>().text = "引くカードを選択してください";


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
            Panelred = ErrorPanel.GetComponent<Image>().color.r;
            Panelgreen = ErrorPanel.GetComponent<Image>().color.g;
            Panelblue = ErrorPanel.GetComponent<Image>().color.b;
            ErrorPanel.GetComponent<Image>().color = new Color(Panelred, Panelgreen, Panelblue, 1.0f);
            Panelalfa = ErrorPanel.GetComponent<Image>().color.a;

            Textred = ErrorText.GetComponent<TextMeshProUGUI>().color.r;
            Textgreen = ErrorText.GetComponent<TextMeshProUGUI>().color.g;
            Textblue = ErrorText.GetComponent<TextMeshProUGUI>().color.b;
            ErrorText.GetComponent<TextMeshProUGUI>().color = new Color(Textred, Textgreen, Textblue, 1.0f);
            Textalfa = ErrorText.GetComponent<TextMeshProUGUI>().color.a;
            Delay();
        }
    }

    void StartFadeOut()
    {
        Panelalfa -= speed;
        Textalfa -= speed;
        ErrorPanel.GetComponent<Image>().color = new Color(Panelred, Panelgreen, Panelblue, Panelalfa);
        ErrorText.GetComponent<TextMeshProUGUI>().color = new Color(Textred, Textgreen, Textblue, Textalfa);
        if (Panelalfa  < 0 && Textalfa < 0)
        {
            isFadeOut = false;
        }
    }

    async void Delay()
    {
        await Task.Delay(2000);
        isFadeOut = true;
    }
}

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
    public GameObject Triangle;
    public GameObject Image;
    GameObject Triangle2;
    public GameObject GetClickPanel;
    GameObject GetClickPanel2;
    public bool isFadeOut = false;
    float speed = 0.001f;
    float Panelred, Panelgreen, Panelblue, Panelalfa;
    float Textred, Textgreen, Textblue, Textalfa;

    //Triangle系
    bool moveTriangle = false;
    float TriangleY = 70;
    bool over = false; //100を超えてる時true

    void Update()
    {
        if (isFadeOut)
        {
            StartFadeOut();
        }

        if (moveTriangle)
        {
            MoveTriangle();
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

            //三角形のオブジェクト作成
            Triangle.GetComponent<Triangle>().CreateTriangle();
            Triangle2 = Object.Instantiate(Triangle);
            Triangle2.GetComponent<Triangle>().CreateTriangle();
            Triangle.transform.SetParent(SubmitImagePanel.transform);
            Triangle2.transform.SetParent(Image.transform);
            GetClickPanel2 = Object.Instantiate(GetClickPanel);
            GetClickPanel.transform.SetParent(SubmitImagePanel.transform); //場のカード上に選択用のパネルを設置
            GetClickPanel.transform.localPosition = new Vector3(0, 0, 100);
            GetClickPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(90, SubmitImagePanel.GetComponent<RectTransform>().sizeDelta.y);
            GetClickPanel.SetActive(true);
            GetClickPanel2.transform.SetParent(Image.transform);　//山札のカード上に選択用のパネルを設置
            GetClickPanel2.transform.localScale = new Vector3(1, 1, 1);
            GetClickPanel2.GetComponent<RectTransform>().sizeDelta = new Vector2(90, Image.GetComponent<RectTransform>().sizeDelta.y);
            GetClickPanel2.SetActive(true);
            moveTriangle = true;

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

    //上下に三角形のオブジェクトを動かすメソッド
    void MoveTriangle()
    {
        if(over)
        {
            TriangleY -= 0.5f;
            if(TriangleY <= 70)
            {
                over = false;
            }
        }
        else
        {
            TriangleY += 0.5f;
            if(TriangleY >= 100)
            {
                over = true;
            }
        }
        Triangle.transform.localPosition = new Vector3(0, TriangleY, 100);
        Triangle2.transform.localPosition = new Vector3(0, TriangleY, 100);
    }
}

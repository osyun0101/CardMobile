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
    public GameObject GetClickPanel2;
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
            var Triangle_1 = SubmitImagePanel.transform.Find("Triangle");
            var Triangle_2 = Image.transform.Find("Triangle(Clone)");
            var clickPanel_1 = SubmitImagePanel.transform.Find("GetClickPanel");
            var clickPanel_2 = Image.transform.Find("GetClickPanel2");

            if (Triangle_1 == null
                && Triangle_2 == null
                && clickPanel_1 == null
                && clickPanel_2 == null)
            {
                Triangle.GetComponent<Triangle>().CreateTriangle();
                Triangle2 = Object.Instantiate(Triangle);
                Triangle2.GetComponent<Triangle>().CreateTriangle();
                Triangle.transform.SetParent(SubmitImagePanel.transform);
                Triangle2.transform.SetParent(Image.transform);
                GetClickPanel.transform.SetParent(SubmitImagePanel.transform); //場のカード上に選択用のパネルを設置
                GetClickPanel.transform.localPosition = new Vector3(0, 0, 100);
                GetClickPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(90, SubmitImagePanel.GetComponent<RectTransform>().sizeDelta.y);
                GetClickPanel.SetActive(true);
                GetClickPanel2.transform.SetParent(Image.transform); //場のカード上に選択用のパネルを設置
                GetClickPanel2.transform.localPosition = new Vector3(0, 0, 100);
                GetClickPanel2.GetComponent<RectTransform>().sizeDelta = new Vector2(90, Image.GetComponent<RectTransform>().sizeDelta.y);
                GetClickPanel2.SetActive(true);
            }
            else
            {
                Triangle.SetActive(true);
                Triangle2.SetActive(true);
                GetClickPanel.SetActive(true);
                GetClickPanel2.SetActive(true);
            }
            moveTriangle = true;
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

    //場に捨てるカードを設置する処理
    public static void SetSubmit(GameObject canvas, GameObject ClickObject, GameObject SelfHandPanel)
    {
        var SubmitImagePanel = canvas.transform.Find("SubmitImage(Clone)").gameObject;
        var selectCards = SelectHandManager.selectCards;
        var random = Random.Range(0, selectCards.Count);
        var selectCard = selectCards[random];
        SubmitImagePanel.SetActive(true);

        var cardImage = Instantiate((GameObject)Resources.Load("CardImage"));
        cardImage.GetComponent<HandCardScript>().cardName = SubmitImagePanel.GetComponent<SubmitImage>().CardName;
        cardImage.GetComponent<RawImage>().texture = SubmitImagePanel.GetComponent<RawImage>().texture;
        cardImage.transform.SetParent(SelfHandPanel.transform);
        cardImage.transform.localScale = new Vector3(1, 1, 1);
        cardImage.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 190);
        SelectHandManager.PlayerHands.Add(cardImage);

        SubmitImagePanel.GetComponent<SubmitImage>().CardName = selectCard.gameObject.GetComponent<HandCardScript>().cardName;
        SubmitImagePanel.GetComponent<RawImage>().texture = selectCard.gameObject.GetComponent<RawImage>().texture;
        ClickObject.SetActive(false);

        SetSelfHand(selectCards);
        //SelectHandManagerのselectCardsフィールドをクリア
        selectCards.Clear();
    }

    //山札から引いた時の処理
    public static void SetHandCard(GameObject SubmitImagePanel)
    {
        var selectCards = SelectHandManager.selectCards;
        SetSelfHand(selectCards);
        //SelfHandPanelにある手札を削除
        /*foreach (var cardobj in selectCards)
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
        SelectHandManager.PlayerHands = newList;*/

        //山札からカードを引いた後、場に出す処理
        var random = Random.Range(0, selectCards.Count);
        var selectCard = selectCards[random];
        SubmitImagePanel.SetActive(true);
        SubmitImagePanel.GetComponent<SubmitImage>().CardName = selectCard.gameObject.GetComponent<HandCardScript>().cardName;
        SubmitImagePanel.GetComponent<RawImage>().texture = selectCard.gameObject.GetComponent<RawImage>().texture;

        //SelectHandManagerのselectCardsフィールドをクリア
        selectCards.Clear();
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
}

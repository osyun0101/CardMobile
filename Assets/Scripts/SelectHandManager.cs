using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHandManager : MonoBehaviour
{
    public static List<HandCardScript> selectCards = new List<HandCardScript>();
    public static bool serial = false;
    public static bool serial_status = false;

    private void Awake()
    {
        var PlayeyHand = GameObject.Find("PlayerHandPanel(Clone)").GetComponent<PlayerObject>().playerModel.hands;
    }

    public static void SetSelectCard(HandCardScript card)
    {
        var cardImage = card.cardName.Substring(0, card.cardName.Length - 2);
        if (selectCards.Count == 1)
        {
            var selectCardImage = selectCards[0].cardName.Substring(0, selectCards[0].cardName.Length - 2);
            if(cardImage == selectCardImage)
            {
                int index_0;
                int cardint;
                var ParseBool = int.TryParse(LastStr(selectCards[0].cardName), out index_0);
                var ParseBool2 = int.TryParse(LastStr(card.cardName), out cardint);
                if (cardint + 1 == index_0 || cardint - 1 == index_0)
                {
                    if (ParseBool && ParseBool2)
                    {
                        serial = true;
                        serial_status = true;
                    }
                }
            }
        }

        if(selectCards.Count >= 2 && serial_status)
        {
            int pramint;
            var parmnum = int.TryParse(LastStr(card.cardName), out pramint);
            string ListImage = "";
            List<int> numList = new List<int>();
            foreach(var obj in selectCards)
            {
                var card_Name = obj.cardName;
                ListImage = card_Name.Substring(0, card_Name.Length - 2);
                int card_num;
                var ParseBl = int.TryParse(LastStr(card_Name), out card_num);
                if (ParseBl)
                {
                    numList.Add(card_num);
                }
            }
            numList.Sort();

            if(numList[0] - 1 == pramint || numList[numList.Count-1] + 1 == pramint)
            {
                if(cardImage == ListImage)
                {
                    serial = true;
                }
            }
        }

        if (selectCards.Count != 0)
        {
            if (!serial)
            {
                var laststr = LastStr(card.cardName);
                var lastStrCheck = true;
                foreach (var c in selectCards)
                {
                    if (!c.cardName.Contains(laststr))
                    {
                        lastStrCheck = false;
                        break;
                    }
                }
                if (!lastStrCheck)
                {
                    foreach (var c in selectCards)
                    {
                        var x = c.transform.localPosition.x;
                        var y = c.transform.localPosition.y;
                        var z = c.transform.localPosition.z;
                        c.transform.localPosition = new Vector3(x, y - 25, z);
                        c.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f, 0.5f);
                        c.GetComponent<Outline>().effectDistance = new Vector2(1f, 1f);
                    }
                    selectCards = new List<HandCardScript>();
                }
                serial_status = false;
            }
        }
        serial = false;
        selectCards.Add(card);
    }

    public static void ResetSelectCard(HandCardScript card)
    {
        selectCards.Remove(card);
    }

    public static string LastStr(string str)
    {
        return str.Substring(str.Length - 2);
    }
}

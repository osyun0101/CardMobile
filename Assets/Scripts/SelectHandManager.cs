using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectHandManager : MonoBehaviour
{
    public static List<HandCardScript> selectCards = new List<HandCardScript>();

    private void Awake()
    {
        var PlayeyHand = GameObject.Find("PlayerHandPanel(Clone)").GetComponent<PlayerObject>().playerModel.hands;
    }

    public static void SetSelectCard(HandCardScript card)
    {
        if(selectCards.Count != 0)
        {
            var laststr = LastStr(card.cardName);
            var lastStrCheck = true;
            foreach(var c in selectCards)
            {
                if (!c.cardName.Contains(laststr))
                {
                    lastStrCheck = false;
                    break;
                }
            }
            if (!lastStrCheck)
            {
                foreach(var c in selectCards)
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
        }

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

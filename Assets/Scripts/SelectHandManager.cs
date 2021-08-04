using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHandManager : MonoBehaviour
{
    public static List<string> selectCards = new List<string>();

    private void Awake()
    {
        var PlayeyHand = GameObject.Find("PlayerHandPanel(Clone)").GetComponent<PlayerObject>().playerModel.hands;
    }

    public static void SetSelectCard(string card)
    {
        //仕様変更にて一旦この処理コメントアウト
        /*if(selectCards.Count != 0)
        {
            var laststr = LastStr(card);
            switch (selectCards.Count)
            {
                //選択されているカードが一枚の時
                case 1:
                    if(!selectCards[0].Contains(laststr))
                    {
                        ResetSelectCard(selectCards[0]);
                    }
                    break;
            }
        }*/

        selectCards.Add(card);
        foreach(var k in selectCards)
        {
            Debug.Log(k);
        }
    }

    public static void ResetSelectCard(string card)
    {
        selectCards.Remove(card);
        foreach(var k in selectCards)
        {
            Debug.Log(k);
        }
    }

    public static string LastStr(string str)
    {
        return str.Substring(str.Length - 2);
    }
}

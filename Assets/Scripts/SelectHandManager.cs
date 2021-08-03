using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHandManager : MonoBehaviour
{
    public static List<string> selectCards = new List<string>();

    private void Awake()
    {
        var PlayeyHand = GameObject.Find("PlayerHandPanel(Clone)").GetComponent<PlayerObject>().playerModel.hands;
        foreach (var hand in PlayeyHand)
        {
            Debug.Log(hand);
        }
    }

    public static void SetSelectCard(string card)
    {
        selectCards.Add(card);
        foreach(var k in selectCards)
        {
            Debug.Log(k);
        }
    }
}

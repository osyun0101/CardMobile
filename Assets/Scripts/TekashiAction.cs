using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TekashiAction : MonoBehaviour
{
    public GameObject TekashiPlayerHandsObj;
    public void Tekashi()
    {
        foreach(var hand in SelectHandManager.PlayerHands)
        {
            TekashiPlayerHandsObj.GetComponent<TekashiPlayerHands>().PlayerHands.Add(hand.GetComponent<HandCardScript>().cardName);
        }
        // シーン切り替え
        PhotonNetwork.LoadLevel("TekashiNext");
    }
}

using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class TekashiAction : MonoBehaviour
{
    public GameObject TekashiPlayerHandsObj;

    public void Tekashi()
    {
        foreach(var hand in SelectHandManager.PlayerHands)
        {
            TekashiPlayerHandsObj.GetComponent<TekashiPlayerHands>().PlayerHands.Add(hand.GetComponent<HandCardScript>().cardName);
        }


        var option = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.All,
        };
        //TekashiAlertのAnimationを実行する
        PhotonNetwork.RaiseEvent((byte)EEventType.tekashiFadeIn, "neko", option, SendOptions.SendReliable);
        // シーン切り替え
        //PhotonNetwork.LoadLevel("TekashiNext");
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class TekashiAction : MonoBehaviour
{
    public GameObject TekashiPlayerHandsObj;
    public GameObject OutPutButtom;

    public void Tekashi()
    {
        OutPutButtom.SetActive(false);
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

        Delay();
    }

    private async void Delay()
    {
        await Task.Delay(3000);
        // シーン切り替え
        PhotonNetwork.LoadLevel("TekashiNext");
    }
}

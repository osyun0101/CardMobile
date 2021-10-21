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
    public GameObject OutPutButtom;

    public void Tekashi()
    {
        OutPutButtom.SetActive(false);
        var option = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.All,
        };
        //TekashiAlertのAnimationを実行する
        PhotonNetwork.RaiseEvent((byte)EEventType.tekashiAction, PhotonNetwork.LocalPlayer.ActorNumber, option, SendOptions.SendReliable);
    }
}

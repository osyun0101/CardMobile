using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using static TekashiNextManager;
using ExitGames.Client.Photon;

public class ReleaseAction : MonoBehaviour
{
    public static int countNum;
    public GameObject CountText;
    public void Release()
    {
        var localPlayer = PhotonNetwork.LocalPlayer;
        var index = localPlayer.ActorNumber - 1;
        int.TryParse(CountText.GetComponent<TextMeshProUGUI>().text, out int count);
        var dic = new Dictionary<string, int>();
        dic.Add("index", index);
        dic.Add("count", count);
        var option = new RaiseEventOptions()
        {
            Receivers = ReceiverGroup.All,
        };
        PhotonNetwork.RaiseEvent((byte)EEventType.Release, dic, option, SendOptions.SendReliable);
    }
}

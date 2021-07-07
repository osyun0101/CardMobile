using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        Debug.Log("PlayRoomに遷移");
        var PlayerList = PhotonNetwork.PlayerList;
        foreach(var player in PlayerList)
        {
            Debug.Log(player.ActorNumber);
            Debug.Log(player.NickName);
        }
    }
}

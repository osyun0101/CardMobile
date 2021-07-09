using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerObject : MonoBehaviourPunCallbacks
{
    public PlayerModel playerModel;
    [PunRPC]
    public void SetPlayerModel(int[] cards)
    {
        Debug.Log(cards[0]);
        Debug.Log("testetst");
    }
}

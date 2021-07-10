using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerObject : MonoBehaviourPunCallbacks
{
    public PlayerModel playerModel;

    [PunRPC]
    public void SetPlayerModel(int[] cardsArr, int playerId)
    {
        var cards = new List<int>(cardsArr);
        var player = PhotonNetwork.PlayerList[playerId - 1];
        playerModel = new PlayerModel(player);
        playerModel.hands = cards;
    }
}

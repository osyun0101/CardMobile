using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerObject : MonoBehaviourPunCallbacks
{
    public PlayerModel playerModel;

    [PunRPC]
    public void SetPlayerModel(int[] cardsArr, int playerId, int playerListLength)
    {
        var cards = new List<int>(cardsArr);
        var playerList = PhotonNetwork.PlayerList;
        var player = playerList[playerId - 1];
        playerModel = new PlayerModel(player);
        playerModel.hands = cards;

        //最後のオブジェクトが生成された時
        if(playerId == playerListLength)
        {
            GameManager.GetAllPlayerHand();
        }
    }

    // ネットワークオブジェクトが生成された時に呼ばれるコールバック
    //こーゆーのもあるってことで残しとこうかな
    /*void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.Sender.IsLocal)
        {
            Debug.Log("自身がネットワークオブジェクトを生成しました");
        }
        else
        {
            Debug.Log("他プレイヤーがネットワークオブジェクトを生成しました");
        }

        GameManager.GetAllPlayerHand();
    }*/
}

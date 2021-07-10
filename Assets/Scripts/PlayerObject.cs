using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerObject : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
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

    // ネットワークオブジェクトが生成された時に呼ばれるコールバック
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.Sender.IsLocal)
        {
            Debug.Log("自身がネットワークオブジェクトを生成しました");
        }
        else
        {
            Debug.Log("他プレイヤーがネットワークオブジェクトを生成しました");
        }
        /*var PlayerHand = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(PlayerHand.Length);*/
    }
}

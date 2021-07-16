using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerObject : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
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
        /*
        //最後のオブジェクトが生成された時
        if(playerId == playerListLength)
        {
            GameManager.GetAllPlayerHand();
        }*/
    }

    // ネットワークオブジェクトが生成された時に呼ばれるコールバック
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        GameManager.PlayerActorNumber += 1;
        var parent = GameObject.Find("Canvas").transform;
        if(GameManager.PlayerActorNumber == PhotonNetwork.PlayerList.Length)
        {
            GameManager.GetAllPlayerHand();
            var OtherPlayerList = new List<GameObject>(GameManager.PlayerHands);
            var index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            OtherPlayerList[index].SetActive(false);
            OtherPlayerList.RemoveAt(index);
            if(OtherPlayerList.Count != 0)
            {
                foreach (var obj in OtherPlayerList)
                {
                    obj.transform.SetParent(parent);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    if (OtherPlayerList.Count == 1)
                    {
                        obj.transform.localPosition = new Vector3(0, 300, 0);
                    }
                }
            }
        }
    }
}

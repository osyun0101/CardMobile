using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    List<int> cards = new List<int>();
    public List<PlayerModel> playerModelList = new List<PlayerModel>();
    private void Awake()
    {
        Debug.Log("PlayRoomに遷移");
        /*var PlayerList = PhotonNetwork.PlayerList;
        foreach(var player in PlayerList)
        {
            Debug.Log(player.ActorNumber);
            Debug.Log(player.NickName);
        }*/
        // マスタークライアントの時
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            int num = 1;
            for (int s = 0; s < 12; s++)
            {
                for (int i = 0; i < 4; i++)
                {
                    cards.Add(num);
                }
                num += 1;
            }

            var PlayerList = PhotonNetwork.PlayerList;
            foreach(var player in PlayerList)
            {
                /*var playerModel = new PlayerModel(player);
                for(int i = 0; i < 5; i++)
                {
                    var random = Random.Range(0, cards.Count);
                    playerModel.hands.Add(cards[random]);
                    cards.RemoveAt(random);
                }*/
                var playerPanel = PhotonNetwork.InstantiateRoomObject("PlayerHand", new Vector3(0, 0, 0), Quaternion.identity);
                playerPanel.GetComponent<PhotonView>().RPC("SetPlayerModel", RpcTarget.All, cards.ToArray());
                //playerModelList.Add(playerModel);
            }
        }
    }
}

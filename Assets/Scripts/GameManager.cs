using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{ 
    public static List<int> cards = new List<int>();
    public static GameObject[] PlayerHands = new GameObject[] { };
    public static int PlayerActorNumber = 0;

    private void Awake()
    {
        Debug.Log("PlayRoomに遷移");
        // マスタークライアントの時
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            int num = 1;
            for (int s = 0; s < 13; s++)
            {
                for (int i = 0; i < 4; i++)
                {
                    cards.Add(num);
                }
                num += 1;
            }

            var PlayerList = PhotonNetwork.PlayerList;
            
            foreach (var player in PlayerList)
            {
                List<int> dataList = new List<int>();
                for (int i = 0; i < 5; i++)
                {
                    var random = Random.Range(0, cards.Count);
                    dataList.Add(cards[random]);
                    cards.RemoveAt(random);
                }

                var playerPanel = PhotonNetwork.InstantiateRoomObject("PlayerHandPanel", new Vector3(0, 0, 0), Quaternion.identity);
                playerPanel.GetComponent<PhotonView>().RPC("SetPlayerModel", RpcTarget.All, dataList.ToArray(), player.ActorNumber, PlayerList.Length);
            }

            var cardsResidue = PhotonNetwork.InstantiateRoomObject("CardsResidue", new Vector3(0, 0, 0), Quaternion.identity);
            cardsResidue.GetComponent<PhotonView>().RPC("cardSet", RpcTarget.All, cards.Count);
        }
    }

    //Rpcで生成したネットワークオブジェクトから実行
    public static void GetAllPlayerHand()
    {
        PlayerHands = GameObject.FindGameObjectsWithTag("Player");
    }
}

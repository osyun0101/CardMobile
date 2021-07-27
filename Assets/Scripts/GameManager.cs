using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static List<string> cards = new List<string>();
    public static GameObject[] PlayerHands = new GameObject[] { };
    public static List<List<string>> PlayerHandsList = new List<List<string>>();
    public static int PlayerActorNumber = 0;
    string mark;

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
                    
                    switch (i)
                    {
                        case 0:
                            mark = "Heart";
                            break;
                        case 1:
                            mark = "Club";
                            break;
                        case 2:
                            mark = "Diamond";
                            break;
                        case 3:
                            mark = "Spade";
                            break;
                    }
                    if(num < 10)
                    {
                        cards.Add(mark + 0 + num.ToString());
                    }
                    else
                    {
                        cards.Add(mark + num.ToString());
                    }
                }
                num += 1;
            }

            var PlayerList = PhotonNetwork.PlayerList;
            int List_i = 0;
            foreach (var player in PlayerList)
            {
                List<string> dataList = new List<string>();
                PlayerHandsList.Add(new List<string>());
                
                for (int i = 0; i < 5; i++)
                {
                    var random = Random.Range(0, cards.Count);
                    dataList.Add(cards[random]);
                    PlayerHandsList[List_i].Add(cards[random]);
                    cards.RemoveAt(random);
                }
                List_i += 1;
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

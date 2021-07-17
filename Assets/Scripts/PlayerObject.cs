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
            var count = OtherPlayerList.Count;
            float angle;
            if(count != 0)
            {
                if(count == 1)
                {
                    foreach (var obj in OtherPlayerList)
                    {
                        obj.transform.SetParent(parent);
                        obj.transform.localScale = new Vector3(1, 1, 1);
                        obj.transform.localPosition = new Vector3(0, 300, 0);
                    }
                }
                else
                {
                    var x_y = new List<int[]>();
                    var reverseX_Y = new List<int[]>();
                    if (count % 2 != 0)
                    {
                        angle = 90f / ((float)count - 1);
                    }
                    else
                    {
                        angle = 90f / (float)count;
                        var updateAngle = angle;
                        for(var i = 0; i < count/2; i++)
                        {
                            reverseX_Y.Add(CalcXY(updateAngle));
                            updateAngle += angle;
                        }

                        foreach(var re in reverseX_Y)
                        {
                            x_y.Add(re);
                        }

                        reverseX_Y.Reverse();
                        foreach(var r in reverseX_Y)
                        {
                            r[0] = -r[0];
                        }
                        reverseX_Y.AddRange(x_y);
                        Set(OtherPlayerList, parent, count, reverseX_Y);
                        
                    }
                }
            }
        }
    }

    public int[] CalcXY(float angle)
    {
        double angleBparse = 180 / angle;
        var sinBangle = 3.14f / angleBparse;
        int y = (int)(300 * sinBangle);

        var angleA = 180 - (90 + angle);
        double angleAparse = 180 / angleA;
        var sinAangle = 3.14f / angleAparse;
        int x = (int)(300 * sinAangle);

        var x_y = new int[] { x, y};
        return x_y;
    }

    public void Set(List<GameObject> OtherPlayerList, Transform parent, int count, List<int[]> x_y)
    {
        var i = 0;
        var count_i = 0;
        foreach (var obj in OtherPlayerList)
        {
            obj.transform.SetParent(parent);
            obj.transform.localScale = new Vector3(1, 1, 1);
            if (count % 2 != 0 && count_i == count / 2)
            {
                obj.transform.localPosition = new Vector3(0, 300, 0);
                count_i += 1;
            }
            else
            {
                obj.transform.localPosition = new Vector3(x_y[i][0], x_y[i][1], 0);
                i += 1;
                count_i += 1;
            }
        }
    }
}

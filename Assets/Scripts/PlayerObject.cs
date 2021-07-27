using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;

public class PlayerObject : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public PlayerModel playerModel;

    [PunRPC]
    public void SetPlayerModel(string[] cardsArr, int playerId, int playerListLength)
    {
        var cards = new List<string>(cardsArr);
        var playerList = PhotonNetwork.PlayerList;
        var player = playerList[playerId - 1];
        playerModel = new PlayerModel(player);
        playerModel.hands = cards;
        this.transform.Find("PlayerHand").GetComponent<TextMeshProUGUI>().text = cards.Count.ToString();
        this.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = player.NickName;
    }

    // ネットワークオブジェクトが生成された時に呼ばれるコールバック
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("生成されました");
        
        //他のプレイヤーの配置
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

            //自分の次の人から左回りになる様にリストを入れ替え
            if(index != 0 && index != count)
            {
                var last_index = count - 1;
                for(var i = index; i < count; i++)
                {
                    OtherPlayerList.Insert(0,OtherPlayerList[last_index]);
                    OtherPlayerList.RemoveAt(last_index + 1);
                }
            }

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
                    var harfx = Screen.width / 2;
                    var harfy = Screen.height / 2;
                    int sideNum;
                    List<int[]> x_y = new List<int[]>();
                    List<int[]> reverseX_Y = new List<int[]>();
                    if(count % 2 == 0)
                    {
                        sideNum = count / 2;
                    }
                    else
                    {
                        sideNum = (count - 1) / 2;
                    }
                    int splitx = harfx / (sideNum + 1);
                    int splity = harfy / (sideNum + 1);
                    int y = 0;
                    for(var i = 0; i < count / 2; i++)
                    {
                        harfx = harfx - splitx;
                        y += splity;
                        var intarr = new int[] { harfx, y };
                        x_y.Add(intarr);
                        //x_yと同じオブジェクトを参照しない様にするため
                        intarr = new int[] { harfx, y };
                        reverseX_Y.Add(intarr);
                    }

                    foreach(var n in x_y)
                    {
                        n[0] = -n[0];
                    }
                    reverseX_Y.Reverse();
                    x_y.AddRange(reverseX_Y);
                    Set(OtherPlayerList, parent, count, x_y);

                    //sinを使用して角度と斜辺から他の辺の長さをx,yとして使用する場合
                    //真ん中の両端が極端に離れてしまう問題がある
                    /*List<int[]> x_y = new List<int[]>();
                    var reverseX_Y = new List<int[]>();
                    float angle;
                    if (count % 2 != 0)
                    {
                        angle = 90f / ((float)count - 1);
                    }
                    else
                    {
                        angle = 90f / (float)count;
                    }

                    var updateAngle = angle;
                    for (var i = 0; i < count / 2; i++)
                    {
                        reverseX_Y.Add(CalcXY(updateAngle));
                        updateAngle += angle;
                    }

                    for (var n = 0; n < reverseX_Y.Count; n++)
                    {
                        x_y.Add(reverseX_Y[n]);
                        reverseX_Y[n] = new int[] { reverseX_Y[n][0], reverseX_Y[n][1] };
                    }

                    x_y.Reverse();
                    foreach (var r in reverseX_Y)
                    {
                        r[0] = -r[0];
                    }
                    reverseX_Y.AddRange(x_y);
                    foreach (var s in reverseX_Y)
                    {
                        Debug.Log(s[0] + "x");
                        Debug.Log(s[1] + "y");
                    }
                    Set(OtherPlayerList, parent, count, reverseX_Y);*/
                }
            }
        }
    }

    //角度と斜辺から他の辺の長さをsinを使って計算する関数
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
                obj.transform.localPosition = new Vector3(0, 330, 0);
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

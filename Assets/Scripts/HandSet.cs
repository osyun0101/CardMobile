using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HandSet : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    private int PlayerId = 0;
    // ネットワークオブジェクトが生成された時に呼ばれるコールバック
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if(PlayerId == 0)
        {
            PlayerId = PhotonNetwork.LocalPlayer.ActorNumber;
            Debug.Log(PlayerId);
        }
    }

    [PunRPC]
    public void SetHand(int id, string[] data)
    {
        if(id == PlayerId)
        {
            var parent = GameObject.Find("Canvas").transform.Find("SelfHandPanel");

            foreach(var d in data)
            {
                var cardImage = Instantiate((GameObject)Resources.Load("CardImage"));
                var path = $"Playing_Cards/Image/PlayingCards/{d}";
                var card = Resources.Load<Sprite>(path);
                cardImage.GetComponent<Image>().sprite = card;
                cardImage.transform.SetParent(parent);
            }
        }
    }
}

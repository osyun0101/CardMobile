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
            var posx = -250;
            foreach(var d in data)
            {
                var cardImage = Instantiate((GameObject)Resources.Load("CardImage"));
                cardImage.GetComponent<HandCardScript>().cardName = d;
                var path = $"Playing_Cards/Image/PlayingCards/{d}";
                var card = Resources.Load<Sprite>(path);
                cardImage.GetComponent<RawImage>().texture = card.texture;
                cardImage.transform.SetParent(parent);
                cardImage.transform.localScale = new Vector3(1, 1, 1);
                cardImage.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 190);
                cardImage.transform.localPosition = new Vector3(posx, 0, 0);
                posx += 125;
            }
        }
    }
}

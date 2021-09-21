using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SubmitImage : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public string CardName;

    [PunRPC]
    public void SetSubmitImage(string cardName)
    {
        CardName = cardName;
        var path = $"Playing_Cards/Image/PlayingCards/{cardName}";
        var card = Resources.Load<Sprite>(path);
        this.GetComponent<RawImage>().texture = card.texture;
    }

    // ネットワークオブジェクトが生成された時に呼ばれるコールバック
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        var Canvas = GameObject.Find("Canvas").transform;
        this.transform.SetParent(Canvas);
        this.transform.localPosition = new Vector3(-120, 0, 0);
        this.transform.localScale = new Vector3(1, 1, 1);
    }
}
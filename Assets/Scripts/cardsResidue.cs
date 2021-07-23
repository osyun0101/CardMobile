using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class cardsResidue : MonoBehaviour
{
    [PunRPC]
    public void cardSet(int cardsCount)
    {
        var Canvas = GameObject.Find("Canvas");
        var cardsImage = Canvas.transform.Find("Image");
        this.transform.SetParent(cardsImage);
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.localPosition = new Vector3(0, -90, 0);
        this.GetComponent<Text>().text = cardsCount.ToString();
    }
}

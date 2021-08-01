using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class TurnText : MonoBehaviour
{
    [PunRPC]
    public void SetText(string name)
    {
        var parent = GameObject.Find("Canvas").transform.Find("SelfHandPanel").Find("YouturnImage");
        this.transform.SetParent(parent);
        this.transform.localScale = new Vector3(1, 1, 1);
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(170, 180);
        this.GetComponent<TextMeshProUGUI>().text = name + "の番です";
        parent.GetComponent<Image>().color = new Color(0f, 100.0f/ 255.0f, 255.0f/255.0f, 1f);
    }
}

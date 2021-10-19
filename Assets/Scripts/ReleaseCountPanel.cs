using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ReleaseCountPanel : MonoBehaviour
{
    [PunRPC]
    public void SetPosition(float posx, float posy, float width, string nickName)
    {
        this.transform.SetParent(TekashiNextManager.ScrollView.transform);
        this.transform.localPosition = new Vector3(posx, posy, 0);
        this.transform.localScale = new Vector3(1, 1, 1);
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 160.0f);
        var PlayerNameObj = this.transform.Find("PlayerName");
        PlayerNameObj.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 50.0f);
        PlayerNameObj.GetComponent<TextMeshProUGUI>().text = nickName;
    }
}

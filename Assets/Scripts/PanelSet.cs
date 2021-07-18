using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PanelSet : MonoBehaviourPunCallbacks
{

    [PunRPC]
    public void RPCFunc(string Name)
    {
        this.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = Name;
        this.transform.SetParent(GameObject.Find("Canvas").transform);
        this.transform.localScale = new Vector3(1, 1, 1);
        var x = -245;
        var y = 95;
        int PlayersCount = photonView.ViewID;
        if (PlayersCount % 2 == 0)
        {
            x = 245;
            if (PlayersCount != 2)
            {
                y -= ((PlayersCount - 2) / 2) * 75;
            }
        }
        else
        {
            if (PlayersCount != 1)
            {
                y -= ((PlayersCount - 1) / 2) * 75;
            }
        }
        this.transform.localPosition = new Vector3(x, y, 0);
    }
}

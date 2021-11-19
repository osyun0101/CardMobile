using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ReplayAction : MonoBehaviour
{
    public void PlayRoomLoad()
    {
        PhotonNetwork.LoadLevel("PlayRoom");
    }
}

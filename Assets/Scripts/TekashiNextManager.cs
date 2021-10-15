using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TekashiNextManager : MonoBehaviour
{
    private void Awake()
    {
        var ReleaseCountPanel = PhotonNetwork.InstantiateRoomObject("ReleaseCountPanel", new Vector3(0, 0, 0), Quaternion.identity);
    }
}

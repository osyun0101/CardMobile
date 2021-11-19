using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedirectAction : MonoBehaviour
{
    public void PlayRoomLoad()
    {
        PhotonNetwork.LoadLevel("PlayRoom");
    }

    public void TopLoad()
    {
        SceneManager.LoadScene("NickName");
    }
}

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TekashiAction : MonoBehaviour
{
    public GameObject TekashiPlayerHandsObj;
    public void Tekashi()
    {
        TekashiPlayerHandsObj.GetComponent<TekashiPlayerHands>().PlayerHands = GameManager.PlayerHands;
        // シーン切り替え
        PhotonNetwork.LoadLevel("TekashiNext");
    }
}

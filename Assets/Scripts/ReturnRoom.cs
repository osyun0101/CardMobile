using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ReturnRoom : MonoBehaviourPunCallbacks
{
    public void OutRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            var d = PhotonNetwork.CurrentRoom;
            Debug.Log(d.Name);
            // 退室
            PhotonNetwork.LeaveRoom();
        }
        SceneManager.LoadScene("CreateOrRoomIn");
    }

    // 部屋から退室した時
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }

    // ロビーから退室した時
    public override void OnLeftLobby()
    {
        Debug.Log("ロビーから退出");
    }
}

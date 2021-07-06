using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Realtime;
using Photon.Pun;

public class RoomButton : MonoBehaviour
{
    [SerializeField] Transform Parent;

    private void Awake()
    {
        //ボタンを取得し表示していた時の処理
        var obj = GameObject.Find("InputFieldEvent");
        if(obj != null)
        {
            var RoomName = obj.GetComponent<InputFieldEvent>().RoomNameText;
            var button = (GameObject)Resources.Load("Button");
            button.transform.Find("Text").GetComponent<Text>().text = RoomName;
            Instantiate(button, Parent);
        }
    }
}

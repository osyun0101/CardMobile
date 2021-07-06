using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputTextKeep : MonoBehaviour
{
    public string RoomNameText;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void NameSet(string text)
    {
        RoomNameText = text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountPanelAction : MonoBehaviour
{
    public int HandCount;
    public void SetCount()
    {
        this.gameObject.transform.Find("PlayerCount").GetComponent<TextMeshProUGUI>().text = HandCount.ToString();
    }
}

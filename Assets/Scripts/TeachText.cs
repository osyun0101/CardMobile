using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachText : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("-----------");
        Debug.Log(GameObject.Find("TekashiPlayerHands").GetComponent<TekashiPlayerHands>().PlayerHands[0]);
    }
}

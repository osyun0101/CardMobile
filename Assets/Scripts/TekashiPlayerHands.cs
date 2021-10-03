using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TekashiPlayerHands : MonoBehaviour
{
    public GameObject[] PlayerHands;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}

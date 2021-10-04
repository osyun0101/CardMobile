using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TekashiPlayerHands : MonoBehaviour
{
    public List<string> PlayerHands = new List<string>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}

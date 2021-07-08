using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class PlayerModel
{
    public Player player;
    public List<int> hands = new List<int>();

    public PlayerModel(Player localPlayer)
    {
        player = localPlayer;
    }
}

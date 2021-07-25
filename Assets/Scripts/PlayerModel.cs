using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class PlayerModel
{
    public Player player;
    public List<string> hands = new List<string>();

    public PlayerModel(Player localPlayer)
    {
        player = localPlayer;
    }
}

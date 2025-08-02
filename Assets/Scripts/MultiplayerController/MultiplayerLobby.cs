using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Keeps track of players that have joined the host, as well as has colours that players are automatically assigned to.
/// </summary>
public class MultiplayerLobby : NetworkBehaviour
{
    public List<GameObject> playersInLobby;
    [SerializeField] private List<Color> playerColours;

    public static MultiplayerLobby instance;

    public delegate void SubscribePlayer(GameObject player);
    public static SubscribePlayer playerJoin;
    public static SubscribePlayer playerLeave;

    public Color GetPlayerColour(int colorID)
    {
        return playerColours[colorID]; // each player gets and sets their colour through the use of their unique player index
    }

    private void OnEnable()
    {
        instance = this;

        playerJoin += AddPlayerToList;
        playerLeave += RemovePlayerFromList;
        
    }

    public void AddPlayerToList(GameObject player)
    {
        playersInLobby.Add(player);
    }
    public void RemovePlayerFromList(GameObject player)
    {
        playersInLobby.Remove(player);
    }

}

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiplayerLobby : MonoBehaviour
{
    [SerializeField] private List<GameObject> players;

    public static MultiplayerLobby instance;

    public delegate void SubscribePlayer(GameObject player);
    public static SubscribePlayer playerJoin;
    public static SubscribePlayer playerLeave;

    private void OnEnable()
    {
        instance = this;

        playerJoin += AddPlayerToList;
        playerLeave += RemovePlayerFromList;
    }

    private void FixedUpdate()
    {
       
    }

    public void AddPlayerToList(GameObject player)
    {
        players.Add(player);
    }
    public void RemovePlayerFromList(GameObject player)
    {
        players.Remove(player);
    }
}

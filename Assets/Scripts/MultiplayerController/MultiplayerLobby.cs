using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
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

    public delegate void PrintPlayerName(GameObject playerObject);
    public static PrintPlayerName assignPlayerName;

    [SerializeField] private GameObject playerNameParent;
    [SerializeField] private ScriptablePrefab playerNamePrefab;

    // allows the player to change their name and display it in the lobby scene
    //[SerializeField] private TMP_InputField inputPlayerName;
    //[SerializeField] private TextMeshProUGUI displayPlayerName;
    //[SerializeField] private string customPlayerName;

    public List<GameObject> lobbyNameList;

    public Color GetPlayerColour(int colorID)
    {
        return playerColours[colorID]; // each player gets and sets their colour through the use of their unique player index
    }

    private void OnEnable()
    {
        instance = this;

        playerJoin += AddPlayerToList;
        assignPlayerName += AddPlayerNameToLobby;

        playerLeave += RemovePlayerFromList;
    }

    private void OnDisable()
    {
        playerJoin -= AddPlayerToList;
        assignPlayerName -= AddPlayerNameToLobby;

        playerLeave -= RemovePlayerFromList;
    }

    public void AddPlayerNameToLobby(GameObject playerObject)
    {
        LobbyDisplay lobbyDisplay = new LobbyDisplay();

        lobbyDisplay.displayObjectPrefab = playerNamePrefab.prefab;
        GameObject namePrefab = Instantiate(lobbyDisplay.displayObjectPrefab);
        namePrefab.transform.SetParent(playerNameParent.transform);
        namePrefab.GetComponent<GeneratePlayerInstance>().playerNameUI.text = playerObject.name;
        namePrefab.GetComponent<GeneratePlayerInstance>().namePlateOwner = playerObject;
        lobbyNameList.Add(namePrefab);
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

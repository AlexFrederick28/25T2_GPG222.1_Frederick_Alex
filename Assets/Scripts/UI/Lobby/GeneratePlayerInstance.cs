using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GeneratePlayerInstance : MonoBehaviour
{
    public delegate void GetLobbyIdDelegate(string lobbyID);
    public static GetLobbyIdDelegate GetLobbyId;

    [SerializeField] private string lobbyID;

    [SerializeField] private GameObject playerNameParent;
    [SerializeField] private ScriptablePrefab playerNamePrefab;

    private void OnEnable()
    {
        GetLobbyId += GetLobbyID;
    }

    private void OnDisable()
    {
        GetLobbyId -= GetLobbyID;
    }

    public void GetLobbyID(string _lobbyID)
    {
        lobbyID = _lobbyID;
    }

    public async void PlayerNamesLobbyDisplay()
    {
        // TODO: JUST USE THE MULTIPLAYERLOBBY.CS TO CHANGE NAMES!

        if (playerNameParent = null)
        {
            playerNameParent = FindFirstObjectByType<PlayerNameParent>().gameObject;
        }

        Lobby joinedLobby = await LobbyService.Instance.GetLobbyAsync(lobbyID);

        List<Player> players = joinedLobby.Players;

        foreach (Player player in players)
        {
            LobbyDisplay lobbyDisplay = new LobbyDisplay();

            lobbyDisplay.displayObjectPrefab = playerNamePrefab.prefab;

            GameObject newLobbyDisplay = Instantiate(lobbyDisplay.displayObjectPrefab);
            newLobbyDisplay.transform.SetParent(playerNameParent.transform);

        }
    }
}

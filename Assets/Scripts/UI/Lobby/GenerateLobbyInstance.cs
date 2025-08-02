using System.Collections.Generic;
using TMPro;
using Unity.Multiplayer.Widgets;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class GenerateLobbyInstance : RelayManager
{
    public delegate void NewLobbyDisplayIstance(string lobbyName, string playerCount, string maxPlayerCount, string lobbyID);
    public static NewLobbyDisplayIstance GenerateNewLobbyDisplayInstance;

    [SerializeField] private TextMeshProUGUI lobbyNameUI;
    [SerializeField] private TextMeshProUGUI playerCountUI;
    [SerializeField] private string lobbyID;

    private bool generatedInstance = false;

    private void OnEnable()
    {
        GenerateNewLobbyDisplayInstance += GenerateNewLobbyDisplayInstance_Event;
    }

    private void OnDisable()
    {
        GenerateNewLobbyDisplayInstance -= GenerateNewLobbyDisplayInstance_Event;
    }

    public void GenerateNewLobbyDisplayInstance_Event(string Name, string currentPlayerCount, string maxPlayerCount, string _lobbyID)
    {
        if (!generatedInstance)
        {
            lobbyNameUI.text = Name;
            playerCountUI.text = currentPlayerCount + " / " + maxPlayerCount;
            lobbyID = _lobbyID; 

            generatedInstance = true;
        }
    }

    public async void JoinPublicLobby()
    {
        try
        {
            Debug.Log("Attempting to join server");
            Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyID);

            QueryLobbiesOptions options = new QueryLobbiesOptions();
            QueryResponse lobbies = await LobbyService.Instance.QueryLobbiesAsync(options);
            List<Lobby> foundLobbies = lobbies.Results;

            foreach (Lobby currentLobby in foundLobbies)
            {
                Debug.Log(currentLobby.Players.Count.ToString());
            }

            joinCode = joinedLobby.Data["relayJoinCode"].Value;

            await StartClientWithRelay(joinCode, connectionType);

            UIDisplay.Instance.lobbySearchUI.SetActive(false); // turning off the searching for game UI
            UIDisplay.Instance.inLobbyUI.SetActive(true); // turning on lobby UI
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}

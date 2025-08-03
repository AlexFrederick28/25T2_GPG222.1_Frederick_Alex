using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Android.Gradle.Manifest;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class LobbyCreator : RelayManager
{
    [SerializeField] private string lobbyName = "new lobby";
    CreateLobbyOptions options = new CreateLobbyOptions();
    [SerializeField] private string lobbyID;

    [SerializeField] private GameObject lobbyDisplayParent;
    [SerializeField] private ScriptablePrefab lobbyDisplayPrefab;

    public async void CreatePublicLobby()
    {
        options.IsPrivate = false;

        Debug.Log("Starting Host");
        await StartHostWithRelay(maxConnections, connectionType);

        Debug.Log("Creating Public Lobby");
        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxConnections, options);
        lobbyID = lobby.Id;

        await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, new UpdateLobbyOptions
        {
            Data = new Dictionary<string, DataObject> {

                {"relayJoinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode) }
            }
        });
    }

    public async void LoginClient()
    {
        loadingScreen.SetActive(true);

        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Player Signed in as: " + AuthenticationService.Instance.PlayerId);
        }

        loadingScreen.SetActive(false);
    }

    public void LogOut()
    {
        loadingScreen.SetActive(true);

        if (AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log("Player Signing Out: " + AuthenticationService.Instance.PlayerId);
            AuthenticationService.Instance.SignOut();
        }

        loadingScreen.SetActive(false);
    }

    public async void JoinPublicLobby(string _lobbyID)
    {
        try
        {
            Debug.Log("Attempting to join server");
            Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(_lobbyID);
            Debug.Log("Joined Server");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void UpdatePlayerData()
    {
        try
        {
            UpdatePlayerOptions options = new UpdatePlayerOptions();

            options.Data = new Dictionary<string, PlayerDataObject>()
            {   
                {
                    "existing data key", new PlayerDataObject(
                    visibility: PlayerDataObject.VisibilityOptions.Private,
                    value: "updated data value")
                },
                {
                    "new data key", new PlayerDataObject(
                    visibility: PlayerDataObject.VisibilityOptions.Public,
                    value: "new data value")
                }
            };

            //Ensure you sign-in before calling Authentication Instance
            //See IAuthenticationService interface
            string playerId = AuthenticationService.Instance.PlayerId;

            var lobby = await LobbyService.Instance.UpdatePlayerAsync(lobbyID, playerId, options);

            //...
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void SearchForLobbies()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;

            // Filter for open lobbies only
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(
                field: QueryFilter.FieldOptions.AvailableSlots,
                op: QueryFilter.OpOptions.GT,
                value: "0")
            };

            // Order by newest lobbies first
            options.Order = new List<QueryOrder>()
            {
                new QueryOrder(
                asc: false,
                field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbies = await LobbyService.Instance.QueryLobbiesAsync(options);

            //...

            List<Lobby> foundLobbies = lobbies.Results;

            foreach (Lobby currentLobby in foundLobbies)
            {
                LobbyDisplay lobbyDisplay = new LobbyDisplay();

                lobbyDisplay.displayObjectPrefab = lobbyDisplayPrefab.prefab;

                GameObject newLobbyDisplay = Instantiate(lobbyDisplay.displayObjectPrefab);
                GenerateLobbyInstance.GenerateNewLobbyDisplayInstance.Invoke(currentLobby.Name, currentLobby.Players.Count.ToString(), currentLobby.MaxPlayers.ToString(), currentLobby.Id);
                newLobbyDisplay.transform.SetParent(lobbyDisplayParent.transform);

            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}

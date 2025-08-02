using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Starts a server using Unity's relay system
/// </summary>
public class RelayManager : MonoBehaviour
{
    [SerializeField] protected int maxConnections = 10;
    [SerializeField] protected string connectionType = "udp";
    [SerializeField] protected string joinCode;

    [SerializeField] protected TextMeshProUGUI joinCodeText;
    [SerializeField] protected TMP_InputField inputJoinCode;
    [SerializeField] protected GameObject loadingScreen;

    public static RelayManager Instance { get; private set; }

    private void OnEnable()
    {
        Instance = this;
    }

    public async void StartHost()
    {
        Debug.Log("Starting Host");
        await StartHostWithRelay(maxConnections, connectionType);

        joinCodeText.text = joinCode;
    }

    public async void StartClient()
    {
        joinCode = inputJoinCode.text;
        Debug.Log("Joining with code: " + joinCode);
        await StartClientWithRelay(joinCode, connectionType);
    }

    public async Task<string> StartHostWithRelay(int maxConnections, string connectionType)
    {
        loadingScreen.SetActive(true);

        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        Debug.Log("Loading");

        var allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, connectionType));
        joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        loadingScreen.SetActive(false);

        return NetworkManager.Singleton.StartHost() ? joinCode : null;
    }

    public async Task<bool> StartClientWithRelay(string joinCode, string connectionType)
    {
        Instance.loadingScreen.SetActive(true);
        //loadingScreen.SetActive(true);

        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode: joinCode);
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, connectionType));

        Instance.loadingScreen.SetActive(false);
        //loadingScreen.SetActive(false);

        return !string.IsNullOrEmpty(joinCode) && NetworkManager.Singleton.StartClient();
    }
}

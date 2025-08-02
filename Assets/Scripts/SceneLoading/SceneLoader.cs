using Unity.Netcode;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loads scenes for transitioning.
/// </summary>
public class SceneLoader : NetworkBehaviour
{
    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    public void LoadGameScene_RPC()
    {
        SceneManager.LoadScene(2);
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    public void LoadMainMenu_RPC()
    {
        SceneManager.LoadScene(0);
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    public void LoadLobbyScene_RPC()
    {
        SceneManager.LoadScene(1);
    }
}

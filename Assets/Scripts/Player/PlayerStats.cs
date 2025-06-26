using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private int playerIndex;
    [SerializeField] private MeshRenderer pilotMeshRenderer;
    
    private Material material;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        MultiplayerLobby.playerJoin.Invoke(gameObject);

        playerIndex = (int)OwnerClientId;
        SetPilotColour(MultiplayerLobby.instance.GetPlayerColour(playerIndex));
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        MultiplayerLobby.playerLeave.Invoke(gameObject);
    }

    private void Awake()
    {
        material = new Material(pilotMeshRenderer.material);
        pilotMeshRenderer.material = material;
    }

    public void SetPilotColour(Color color)
    {
        material.color = color;
    }
}

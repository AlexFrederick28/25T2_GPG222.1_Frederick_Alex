using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Assigns the player with their unique index number, as well as sends a signal to the multiplayer lobby for joining and leaving. Sets the colour of the player pilot through the multiplayer lobby colours. Sets spawn position through PlayerPositioning.cs upon entering the game scene.
/// </summary>
public class PlayerStats : NetworkBehaviour
{
    public int playerIndex;
    [SerializeField] private MeshRenderer pilotMeshRenderer;
    
    private Material material;

    private bool isInSpawnPosition = false;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        MultiplayerLobby.playerJoin.Invoke(gameObject);

        playerIndex = (int)OwnerClientId;
        SetPilotColour(MultiplayerLobby.instance.GetPlayerColour(playerIndex)); // setting the colour of the pilot using the player index in relation to the colour from the multiplayer lobby colour list
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

    private void Update()
    {
        SetPlayerSpawnPosition();
    }

    private void SetPlayerSpawnPosition()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        Scene gameScene = SceneManager.GetSceneByBuildIndex(2);

        if (currentScene == gameScene && isInSpawnPosition == false)
        {
            PlayerPositioning.SetSpawnPositionEvent.Invoke(gameObject);

            isInSpawnPosition = true;
        }
    }
}

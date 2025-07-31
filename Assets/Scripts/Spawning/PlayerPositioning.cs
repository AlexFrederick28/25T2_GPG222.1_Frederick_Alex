using Unity.Netcode;
using UnityEngine;

public class PlayerPositioning : NetworkBehaviour
{
    [SerializeField] private GameObject spawnPositionOne;
    [SerializeField] private GameObject spawnPositionTwo;

    private bool setSpawnPositions = false;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        SetPlayerPositions_RPC();
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Unreliable, RequireOwnership = true)]
    private void SetPlayerPositions_RPC()
    {
        foreach (GameObject player in MultiplayerLobby.instance.playersInLobby)
        {
            Vector3 spawnPos = new Vector3(Random.Range(spawnPositionOne.transform.position.x, spawnPositionTwo.transform.position.x), Random.Range(spawnPositionOne.transform.position.y, spawnPositionTwo.transform.position.y), Random.Range(spawnPositionOne.transform.position.z, spawnPositionTwo.transform.position.z));

            player.transform.position = spawnPos;
        }
    }
}

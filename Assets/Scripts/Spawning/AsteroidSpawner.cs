using Unity.Netcode;
using UnityEngine;

public class AsteroidSpawner : NetworkBehaviour
{
    [SerializeField] private int number;
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private GameObject prefab;

    private void FixedUpdate()
    {
        if (IsServer)
        {
            Spawn_RPC();
        }
        if (IsLocalPlayer)
        {
            Spawn_RPC();
            Debug.Log("spawned");
        }
    }

    [Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    public void Spawn_RPC()
    {
        if (spawnOnStart)
        {
            for (int i = 0; i < number; i++)
            {
                float spawnX = Random.Range(transform.position.x + 50, transform.position.x - 50);
                float spawnY = Random.Range(transform.position.y + 50, transform.position.y - 50);
                float spawnZ = Random.Range(transform.position.z + 50, transform.position.z - 50);
                Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

                GameObject newAsteroid = Instantiate(prefab, spawnPosition, Quaternion.identity);
                newAsteroid.GetComponent<NetworkObject>().Spawn();
            }
        }

        spawnOnStart = false;
    }
}

using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Manages network objects that need to be spawned in. (Usually when loading into a new scene)
/// </summary>
public class PersistenceManager : NetworkBehaviour
{
    public delegate void PersistenceEvents(GameObject go);
    public static PersistenceEvents SpawnNetworkObject;

    private void OnEnable()
    {
        SpawnNetworkObject += SpawnNetworkObject_Event;
    }
    private void OnDisable()
    {
        SpawnNetworkObject -= SpawnNetworkObject_Event;
    }

    private void SpawnNetworkObject_Event(GameObject gameObject)
    {
        if (IsHost || IsServer)
        {
            if (gameObject.GetComponent<NetworkObject>().IsSpawned == false) // TODO: This doesnt work to spawn network objects... Fix it
            {
                gameObject.GetComponent<NetworkObject>().Spawn();
            }
        }
    }
}

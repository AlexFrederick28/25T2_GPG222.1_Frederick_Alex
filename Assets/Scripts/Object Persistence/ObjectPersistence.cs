using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class ObjectPersistence : MonoBehaviour
{
    private NetworkObject networkObject;
    [SerializeField] private bool dontDestroyOnLoad = true;
    [SerializeField] private bool spawnNetworkObject = true;

    private void Start()
    {
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (spawnNetworkObject)
        {
            if (GetComponent<NetworkObject>() && networkObject == null)
            {
                networkObject = GetComponent<NetworkObject>();

                if (networkObject.IsSpawned == false)
                {
                    networkObject.Spawn();
                }
            }
        }
    }
}

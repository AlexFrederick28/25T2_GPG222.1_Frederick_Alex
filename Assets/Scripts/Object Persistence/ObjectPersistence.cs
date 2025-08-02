using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

/// <summary>
/// Gives the attached object the ability to not be destroyed across loading scenes, as well as spawns the network object when loading into a new scene.
/// </summary>
public class ObjectPersistence : MonoBehaviour
{
    private NetworkObject networkObject;
    [SerializeField] private bool dontDestroyOnLoad = true;
    [SerializeField] private bool spawnNetworkObject = true;
    [SerializeField] private bool destroyObjectWithScene = true;

    private void Start()
    {
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }

        if (spawnNetworkObject)
        {
            PersistenceManager.SpawnNetworkObject.Invoke(gameObject);
        }
    }

    private void Update()
    {
        if (!destroyObjectWithScene)
        {
            if (networkObject == null)
            {
                networkObject = GetComponent<NetworkObject>();
            }
            if (networkObject != null)
            {
                if (dontDestroyOnLoad && networkObject.DestroyWithScene == true)
                {
                    networkObject.DestroyWithScene = false;
                }
            }
        }
    }

}

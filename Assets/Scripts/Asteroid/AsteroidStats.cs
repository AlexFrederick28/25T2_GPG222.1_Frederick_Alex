using UnityEngine;
using Unity.Netcode;
using UnityEngine.Rendering.Universal;
using Unity.Services.Matchmaker.Models;

/// <summary>
/// Gathers the players index from collisions of their projectile, setting the colour as well as splitting the asteroid if possible.
/// </summary>
public class AsteroidStats : NetworkBehaviour
{
    public Color colour;
    public NetworkVariable<int> ownerID = new NetworkVariable<int>(0);
    [SerializeField] private Material material;
    [SerializeField] private MeshRenderer mesh;

    [SerializeField] private bool isStageOne = false;
    [SerializeField] private bool isStageTwo = false;
    [SerializeField] private GameObject stageTwoAsteroid;
    [SerializeField] private int maxSplitAmount = 2;

    private void Awake()
    {
        material = new Material(mesh.material); // getting reference to a material to use to apply colours

        if (isStageTwo)
        {
            stageTwoAsteroid = null;
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        ownerID.OnValueChanged += OnOwnerIDChanged;
    }

    private void OnOwnerIDChanged(int oldValue, int newValue)
    {
        ApplyOwnerColour_RPC();
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
    public void ApplyOwnerColour_RPC()
    {
        SetOwnerColour_RPC(MultiplayerLobby.instance.GetPlayerColour(ownerID.Value));

        mesh.material = material;
    }

    [Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
    private void GetOwnerID_RPC(int ID)
    {
        ownerID.Value = ID;
        Debug.Log("Got Owner ID: " + ownerID.Value);
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
    private void SetOwnerColour_RPC(Color color)
    {
        material.color = color;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (IsServer)
        {
            SplitAsteroid_RPC();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<RocketBehaviour>())
        {
            GetOwnerID_RPC(collision.gameObject.GetComponent<RocketBehaviour>().ownerID);
            ApplyOwnerColour_RPC();

            if (isStageOne)
            {
                Destroy(gameObject);
            }
        }
    }

    [Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    private void SplitAsteroid_RPC()
    {
        if (isStageOne)
        {
            for (int i = 0; i < maxSplitAmount; i++)
            {
                GameObject newAsteroid = Instantiate(stageTwoAsteroid, transform.position, Quaternion.identity);
                newAsteroid.GetComponent<NetworkObject>().Spawn();
                newAsteroid.GetComponent<AsteroidStats>().ownerID.Value = ownerID.Value;
                newAsteroid.GetComponent<AsteroidStats>().ApplyOwnerColour_RPC();
            }
        }
    }
}

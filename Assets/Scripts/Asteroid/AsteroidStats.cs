using UnityEngine;
using Unity.Netcode;
using UnityEngine.Rendering.Universal;
using Unity.Services.Matchmaker.Models;

public class AsteroidStats : NetworkBehaviour
{
    public Color colour;
    public int ownerID;
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

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    public void ApplyOwnerColour_RPC()
    {
        SetOwnerColour(MultiplayerLobby.instance.GetPlayerColour(ownerID));

        mesh.material = material;
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    private void GetOwnerID_RPC(int ID)
    {
        Debug.Log("Got Owner ID");
        ownerID = ID;
    }

    private void SetOwnerColour(Color color)
    {
        material.color = color;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        SplitAsteroid_RPC();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<RocketBehaviour>())
        {
            ownerID = collision.gameObject.GetComponent<RocketBehaviour>().ownerID; 
            GetOwnerID_RPC(ownerID);
            ApplyOwnerColour_RPC();
            Destroy(gameObject);
        }
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    private void SplitAsteroid_RPC()
    {
        if (isStageOne)
        { 
            for (int i = 0; i < maxSplitAmount; i++)
            {
                GameObject newAsteroid = Instantiate(stageTwoAsteroid, transform.position, Quaternion.identity);
                newAsteroid.GetComponent<NetworkObject>().Spawn();
                newAsteroid.GetComponent<AsteroidStats>().ownerID = ownerID;
                newAsteroid.GetComponent<AsteroidStats>().ApplyOwnerColour_RPC();
            }
        }
    }
}

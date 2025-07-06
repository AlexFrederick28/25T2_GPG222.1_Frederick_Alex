using UnityEngine;
using Unity.Netcode;
using UnityEngine.Rendering.Universal;

public class AsteroidStats : NetworkBehaviour
{
    [SerializeField] private Color colour;
    public int ownerID;
    [SerializeField] private Material material;
    [SerializeField] private MeshRenderer mesh;

    private void Awake()
    {
        material = new Material(mesh.material); // getting reference to a material to use to apply colours
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    private void ApplyOwnerColour_RPC()
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<RocketBehaviour>())
        {
            ownerID = collision.gameObject.GetComponent<RocketBehaviour>().ownerID; 
            GetOwnerID_RPC(ownerID);
            ApplyOwnerColour_RPC();
        }
    }
}

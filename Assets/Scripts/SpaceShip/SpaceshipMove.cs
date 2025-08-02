using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Moves the spaceship forward using forces and Unity's new input system
/// </summary>
public class SpaceshipMove : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    [SerializeField] private InputAction moveAction;

    public override void OnNetworkSpawn()
    {
        gameObject.name += " [ " + OwnerClientId + " ] ";
        moveAction.Enable();

        base.OnNetworkSpawn();
    }

    public override void OnNetworkDespawn()
    {
        moveAction.Disable();
        base.OnNetworkDespawn();
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            if (moveAction.IsPressed())
            {
                RequestMoveForward_RPC();
            }
        }
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Unreliable, RequireOwnership = false)]
    private void MoveForward_RPC() // strictly used to move the spaceship forward using forces
    {
        rb.AddRelativeForce(0, 0, speed, ForceMode.Acceleration);
    }

    [Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    private void RequestMoveForward_RPC()
    {
        MoveForward_RPC();
    }

}

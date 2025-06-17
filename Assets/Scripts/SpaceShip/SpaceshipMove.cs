using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipMove : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    [SerializeField] private InputAction moveAction;

    //private void OnEnable()
    //{
    //    moveAction.Enable();
    //}

    //private void OnDisable()
    //{
    //    moveAction.Disable();
    //}

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

    // this update is used for testing
    private void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            if (moveAction.IsPressed())
            {
                RequestMoveForward_RPC();
            }
        }
        //if (IsClient)
        //{
        //    MoveForward_RPC();
        //}
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
    private void MoveForward_RPC() // strictly used to move the spaceship forward using forces
    {
        rb.AddRelativeForce(0, 0, speed, ForceMode.Acceleration);

        Debug.Log("Moving forward");

        //if (moveAction.IsPressed())
        //{
        //    rb.AddRelativeForce(0, 0, speed, ForceMode.Acceleration);

        //    Debug.Log("Moving forward");
        //}
        //else
        //{
        //    rb.AddRelativeForce(0, 0, 0, 0);
        //}
    }

    [Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    private void RequestMoveForward_RPC()
    {
        MoveForward_RPC();
    }

}

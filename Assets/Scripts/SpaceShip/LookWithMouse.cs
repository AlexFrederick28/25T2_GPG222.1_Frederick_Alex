using System.Threading;
using TreeEditor;
using Unity.Netcode;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;


public class LookWithMouse : NetworkBehaviour
{
    [SerializeField] private Vector2 mouseTurn;
    [SerializeField] private Vector3 mousePosition;
    [SerializeField] private Vector3 mouseView;
    [SerializeField] private Camera aimCamera;
    [SerializeField] private float sensitivity;

    [SerializeField] private InputAction lookActionX;
    [SerializeField] private InputAction lookActionY;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            aimCamera.gameObject.SetActive(true);
        }

        lookActionX.Enable();
        lookActionY.Enable();

        base.OnNetworkSpawn();
    }

    public override void OnNetworkDespawn()
    {
        lookActionX.Disable();
        lookActionY.Disable();

        base.OnNetworkDespawn();
    }

    private void FixedUpdate()
    {
        if (IsLocalPlayer)
        {
            LockMouseToScreen();

            if (lookActionX.IsPressed() || lookActionY.IsPressed())
            {
                Debug.Log("Rotating");
                ReadMouseInputs();
                SendControlsToServer_RPC(mouseView);
            }
        }
    }

    [Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
    private void SyncRotation_Rpc(Vector3 _mouseView)
    {
        transform.rotation = Quaternion.Euler(_mouseView);
    }

    private void ReadMouseInputs()
    {
        mouseTurn.x += lookActionX.ReadValue<float>();
        mouseTurn.y += lookActionY.ReadValue<float>();
        //mousePosition = new Vector2(mouseTurn.x, mouseTurn.y); // shows values in inspector

        if (transform.up.y < 0)
        {
            mouseView = new Vector3(-mouseTurn.y, -mouseTurn.x, 0);
        }
        else
        {
            mouseView = new Vector3(-mouseTurn.y, mouseTurn.x, 0);
        }
    }

    [Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    private void SendControlsToServer_RPC(Vector3 _mouseView)
    {
        SyncRotation_Rpc(_mouseView);
    }

    private void LockMouseToScreen()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }






    // OLD INPUT CODE USED -- KEPT FOR BACKUP

    //if (transform.rotation.x > 0)
    //{
    //    mousePosition = lookAction.ReadValue<Vector2>() * sensitivity;
    //    Quaternion rotationY = Quaternion.Euler(-mousePosition.y, 0, 0);
    //    Quaternion rotationX = Quaternion.Euler(0, mousePosition.x, 0);

    //    transform.localRotation = rotationX * rotationY;

    //    Debug.Log("Should be upright");
    //}
    //else
    //{
    //    mousePosition = lookAction.ReadValue<Vector2>() * sensitivity;
    //    Quaternion rotationY = Quaternion.Euler(-mousePosition.y, 0, 0);
    //    Quaternion rotationX = Quaternion.Euler(0, -mousePosition.x, 0);

    //    transform.localRotation = rotationX * rotationY;

    //    Debug.Log("Should be upside down");
    //}
}

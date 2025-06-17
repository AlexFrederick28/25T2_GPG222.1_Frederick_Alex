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
    [SerializeField] private Camera aimCamera;
    [SerializeField] private float sensitivity;

    [SerializeField] private InputAction lookActionX;
    [SerializeField] private InputAction lookActionY;

    private void OnEnable()
    {
        lookActionX.Enable();
        lookActionY.Enable();
    }

    private void OnDisable()
    {
        lookActionX.Disable();
        lookActionY.Disable();
    }

    private void FixedUpdate()
    {
        CameraToMousePosition();
        LockMouseToScreen();
    }

    private void CameraToMousePosition()
    {

        

        if (transform.up.y < 0)
        {
            mouseTurn.x += lookActionX.ReadValue<float>();
            mouseTurn.y += lookActionY.ReadValue<float>();
            mousePosition = new Vector2(mouseTurn.x, mouseTurn.y); // shows values in inspector
            Vector3 mouseView = new Vector3(-mouseTurn.y, -mouseTurn.x, 0);
            transform.rotation = Quaternion.Euler(mouseView);
        }
        else
        {
            mouseTurn.x += lookActionX.ReadValue<float>();
            mouseTurn.y += lookActionY.ReadValue<float>();
            mousePosition = new Vector2(mouseTurn.x, mouseTurn.y); // shows values in inspector
            Vector3 mouseView = new Vector3(-mouseTurn.y, mouseTurn.x, 0);
            transform.rotation = Quaternion.Euler(mouseView);
        }
    }

    private void LockMouseToScreen()
    {
        // undecided whether or not the mouse can actually be locked as the mouse needs to move to grab the position

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

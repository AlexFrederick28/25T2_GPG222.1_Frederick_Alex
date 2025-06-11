using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;


public class LookWithMouse : MonoBehaviour
{
    [SerializeField] private Vector3 mousePosition;
    [SerializeField] private Camera aimCamera;
    [SerializeField] private float sensitivity;

    [SerializeField] private InputAction lookAction;

    private void OnEnable()
    {
        lookAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();
    }

    private void FixedUpdate()
    {
        CameraToMousePosition();
        //LockMouseToScreen();
    }

    private void CameraToMousePosition()
    {
        mousePosition = lookAction.ReadValue<Vector2>() * sensitivity;
        Quaternion rotationY = Quaternion.Euler(-mousePosition.y, 0, 0);
        Quaternion rotationX = Quaternion.Euler(0, mousePosition.x, 0);

        transform.rotation = rotationX * rotationY;
    }

    private void LockMouseToScreen()
    {
        // undecided whether or not the mouse can actually be locked as the mouse needs to move to grab the position
    }
}

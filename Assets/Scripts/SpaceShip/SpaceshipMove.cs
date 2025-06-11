using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipMove : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    [SerializeField] private InputAction moveAction;

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    // this update is used for testing
    private void Update()
    {
        MoveForward();
    }

    private void MoveForward() // strictly used to move the spaceship forward using forces
    {
        if (moveAction.IsPressed())
        {
            rb.AddRelativeForce(0, 0, speed, ForceMode.Acceleration);
        }
        else
        {
            rb.AddRelativeForce(0, 0, 0, 0);
        }
    }

}

using UnityEngine;

public class SpaceshipMove : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    // this update is used for testing
    private void Update()
    {
        MoveForward();
    }

    private void MoveForward() // strictly used to move the spaceship forward using forces
    {
        rb.AddRelativeForce(0, 0, speed, ForceMode.Acceleration);
    }

}

using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Rotates the asteroids model, as well as moves the asteroid using forces 
/// </summary>
public class AsteroidMovement : NetworkBehaviour
{

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform mesh;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;

    private bool applyRandomRotation = false;
    [SerializeField] private bool collided = false;

    private void FixedUpdate()
    {
        StartWithRandomRotation();
        RotateAsteroidVisual();
        MoveAsteroid();
        RotateAsteroidOnCollision();
    }

    private void RotateAsteroidVisual()
    {
        mesh.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void MoveAsteroid()
    {
        rb.AddRelativeForce(0, 0, moveSpeed, ForceMode.VelocityChange);
    }

    private void RotateAsteroidOnCollision()
    {
        if (collided)
        {
            if (transform.rotation.x > 0)
            {
                transform.rotation = Quaternion.Euler(Random.Range(-180f, 0), Random.Range(-180f, 0), Random.Range(-180f, 0));
            }
            else
            {
                transform.rotation = Quaternion.Euler(Random.Range(0, 180f), Random.Range(0, 180f), Random.Range(0, 180f));
            } 
        }
    }

    private void StartWithRandomRotation()
    {
        if (!applyRandomRotation)
        {
            Quaternion rotation = Quaternion.Euler(Random.Range(1f, 180f), Random.Range(1f, 180f), 0);

            transform.rotation = rotation;

            applyRandomRotation = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       collided = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        collided = false;
    }
}

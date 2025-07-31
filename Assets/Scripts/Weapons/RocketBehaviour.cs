using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;

public class RocketBehaviour : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private Color colour;
    public int ownerID;
    [SerializeField] private List<MeshRenderer> rocketMesh;
    [SerializeField] private Material material;
    [SerializeField] private Collider trigerCollider;

    private void Awake()
    {
        material = new Material(rocketMesh[0].material); // getting reference to a material to use to apply colours
    }

    private void Update()
    {
        MoveForward();

        Destroy(gameObject, 15);
    }

    private void MoveForward()
    {
        rb.AddRelativeForce(0, 0, speed, ForceMode.VelocityChange);
    }

    private void ApplyOwnerColour()
    {
        SetOwnerColour(MultiplayerLobby.instance.GetPlayerColour(ownerID));

        foreach (var mesh in rocketMesh)
        {
            mesh.material = material;
        }
    }

    private void SetOwnerColour(Color color)
    {
        material.color = color;
    }

    private void OnTriggerEnter(Collider other) // getting the player ID who spawned the rocket and setting the rockets colour
    {
        if (other.gameObject.GetComponent<PlayerStats>())
        {
            ownerID = other.GetComponent<PlayerStats>().playerIndex;
            ApplyOwnerColour();
            trigerCollider.enabled = false; // ensures that no other random player takes ownership of the rocket
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}

using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipWeapon : NetworkBehaviour
{
    [SerializeField] private Transform gunTransform;
    [SerializeField] private GameObject rocketPrefab;

    [SerializeField] private float fireRate;
    private float shootTimer;

    [SerializeField] private InputAction shootAction;

    private void OnEnable()
    {
        shootAction.Enable();

        shootTimer = fireRate; // ensures the player can shoot on start, rather than waiting the initial timer -- subject to change
    }

    private void OnDisable()
    {
        shootAction.Disable();
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer > fireRate)
            {
                if (shootAction.IsPressed())
                {
                    SpawnBullet_RPC();

                    shootTimer = 0;
                }
            }
        }
    }

    [Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable, RequireOwnership = true)]
    private void SpawnBullet_RPC()
    {
        GameObject newRocket = Instantiate(rocketPrefab, gunTransform);
        newRocket.GetComponent<NetworkObject>().Spawn();
    }
}

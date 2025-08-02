using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Sets the area of which players can be spawned in.
/// </summary>
public class PlayerPositioning : NetworkBehaviour
{
    [SerializeField] private GameObject spawnPositionOne;
    [SerializeField] private GameObject spawnPositionTwo;

    public delegate void PlayerPositioningDelegate(GameObject go);
    public static PlayerPositioningDelegate SetSpawnPositionEvent;

    private void OnEnable()
    {
        SetSpawnPositionEvent += SetPlayerSpawnPosition;
    }
    private void OnDisable()
    {
        SetSpawnPositionEvent -= SetPlayerSpawnPosition;
    }

    private void SetPlayerSpawnPosition(GameObject go)
    {
        Vector3 spawnPos = new Vector3(Random.Range(spawnPositionOne.transform.position.x, spawnPositionTwo.transform.position.x), Random.Range(spawnPositionOne.transform.position.y, spawnPositionTwo.transform.position.y), Random.Range(spawnPositionOne.transform.position.z, spawnPositionTwo.transform.position.z));

        go.transform.position = spawnPos;

        Debug.Log("Set " + go.name + " Position");
    }
}

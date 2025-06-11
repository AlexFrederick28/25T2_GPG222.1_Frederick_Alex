using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private int number;
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private GameObject prefab;

    private void FixedUpdate()
    {
        Spawn();
    }

    public void Spawn()
    {
        if (spawnOnStart)
        {
            for (int i = 0; i < number; i++)
            {
                float spawnX = Random.Range(transform.position.x + 50, transform.position.x - 50);
                float spawnY = Random.Range(transform.position.y + 50, transform.position.y - 50);
                float spawnZ = Random.Range(transform.position.z + 50, transform.position.z - 50);
                Vector3 spawnPosition = new Vector3 (spawnX, spawnY, spawnZ);

                Instantiate(prefab, spawnPosition, Quaternion.identity);
            }
        }

        spawnOnStart = false;
    }
}

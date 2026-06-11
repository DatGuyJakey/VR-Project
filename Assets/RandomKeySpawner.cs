using UnityEngine;

// This script moves a key to one random spawn point when the scene starts.
// Place this script on an empty GameObject such as FirstKeySpawner.
public class RandomKeySpawner : MonoBehaviour
{
    [Header("Key to Spawn")]
    [Tooltip("Drag the key GameObject here, for example Key01_RoomKey.")]
    public GameObject keyObject;

    [Header("Possible Spawn Points")]
    [Tooltip("Drag all possible key hiding places into this list.")]
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public bool resetRotationToSpawnPoint = true;
    public bool wakeRigidbodyAfterSpawn = true;

    // Other scripts can read this to know where the key spawned.
    public Transform ChosenSpawnPoint { get; private set; }

    void Start()
    {
        SpawnKeyAtRandomPoint();
    }

    public void SpawnKeyAtRandomPoint()
    {
        if (keyObject == null)
        {
            Debug.LogError("RandomKeySpawner: No keyObject assigned.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("RandomKeySpawner: No spawn points assigned.");
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        ChosenSpawnPoint = spawnPoints[randomIndex];

        if (ChosenSpawnPoint == null)
        {
            Debug.LogError("RandomKeySpawner: One of the spawn points is missing.");
            return;
        }

        if (resetRotationToSpawnPoint)
        {
            keyObject.transform.SetPositionAndRotation(
                ChosenSpawnPoint.position,
                ChosenSpawnPoint.rotation
            );
        }
        else
        {
            keyObject.transform.position = ChosenSpawnPoint.position;
        }

        keyObject.SetActive(true);

        Rigidbody rb = keyObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            if (wakeRigidbodyAfterSpawn)
            {
                rb.WakeUp();
            }
        }

        Debug.Log("Key spawned at: " + ChosenSpawnPoint.name);
    }
}
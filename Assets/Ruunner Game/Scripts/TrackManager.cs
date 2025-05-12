using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{
    public GameObject[] trackPrefabs;     // Different track variations
    public GameObject[] obstaclePrefabs;  // Obstacle prefabs to spawn
    public Transform player;              // Reference to the player
    public float trackLength = 30f;       // Length of each track piece
    public float spawnThreshold = 25f;    // Distance to player before spawning
    public float obstacleChance = 0.5f;   // Chance of spawning an obstacle

    private Vector3 nextSpawnPosition = Vector3.zero;
    private Queue<GameObject> spawnedTracks = new Queue<GameObject>();

    void Start()
    {
        SpawnInitialTracks();
    }

    void Update()
    {
        if (Vector3.Distance(player.position, nextSpawnPosition) < spawnThreshold)
        {
            SpawnTrack(Random.Range(0, trackPrefabs.Length));
        }

        CleanUpTracks();
    }

    // Spawn initial tracks
    void SpawnInitialTracks()
    {
        for (int i = 0; i < trackPrefabs.Length; i++)
        {
            SpawnTrack(i == 0 ? 0 : Random.Range(0, trackPrefabs.Length));
        }
    }

    // Spawn a new track
    void SpawnTrack(int index)
    {
        GameObject newTrack = Instantiate(trackPrefabs[index], nextSpawnPosition, Quaternion.identity);
        spawnedTracks.Enqueue(newTrack);
        nextSpawnPosition += Vector3.forward * trackLength;

        // Spawn obstacles or collectibles on the new track
        if (obstaclePrefabs != null && obstaclePrefabs.Length > 0)
        {
            if (Random.value < obstacleChance)
            {
                GameObject obstacle = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], newTrack.transform);
                // Example: Set position of the obstacle on the track
                obstacle.transform.localPosition = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-1f, 1f));
            }
        }
    }

    // Cleanup track segments that are behind the player
    void CleanUpTracks()
    {
        // Check if the track segment is behind the player (in the negative Z-direction)
        if (spawnedTracks.Count > 0)
        {
            GameObject oldestTrack = spawnedTracks.Peek();
            if (oldestTrack.transform.position.z + trackLength < player.position.z)
            {
                Destroy(oldestTrack);
                spawnedTracks.Dequeue();
            }
        }
    }
}


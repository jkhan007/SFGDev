using UnityEngine;
using System.Collections.Generic;

public class TrackManager : MonoBehaviour
{
    public GameObject[] trackPrefabs;     // Different track variations
    public int initialTracks = 5;         // How many tracks to spawn initially
    public Transform player;              // Reference to the player
    public float trackLength = 30f;       // Length of each track piece
    public int maxTracksOnScreen = 7;     // For cleanup
    public float spawnThreshold = 25f;    // Distance to player before spawning

    private Vector3 nextSpawnPosition = Vector3.zero;
    private Queue<GameObject> spawnedTracks = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < initialTracks; i++)
        {
            SpawnTrack(i == 0 ? 0 : Random.Range(0, trackPrefabs.Length));
        }
    }

    void Update()
    {
        if (Vector3.Distance(player.position, nextSpawnPosition) < spawnThreshold)
        {
            SpawnTrack(Random.Range(0, trackPrefabs.Length));
            if (spawnedTracks.Count > maxTracksOnScreen)
            {
                Destroy(spawnedTracks.Dequeue());
            }
        }
    }

    void SpawnTrack(int index)
    {
        GameObject newTrack = Instantiate(trackPrefabs[index], nextSpawnPosition, Quaternion.identity);
        spawnedTracks.Enqueue(newTrack);
        nextSpawnPosition += Vector3.forward * trackLength;
    }
}


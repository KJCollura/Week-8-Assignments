using UnityEngine;
using System.Collections.Generic;

public class TreeRockSpawner : MonoBehaviour
{
    public Terrain terrain;

    [Header("Tree & Rock Objects")]
    public GameObject[] trees;
    public GameObject[] rocks;

    [Header("Spawn Counts")]
    public int treeCount = 30;
    public int rockCount = 40;

    [Header("Spawn Settings")]
    public float minTreeDistance = 5.0f;
    public float minRockDistance = 3.0f;

    private List<Vector3> treePositions = new List<Vector3>();
    private List<Vector3> rockPositions = new List<Vector3>();

    private void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("No Terrain assigned! Attach a Terrain to the TreeRockSpawner.");
            return;
        }

        SpawnObjects(rocks, rockCount, 0.0f, 0.3f, rockPositions, minRockDistance);
        SpawnObjects(trees, treeCount, 0.3f, 1.0f, treePositions, minTreeDistance, rockPositions);
    }

    void SpawnObjects(GameObject[] objectArray, int count, float minHeight, float maxHeight, List<Vector3> occupiedPositions, float minDistance, List<Vector3> avoidPositions = null)
    {
        if (objectArray.Length == 0) return;

        TerrainData terrainData = terrain.terrainData;
        int attempts = count * 10; // Prevent infinite loops
        
        for (int i = 0; i < count && attempts > 0; i++)
        {
            attempts--;
            
            float randomX = Random.Range(0, terrainData.size.x);
            float randomZ = Random.Range(0, terrainData.size.z);
            float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

            if (terrainHeight < minHeight * terrainData.size.y || terrainHeight > maxHeight * terrainData.size.y)
                continue;

            Vector3 position = new Vector3(randomX, terrainHeight, randomZ);

            // Ensure proper spacing
            if (IsTooClose(position, occupiedPositions, minDistance) || (avoidPositions != null && IsTooClose(position, avoidPositions, minDistance)))
                continue;

            GameObject objectToSpawn = objectArray[Random.Range(0, objectArray.Length)];
            GameObject spawnedObject = Instantiate(objectToSpawn, position, Quaternion.identity);

            // Get terrain normal for slope alignment
            Vector3 terrainNormal = terrainData.GetInterpolatedNormal(randomX / terrainData.size.x, randomZ / terrainData.size.z);
            spawnedObject.transform.position = position;
            spawnedObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, terrainNormal);

            if (System.Array.Exists(trees, tree => tree == objectToSpawn))
            {
                spawnedObject.transform.rotation *= Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                spawnedObject.transform.localScale *= Random.Range(3.5f, 5.5f); // Increase tree size
            }
            else if (System.Array.Exists(rocks, rock => rock == objectToSpawn))
            {
                spawnedObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, terrainNormal);
                spawnedObject.transform.localScale *= Random.Range(0.5f, 1.0f); // Scale rocks smaller
            }

            occupiedPositions.Add(position);
            Debug.Log($"Spawning {objectToSpawn.name} at {position} with normal {terrainNormal}");
        }
    }

    bool IsTooClose(Vector3 position, List<Vector3> otherPositions, float minDistance)
    {
        foreach (var otherPosition in otherPositions)
        {
            if (Vector3.Distance(position, otherPosition) < minDistance)
                return true;
        }
        return false;
    }
}


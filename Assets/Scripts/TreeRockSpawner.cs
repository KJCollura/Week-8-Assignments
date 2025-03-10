using UnityEngine;

public class TreeRockSpawner : MonoBehaviour
{
    public Terrain terrain;

    [Header("Tree & Rock Objects")]
    public GameObject[] trees;
    public GameObject[] rocks;

    [Header("Spawn Counts")]
    public int treeCount = 30;
    public int rockCount = 40;

    private void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("No Terrain assigned! Attach a Terrain to the TreeRockSpawner.");
            return;
        }

        SpawnObjects(trees, treeCount, 0.3f, 1.0f); // Trees spawn on higher terrain
        SpawnObjects(rocks, rockCount, 0.0f, 0.3f); // Rocks spawn in lower areas
    }

    void SpawnObjects(GameObject[] objectArray, int count, float minHeight, float maxHeight)
    {
        if (objectArray.Length == 0) return;

        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < count; i++)
        {
            float randomX = Random.Range(0, terrainData.size.x);
            float randomZ = Random.Range(0, terrainData.size.z);
            float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) / terrainData.size.y;

            if (terrainHeight < minHeight || terrainHeight > maxHeight)
                continue;

            Vector3 position = new Vector3(randomX, terrainHeight * terrainData.size.y, randomZ);
            GameObject objectToSpawn = objectArray[Random.Range(0, objectArray.Length)];
            GameObject spawnedObject = Instantiate(objectToSpawn, position, Quaternion.identity);

            // Get terrain normal for slope alignment
            Vector3 terrainNormal = terrainData.GetInterpolatedNormal(randomX / terrainData.size.x, randomZ / terrainData.size.z);

            if (System.Array.Exists(trees, tree => tree == objectToSpawn))
            {
                spawnedObject.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                spawnedObject.transform.localScale *= Random.Range(3.5f, 5.5f); // Increase tree size
            }
            else if (System.Array.Exists(rocks, rock => rock == objectToSpawn))
            {
                spawnedObject.transform.rotation = Quaternion.LookRotation(Vector3.Cross(spawnedObject.transform.right, terrainNormal), terrainNormal);
                spawnedObject.transform.localScale *= Random.Range(0.5f, 1.0f); // Scale rocks smaller
            }

            Debug.Log($"Spawning {objectToSpawn.name} at {position} with normal {terrainNormal}");
        }
    }
}

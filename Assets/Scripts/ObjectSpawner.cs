using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Terrain terrain;

    [Header("Objects to Spawn")]
    public GameObject[] flowers;
    public GameObject[] plants;
    public GameObject[] rocks;
    public GameObject[] grass;
    public GameObject[] trees;
    public GameObject[] mushrooms;

    [Header("Spawn Counts")]
    public int flowerCount = 50;
    public int plantCount = 75;
    public int rockCount = 40;
    public int grassCount = 100;
    public int treeCount = 30;
    public int mushroomCount = 60;

    private void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("No Terrain assigned! Attach a Terrain to the ObjectSpawner.");
            return;
        }

        SpawnObjects(flowers, flowerCount, 0.3f, 0.6f);
        SpawnObjects(plants, plantCount, 0.2f, 0.8f);
        SpawnObjects(rocks, rockCount, 0.0f, 0.3f);
        SpawnObjects(grass, grassCount, 0.4f, 0.9f);
        SpawnObjects(trees, treeCount, 0.3f, 1.0f); // Trees should spawn on higher terrain
        SpawnObjects(mushrooms, mushroomCount, 0.1f, 0.6f); // Mushrooms in mid-low height areas
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

            // Check if the terrain height is within the defined range for this object type
            if (terrainHeight < minHeight || terrainHeight > maxHeight)
                continue;

            // Get terrain normal (surface slope)
            Vector3 terrainNormal = terrainData.GetInterpolatedNormal(randomX / terrainData.size.x, randomZ / terrainData.size.z);

            Vector3 position = new Vector3(randomX, terrainHeight * terrainData.size.y, randomZ) + terrainNormal * 0.1f; // Slightly adjust position to match terrain

            GameObject objectToSpawn = objectArray[Random.Range(0, objectArray.Length)];
            GameObject spawnedObject = Instantiate(objectToSpawn, position, Quaternion.identity);
            
            // Rotate objects to match terrain
            if (trees.Length > 0 && System.Array.Exists(trees, tree => tree == objectToSpawn))
            {
                // Keep trees upright but randomize their Y rotation slightly
                spawnedObject.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                spawnedObject.transform.localScale *= Random.Range(1.5f, 2.5f); // Scale trees up
            }
            else if (rocks.Length > 0 && System.Array.Exists(rocks, rock => rock == objectToSpawn))
            {
                // Fully rotate rocks to match the terrain slope
                spawnedObject.transform.rotation = Quaternion.LookRotation(Vector3.Cross(spawnedObject.transform.right, terrainNormal), terrainNormal);
                spawnedObject.transform.localScale *= Random.Range(0.8f, 1.2f); // Scale rocks slightly
            }
            else if (flowers.Length > 0 && System.Array.Exists(flowers, flower => flower == objectToSpawn))
            {
                spawnedObject.transform.localScale *= Random.Range(1.2f, 1.8f); // Increase flower size
            }
            else if (plants.Length > 0 && System.Array.Exists(plants, plant => plant == objectToSpawn))
            {
                spawnedObject.transform.localScale *= Random.Range(1.2f, 1.8f); // Increase plant size
            }
            else if (grass.Length > 0 && System.Array.Exists(grass, g => g == objectToSpawn))
            {
                spawnedObject.transform.localScale *= Random.Range(1.2f, 1.8f); // Increase grass size
            }
            else if (mushrooms.Length > 0 && System.Array.Exists(mushrooms, mushroom => mushroom == objectToSpawn))
            {
                spawnedObject.transform.localScale *= Random.Range(1.2f, 1.8f); // Increase mushroom size
            }
            Debug.Log($"Attempting to spawn: {objectToSpawn.name} at {position} with height: {terrainHeight}");
            
            // Scale trees larger than rocks
            if (trees.Length > 0 && System.Array.Exists(trees, tree => tree == objectToSpawn))
            {
                spawnedObject.transform.localScale *= Random.Range(1.5f, 2.5f); // Scale trees up
            }
            else if (flowers.Length > 0 && System.Array.Exists(flowers, flower => flower == objectToSpawn))
            {
                spawnedObject.transform.localScale *= Random.Range(1.2f, 1.8f); // Increase flower size
            }
            else if (plants.Length > 0 && System.Array.Exists(plants, plant => plant == objectToSpawn))
            {
                spawnedObject.transform.localScale *= Random.Range(1.2f, 1.8f); // Increase plant size
            }
            else if (grass.Length > 0 && System.Array.Exists(grass, g => g == objectToSpawn))
            {
                spawnedObject.transform.localScale *= Random.Range(1.2f, 1.8f); // Increase grass size
            }
            else if (mushrooms.Length > 0 && System.Array.Exists(mushrooms, mushroom => mushroom == objectToSpawn))
            {
                spawnedObject.transform.localScale *= Random.Range(1.2f, 1.8f); // Increase mushroom size
            }
            else if (rocks.Length > 0 && System.Array.Exists(rocks, rock => rock == objectToSpawn))
            {
                spawnedObject.transform.localScale *= Random.Range(0.8f, 1.2f); // Scale rocks slightly
            }
        }
    }
}

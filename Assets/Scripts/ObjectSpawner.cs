using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Terrain terrain;

    [Header("Objects to Spawn")]
    public GameObject[] flowers;
    public GameObject[] plants;
    public GameObject[] rocks;
    public GameObject[] grass;

    [Header("Spawn Counts")]
    public int flowerCount = 50;
    public int plantCount = 75;
    public int rockCount = 40;
    public int grassCount = 100;

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

            Vector3 position = new Vector3(randomX, terrainHeight * terrainData.size.y, randomZ);

            GameObject objectToSpawn = objectArray[Random.Range(0, objectArray.Length)];
            Instantiate(objectToSpawn, position, Quaternion.identity);
        }
    }
}
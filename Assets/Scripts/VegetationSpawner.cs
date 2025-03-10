using UnityEngine;

public class VegetationSpawner : MonoBehaviour
{
    public Terrain terrain;

    [Header("Vegetation Objects")]
    public GameObject[] flowers;
    public GameObject[] plants;
    public GameObject[] grass;
    public GameObject[] mushrooms;

    [Header("Spawn Counts")]
    public int flowerCount = 50;
    public int plantCount = 75;
    public int grassCount = 100;
    public int mushroomCount = 60;

    private void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("No Terrain assigned! Attach a Terrain to the VegetationSpawner.");
            return;
        }

        SpawnObjects(flowers, flowerCount, 0.3f, 0.6f);
        SpawnObjects(plants, plantCount, 0.2f, 0.8f);
        SpawnObjects(grass, grassCount, 0.4f, 0.9f);
        SpawnObjects(mushrooms, mushroomCount, 0.1f, 0.6f);
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

            Vector3 position = new Vector3(randomX, terrainHeight * terrainData.size.y - 0.1f, randomZ);

            GameObject objectToSpawn = objectArray[Random.Range(0, objectArray.Length)];
            GameObject spawnedObject = Instantiate(objectToSpawn, position, Quaternion.identity);
            
            // Get terrain normal for slope alignment
            Vector3 terrainNormal = terrainData.GetInterpolatedNormal(randomX / terrainData.size.x, randomZ / terrainData.size.z);
            spawnedObject.transform.rotation = Quaternion.LookRotation(Vector3.Cross(spawnedObject.transform.right, terrainNormal), terrainNormal);

            if (System.Array.Exists(flowers, flower => flower == objectToSpawn) || System.Array.Exists(plants, plant => plant == objectToSpawn))
            {
                spawnedObject.transform.localScale *= Random.Range(1.8f, 2.5f); // Make flowers and plants larger
            }
            else if (System.Array.Exists(mushrooms, mushroom => mushroom == objectToSpawn))
            {
                spawnedObject.transform.localScale *= Random.Range(1.0f, 1.5f); // Keep mushrooms smaller
            }

            Debug.Log($"Spawning {objectToSpawn.name} at {position} with height: {terrainHeight}");
        }
    }
}

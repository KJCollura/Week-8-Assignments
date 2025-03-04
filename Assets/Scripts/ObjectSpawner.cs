using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Terrain terrain;
    public GameObject[] objectsToSpawn; // Array of objects to spawn (trees, rocks, etc.)
    public int objectCount = 100; // Number of objects to spawn

    private void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("No Terrain assigned! Attach a Terrain to the ObjectSpawner.");
            return;
        }

        PlaceObjects();
    }

    void PlaceObjects()
    {
        if (objectsToSpawn.Length == 0) return;

        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < objectCount; i++)
        {
            // Pick a random position on the terrain
            float randomX = Random.Range(0, terrainData.size.x);
            float randomZ = Random.Range(0, terrainData.size.z);
            float terrainHeight = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

            Vector3 position = new Vector3(randomX, terrainHeight, randomZ);

            // Pick a random object from the array
            GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];

            // Instantiate the object at the determined position
            Instantiate(objectToSpawn, position, Quaternion.identity);
        }
    }
}

using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 256;    // Width of the terrain
    public int height = 256;   // Depth of the terrain
    public int depth = 50;     // Maximum terrain height

    public float scale = 20f;  // Controls terrain smoothness
    public float offsetX = 100f;  // Random offset for Perlin noise
    public float offsetY = 100f;

    private Terrain terrain;

    private void Start()
    {
        // Get the Terrain component
        terrain = GetComponent<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("No Terrain component found! Attach this script to a GameObject with a Terrain.");
            return;
        }

        // Assign random offsets for variation
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);

        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;
        return Mathf.PerlinNoise(xCoord, yCoord) * 0.5f; // Keep terrain at reasonable height
    }
}

